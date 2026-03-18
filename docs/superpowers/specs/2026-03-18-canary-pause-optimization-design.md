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

**Note sur Pass 1** : au demarrage, tous les compteurs sont Unknown (MeterHealthTracker est in-memory, pas persiste). Les priorites 1, 2, 3 collapsent au meme resultat. C'est attendu — la selection intelligente est surtout utile aux passes 2 et 3.

**Changement de signature requis** : `RunSinglePassAsync` doit recevoir `successfulSerials` en parametre additionnel car il est defini dans `RunSessionAsync` (ligne 57) mais utilise dans `RunSinglePassAsync` (ligne 168). Ajouter le parametre a la signature :

```csharp
// Avant
private async Task<PassResult> RunSinglePassAsync(
    List<CompteurEquipement> meters, PassConfig passConfig, ...)

// Apres
private async Task<PassResult> RunSinglePassAsync(
    List<CompteurEquipement> meters, PassConfig passConfig,
    HashSet<string> successfulSerials, ...)
```

**Signature SelectCanaryMeter** :
```csharp
private CompteurEquipement SelectCanaryMeter(
    List<CompteurEquipement> meterList,
    HashSet<string> successfulSerials)
```

### 2. Canary multi-tentatives sur toutes les passes

**Fichier** : `ReadSessionOrchestrator.cs` lignes 336-401

Actuellement : Pass 1/2 tentent 1 seul canary. Pass 3 tente 3 canary (lignes 383-398).

**Nouveau** : toutes les passes tentent jusqu'a 3 canary differents avant d'abandonner l'IP. Les candidats suivent l'ordre de priorite du point 1. Si un canary echoue, il est retire de la liste et le suivant est teste.

**Gestion de la session TCP entre canary** : la session TCP (transport) est ouverte au niveau IP, pas au niveau compteur. Apres un echec canary, le transport peut etre dans un etat indetermine. Regle :
- Apres chaque echec canary, appeler `session.Reader?.Disconnect()` puis `session.Reader?.InitializeConnection()` avant de tenter le candidat suivant
- Si la reconnexion echoue, considerer l'IP comme inaccessible et passer au deferral

**Note** : la session TCP (`OpenTransportAsync`) est partagee par tous les compteurs d'un meme concentrateur — elle utilise l'IP du premier compteur du groupe. Tous les compteurs du groupe partagent la meme IP/port, donc le transport ouvert est valide pour n'importe quel compteur du groupe.

```
Pour chaque IP :
  session = OpenTransport(ip)
  candidats = SelectCanaryCandidates(meterList, successfulSerials, max=3)
  pour chaque candidat :
    resultat = ReadSingleMeterOnSessionAsync(candidat, canaryTimeout)
    si succes → utiliser comme canary, lire les autres compteurs
    si echec → Disconnect + Reconnect, tenter le candidat suivant
  si tous echouent ou reconnexion echoue :
    Pass < 3 → differer compteurs non-testes, IP differee
    Pass 3 → abandon definitif (compteurs echoues), BudgetExpire (jamais testes)
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

**Nouveau** : la pause est proportionnelle au nombre d'IPs distinctes dans `currentMeters` (la liste post-filtre a la ligne 95-96, apres suppression des compteurs deja lus). C'est le bon point de calcul car il reflete les IPs ayant encore des compteurs a lire pour la passe suivante.

```csharp
// Remplace la logique a la ligne 106-110
var uniqueIps = currentMeters
    .Select(m => m.Equipement?.AdresseIp)
    .Where(ip => ip != null)
    .Distinct().Count();

int actualPauseSeconds;
if (uniqueIps <= 3)
    actualPauseSeconds = 0;
else if (uniqueIps <= 10)
    actualPauseSeconds = 120;
else
    actualPauseSeconds = passConfig.PauseAfterSeconds;

if (actualPauseSeconds > 0 && currentMeters.Count > 0)
{
    _logger.LogInformation("Pause {PauseSec}s avant Pass {NextPass} ({IpCount} IPs restantes)",
        actualPauseSeconds, passIndex + 2, uniqueIps);
    await Task.Delay(TimeSpan.FromSeconds(actualPauseSeconds), ct);
    report.TotalPauseMs += actualPauseSeconds * 1000; // comptabiliser la pause reelle
}
```

**Note** : `report.TotalPauseMs` doit utiliser `actualPauseSeconds` (la pause reellement effectuee) et non `passConfig.PauseAfterSeconds` (la valeur de config).

### 5. Separation du sort des compteurs et de l'IP

**Fichier** : `ReadSessionOrchestrator.cs` lignes 373-401

Actuellement : si le canary echoue, tous les compteurs de l'IP sont differes (Pass <3) ou abandonnes (Pass 3).

**Nouveau** : quand les 3 canary echouent, on applique un deferral granulaire.

**En Pass < 3** (deferral) :
- Compteurs deja lus avec succes dans ce cycle → retires du scope (deja OK)
- Tous les autres compteurs (echoues + jamais testes) → differes pour la passe suivante
- IP differee

**En Pass 3** (final) :
- Compteurs deja lus avec succes → ignores (deja OK)
- Compteurs echoues comme canary dans cette passe → `AbandonDefinitif`
- Autres compteurs (jamais testes ou echoues dans passes precedentes) → `BudgetExpire`

**Determination "echoue comme canary"** : on maintient un `HashSet<string> failedCanarySerials` local a la boucle canary multi-tentatives. Les compteurs dans ce set sont marques `AbandonDefinitif`. Les autres sont `BudgetExpire`.

Cela evite d'avoir besoin d'un compteur de tentatives par compteur a travers les passes — on se base uniquement sur l'echec dans la passe courante.

## Fichiers impactes

| Fichier | Modification |
|---------|-------------|
| `ReadSessionOrchestrator.cs` | `SelectCanaryMeter`, `SelectCanaryCandidates`, boucle canary multi-tentatives avec Disconnect/Reconnect, signature `RunSinglePassAsync` (ajout `successfulSerials`), pauses adaptatives avec `actualPauseSeconds`, deferral granulaire avec `failedCanarySerials` |
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
| Compteurs jamais testes mais abandonnes | oui (KATI: 11) | non (differes ou BudgetExpire) |
| Taux de lecture par session | 22-52% | 50-70% estime |
