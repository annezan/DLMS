# Optimisation du canary, timeouts et pauses

**Date** : 2026-03-18
**Contexte** : Tests par concentrateur (BADALA, BALKOU, KATI, SOTUBA) — taux de lecture 22-52%. Le canary est mal selectionne, les timeouts trop eleves, les pauses inutiles, et un canary en echec condamne tous les compteurs de l'IP.

## Problemes

1. **Canary choisi parmi les compteurs deja echoues** — Aux passes 2/3, le premier compteur (canary) a souvent echoue en passe precedente. S'il echoue encore, toute l'IP est differee/abandonnee, meme si d'autres compteurs auraient repondu.

2. **Timeout canary trop eleve** — Pass 1: 180s, Pass 2: 300s, Pass 3: 420s. Logs montrent des canary a 308s (BALKOU) et 261s (SOTUBA) qui consomment 30-50% du budget d'une passe.

3. **Pauses fixes de 5 min** — Appliquees meme avec 1 seul concentrateur. KATI: 0.7 min de lecture + 10 min de pauses.

4. **Un canary en echec condamne toute l'IP** — KATI Pass 2: 3 canary echoues = 11 compteurs jamais testes en abandon definitif.

## Design

### 1. Selection intelligente du canary

**Fichier** : `ReadSessionOrchestrator.cs` ligne 337

Actuellement : `canaryMeter = meterList[0]` (premier apres tri par cache).

**Nouveau** : methode `SelectCanaryMeter` avec logique de priorite :

```
Priorite 1 : compteur deja lu avec succes dans une passe precedente du meme cycle
Priorite 2 : compteur jamais teste (categorie Unknown dans MeterHealthTracker)
Priorite 3 : compteur avec cache d'association (Fast/Medium)
Priorite 4 : compteur deja echoue (comportement actuel, dernier recours)
```

Le `successfulSerials` (HashSet deja maintenu ligne 93) est passe au selecteur.

**Signature** :
```csharp
private CompteurEquipement SelectCanaryMeter(
    List<CompteurEquipement> meterList,
    HashSet<string> successfulSerials)
```

### 2. Canary multi-tentatives sur toutes les passes

**Fichier** : `ReadSessionOrchestrator.cs` lignes 336-401

Actuellement : Pass 1/2 tentent 1 seul canary. Pass 3 tente 3 canary (lignes 383-398).

**Nouveau** : toutes les passes tentent jusqu'a 3 canary differents avant d'abandonner l'IP. Les candidats suivent l'ordre de priorite du point 1. Si un canary echoue, il est retire de la liste et le suivant est teste.

```
Pour chaque IP :
  candidats = SelectCanaryCandidates(meterList, successfulSerials, max=3)
  pour chaque candidat :
    resultat = ReadSingleMeterOnSessionAsync(candidat, canaryTimeout)
    si succes → utiliser comme canary, lire les autres compteurs
    si echec → tenter le candidat suivant
  si tous echouent :
    Pass < 3 → differer IP et compteurs non-testes
    Pass 3 → abandon definitif
```

**Signature** :
```csharp
private List<CompteurEquipement> SelectCanaryCandidates(
    List<CompteurEquipement> meterList,
    HashSet<string> successfulSerials,
    int maxCandidates = 3)
```

### 3. Timeout canary reduit

**Fichier** : `scripts/migrate-multipass.sql` ou `ReadingConfiguration` en base

Modifier les valeurs de `CanaryTimeoutSeconds` dans la config des passes :

| Passe | Avant | Apres |
|-------|-------|-------|
| Pass 1 | 180s | **60s** |
| Pass 2 | 300s | **90s** |
| Pass 3 | 420s | **90s** |

**Implementation** : mise a jour SQL dans `ReadingConfiguration` ou dans `appsettings.json` section `MultiPass.Passes[].CanaryTimeoutSeconds`.

### 4. Pauses adaptatives

**Fichier** : `ReadSessionOrchestrator.cs` lignes 106-110

Actuellement : pause fixe = `passConfig.PauseAfterSeconds` (300s = 5 min).

**Nouveau** : la pause est proportionnelle au nombre d'IPs distinctes restantes dans le scope.

```csharp
// Remplace Task.Delay(passConfig.PauseAfterSeconds)
var uniqueIps = currentMeters
    .Select(m => m.Equipement?.AdresseIp)
    .Distinct().Count();

int pauseSeconds;
if (uniqueIps <= 3)
    pauseSeconds = 0;           // pas de pause pour peu d'IPs
else if (uniqueIps <= 10)
    pauseSeconds = 120;         // 2 min
else
    pauseSeconds = passConfig.PauseAfterSeconds; // 5 min (defaut)
```

**Condition supplementaire** : si `pauseSeconds > 0`, on garde le `Task.Delay` et le log. Sinon on skip.

### 5. Separation du sort des compteurs et de l'IP

**Fichier** : `ReadSessionOrchestrator.cs` lignes 373-401

Actuellement : si le canary echoue, tous les compteurs de l'IP sont differes (Pass <3) ou abandonnes (Pass 3).

**Nouveau** : quand les 3 canary echouent en Pass <3, on ne differe pas aveuglementous les compteurs. On distingue :

- **Compteurs deja lus avec succes** dans ce cycle → retires du scope (deja OK, pas besoin de retenter)
- **Compteurs echoues (canary compris)** → differes pour la passe suivante
- **Compteurs jamais testes** → differes pour la passe suivante (pas abandonnes)

En Pass 3, si les 3 canary echouent :
- Les compteurs deja echoues 2+ fois → abandon definitif
- Les compteurs jamais testes → marques `BudgetExpire` au lieu de `AbandonDefinitif` (plus clair pour le suivi)

## Fichiers impactes

| Fichier | Modification |
|---------|-------------|
| `ReadSessionOrchestrator.cs` | `SelectCanaryMeter`, `SelectCanaryCandidates`, boucle canary multi-tentatives, pauses adaptatives, logique de deferral granulaire |
| `MultiPassConfig.cs` ou `appsettings.json` | Reduction des `CanaryTimeoutSeconds` |
| `scripts/` | Script SQL pour mettre a jour les timeouts en base si stockes dans `ReadingConfiguration` |

## Ce qui ne change pas

- Le MeterHealthTracker et ses categories
- Le budget tracking et PassBudget
- Le pool adaptatif et la concurrence par IP
- La lecture des compteurs (ReadSingleMeterOnSessionAsync)
- Le cycle manager et la persistence des sessions
- La lecture des profils (optimisation precedente)

## Impact attendu

| Metrique | Avant | Apres |
|----------|-------|-------|
| Temps perdu par canary lent | 180-420s | 60-90s max |
| Compteurs en abandon definitif (1 concentrateur) | 50-60% | <20% |
| Pauses inutiles (1 concentrateur) | 10 min | 0 min |
| Compteurs jamais testes mais abandonnes | oui (KATI: 11) | non (differes, pas abandonnes) |
| Taux de lecture par session | 22-52% | 50-70% estime |
