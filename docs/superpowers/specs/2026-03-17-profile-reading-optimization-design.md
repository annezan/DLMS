# Optimisation de la lecture des profils DLMS

**Date** : 2026-03-17
**Contexte** : Retours client — donnees de profils pas a jour, cycles trop longs, sessions inefficaces

## Probleme

Le systeme lit les 12 profils DLMS en une seule requete couvrant les 24 dernieres heures, sans suivi du dernier horodatage lu. Consequences :

- **Profil 2 (5 min)** : 288 lignes/jour relues a chaque session, timeout frequent, donnees 12h en retard
- **Profil 1 (1h)** : 24 lignes/jour, 2h de retard meme sur compteurs lus recemment
- **Profil 3 (24h)** : OK (peu de donnees)
- **Deduplication a posteriori** : on lit tout, on compare en base, on jette les doublons — travail reseau gaspille

## Design

### 1. Table `MeterProfileReadHistory`

Nouvelle table pour suivre le dernier horodatage lu avec succes par compteur et par profil.

```
MeterProfileReadHistory
  Id                  int, PK auto-increment
  CompteurSerial      nvarchar(50), NOT NULL
  ProfileObis         nvarchar(20), NOT NULL
  LastReadUpTo        datetime2, NOT NULL       -- dernier timestamp de donnee lue
  LastReadAt          datetime2, NOT NULL       -- quand la lecture a eu lieu
  RowsRead            int, NOT NULL DEFAULT 0   -- nombre de lignes lues (suivi volume)
  ReadDurationMs      bigint, NOT NULL DEFAULT 0 -- duree de lecture (tuning timeout)
  UNIQUE(CompteurSerial, ProfileObis)
```

**Migration SQL** :

```sql
CREATE TABLE MeterProfileReadHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompteurSerial NVARCHAR(50) NOT NULL,
    ProfileObis NVARCHAR(20) NOT NULL,
    LastReadUpTo DATETIME2 NOT NULL,
    LastReadAt DATETIME2 NOT NULL,
    RowsRead INT NOT NULL DEFAULT 0,
    ReadDurationMs BIGINT NOT NULL DEFAULT 0,
    CONSTRAINT UQ_MeterProfile UNIQUE (CompteurSerial, ProfileObis)
);
CREATE INDEX IX_MeterProfileReadHistory_Serial ON MeterProfileReadHistory(CompteurSerial);
```

### 2. Index unique sur `Gxdlmsprofilgenericdetail`

Prerequis a la suppression de la deduplication en memoire : ajouter un index unique sur la table existante pour eviter les doublons au niveau SQL.

**Migration SQL** (executer apres nettoyage des doublons existants) :

```sql
-- Etape 1 : supprimer les doublons existants (garder le plus recent)
WITH cte AS (
    SELECT Id,
           ROW_NUMBER() OVER (
               PARTITION BY NumeroCompteur, GxdlmsprofilgenericId, CodeObisId, DateEnr
               ORDER BY Id DESC
           ) AS rn
    FROM Gxdlmsprofilgenericdetails
)
DELETE FROM cte WHERE rn > 1;

-- Etape 2 : creer l'index unique
CREATE UNIQUE INDEX UQ_ProfileDetail_NoDup
    ON Gxdlmsprofilgenericdetails(NumeroCompteur, GxdlmsprofilgenericId, CodeObisId, DateEnr)
    WHERE DateEnr IS NOT NULL;
```

### 3. Lecture sequentielle priorisee par profil

Au lieu de lire les 12 profils en une seule requete `ReadRowsByRange`, les lire un par un dans cet ordre avec timeout individuel :

| Priorite | Profil | OBIS | Intervalle | Timeout | Fallback max |
|----------|--------|------|------------|---------|-------------|
| 1 | Profil 3 | 1.0.99.3.0.255 | 24h | 30s | 48h |
| 2 | Profil 1 | 1.0.99.1.0.255 | 1h | 60s | 48h |
| 3 | Mensuel | (selon config) | mensuel | 30s | 48h |
| 4 | Profil 2 | 1.0.99.2.0.255 | 5min | 120s | 12h |
| 5 | Evenements | 0.0.99.98.0-7.255 | variable | 20s chacun | 48h |

**Regles** :
- Chaque profil a son propre timeout
- Si un profil timeout, on passe au suivant (pas d'abandon total)
- On reutilise la meme session HDLC (pas de reconnexion entre profils)
- On persiste `LastReadUpTo` pour chaque profil lu et **sauvegarde en base avec succes**
- Si un timeout DLMS corrompt l'etat HDLC (exception Gurux), on tente un `Disconnect` + `Reconnect` avant le profil suivant. Si la reconnexion echoue, on arrete la lecture de profils pour ce compteur.

**Timeouts configurables** : les timeouts par profil et l'ordre de priorite seront stockes dans la table `ReadingConfiguration` existante pour permettre le tuning sans redeploiement.

### 4. Lecture incrementale

Le `dateStart` de `ReadRowsByRange` vient de `MeterProfileReadHistory.LastReadUpTo` au lieu de `DateTime.Now.Date`.

```
Avant :  ReadRowsByRange("00:00" -> "14:00")  = 168 lignes pour Profil 2
Apres :  ReadRowsByRange("13:00" -> "14:00")  = 12 lignes pour Profil 2
```

**Fallback par profil** : si `LastReadUpTo` est null ou trop ancien, on applique le fallback max du profil :
- Profil 2 (5min) : max 12h (= 144 lignes max, evite les lectures massives)
- Autres profils : max 48h

**Arrondi `dateEnd`** :
- Profil 2 (5min) : arrondi aux 5 minutes inferieures
- Profil 1 (1h) : arrondi a l'heure inferieure
- Profil 3 (24h) : arrondi au jour

**Garde-fou horloge compteur** : si `LastReadUpTo > DateTime.Now + 15min`, on le reset a `DateTime.Now - fallback` pour eviter les problemes de derive d'horloge du compteur.

### 5. Remplacement de la deduplication

La deduplication dans `ProcessAndSaveProfileDataAsync` (HashSet + SELECT existants) est remplacee par :
- L'index unique `UQ_ProfileDetail_NoDup` sur `Gxdlmsprofilgenericdetails` (section 2)
- `BulkInsertAsync` avec `BulkConfig { InsertIfNotExists = true }` via EFCore.BulkExtensions (deja present dans le projet)
- La dedup en memoire peut etre supprimee car l'index unique garantit l'absence de doublons

### 6. Interface modifiee — `NonStaticReaderCommunication`

La methode `ReadRowsByRangeAsync` existante itere deja `session.ReadObjects` en boucle. Pour lire un seul profil, on definit `session.ReadObjects` avec un seul OBIS avant chaque appel. Pas de nouvelle methode necessaire.

**Pattern d'utilisation** :
```
// Pour chaque profil dans l'ordre de priorite :
session.ReadObjects = ParseObjects($"{profileObis}:2");
var cts = new CancellationTokenSource(profileTimeoutMs);
var result = await reader.ReadRowsByRangeAsync(session, dateStart, dateEnd, cts.Token);
```

### 7. Interface modifiee — `ProcessAndSaveProfileDataAsync`

Nouvelle surcharge acceptant un seul profil au lieu de l'ancien format multi-profil :

```csharp
// Nouveau : traite les donnees d'un seul profil
Task ProcessAndSaveSingleProfileAsync(
    string profileData,        // JSON d'un seul profil
    string meterSerial,
    string profileObis,        // OBIS du profil traite
    DateTime dateStart,
    DateTime dateEnd
);
```

L'ancien `ProcessAndSaveProfileDataAsync` multi-profil reste intact pour compatibilite avec les chemins existants non modifies.

### 8. Semantique `MeterReadOutcome`

Un nouveau champ `ProfileResults` est ajoute a `MeterReadOutcome` :

```csharp
public class ProfileReadResult
{
    public string ProfileObis { get; set; }
    public bool Success { get; set; }
    public int RowsRead { get; set; }
    public long DurationMs { get; set; }
    public string Error { get; set; }
}

// Dans MeterReadOutcome :
public List<ProfileReadResult> ProfileResults { get; set; }
```

**Regle de succes** : le compteur est considere `Success = true` si **au moins un profil** a ete lu avec succes. Cela permet au multi-pass orchestrator de ne pas retenter un compteur ou Profil 3 + Profil 1 sont OK mais Profil 2 a timeout.

### 9. Flux modifie dans `ReadSingleMeterOnSessionAsync`

```
1. HDLC connect + auth (inchange)
2. Read instant data (inchange)
3. Pour chaque profil dans l'ordre de priorite :
   a. Recuperer LastReadUpTo depuis MeterProfileReadHistory
   b. Appliquer garde-fou horloge (reset si > now + 15min)
   c. Calculer dateStart = max(LastReadUpTo, now - fallbackMax)
   d. Calculer dateEnd = arrondi selon intervalle du profil
   e. Si dateStart >= dateEnd → skip (rien de nouveau)
   f. session.ReadObjects = ParseObjects(profil + ":2")
   g. ReadRowsByRangeAsync avec CancellationToken(profileTimeout)
   h. Si succes DLMS → ProcessAndSaveSingleProfileAsync
   i. Si succes DB → update MeterProfileReadHistory (LastReadUpTo, RowsRead, DurationMs)
   j. Si timeout DLMS → log warning, tenter reconnexion HDLC si necessaire
   k. Ajouter ProfileReadResult au MeterReadOutcome
4. HDLC disconnect
```

### 10. Chemins de code secondaires

**`ReadMeterWithExistingSessionAsync`** (DLMSParallelReadService ligne ~812) : utilise le meme pattern que `ReadSingleMeterOnSessionAsync` pour la lecture de profils (lignes 956-960). Doit etre mis a jour de la meme maniere — extraire la logique de lecture priorisee dans une methode commune `ReadProfilesSequentialAsync` appelee par les deux chemins.

**`ProcessUmadMissingReadsGroupAsync`** (lignes ~395) : les lectures de rattrapage gardent le comportement actuel (plage de dates specifique, tous les profils d'un coup) car elles ciblent des trous precis. La lecture incrementale ne s'applique pas ici.

## Fichiers impactes

| Fichier | Modification |
|---------|-------------|
| `DLMS_SERVICE/Services/DLMSParallelReadService.cs` | Extraire `ReadProfilesSequentialAsync`, modifier `ReadSingleMeterOnSessionAsync` et `ReadMeterWithExistingSessionAsync` |
| `DLMS_COMMUNICATION/Reader/NonStaticReaderCommunication.cs` | Pas de changement structurel — on reutilise `ReadRowsByRangeAsync` avec un seul OBIS dans ReadObjects |
| `DLMS_DAL/` | Nouveau repository `MeterProfileReadHistoryRepository` (CRUD + GetLastReadUpTo) |
| `DLMS_MODELS/` | Nouvelle entite `MeterProfileReadHistory` |
| `DLMS_SERVICE/Services/DLMSHardwareService.cs` | Nouvelle methode `ProcessAndSaveSingleProfileAsync`, remplacer dedup par `InsertIfNotExists` |
| `DLMS_SERVICE/Services/MultiPass/SessionModels.cs` | Ajouter `ProfileReadResult` et `ProfileResults` a `MeterReadOutcome` |
| `scripts/` | Migration SQL : table `MeterProfileReadHistory` + index unique sur `Gxdlmsprofilgenericdetails` |

## Ce qui ne change pas

- Connexion HDLC et authentification
- Lecture des donnees instantanees (registres)
- Multi-pass orchestrator (sessions, passes, budget)
- Cycle manager
- Format de stockage en base des profils (`Gxdlmsprofilgenericdetail`)
- Lectures de rattrapage (`ProcessUmadMissingReadsGroupAsync`)

## Impact attendu

| Metrique | Avant | Apres |
|----------|-------|-------|
| Volume donnees Profil 2 par lecture | 288 lignes | ~12 lignes (incremental 1h) |
| Volume donnees Profil 1 par lecture | 24 lignes | ~1-2 lignes (incremental) |
| Retard Profil 1 | ~2h | <1h |
| Retard Profil 2 | ~12h | <1h |
| Temps lecture profils par compteur | 15-120s | 5-30s |
| Profils garantis meme si timeout | 0 (tout ou rien) | Profil 3 + Profil 1 minimum |
