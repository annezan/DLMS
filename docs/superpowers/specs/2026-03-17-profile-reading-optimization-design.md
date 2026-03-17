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
  LastReadUpTo        datetime, NOT NULL       -- dernier timestamp de donnee lue
  LastReadAt          datetime, NOT NULL       -- quand la lecture a eu lieu
  UNIQUE(CompteurSerial, ProfileObis)
```

**Migration SQL** :

```sql
CREATE TABLE MeterProfileReadHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CompteurSerial NVARCHAR(50) NOT NULL,
    ProfileObis NVARCHAR(20) NOT NULL,
    LastReadUpTo DATETIME NOT NULL,
    LastReadAt DATETIME NOT NULL,
    CONSTRAINT UQ_MeterProfile UNIQUE (CompteurSerial, ProfileObis)
);
CREATE INDEX IX_MeterProfileReadHistory_Serial ON MeterProfileReadHistory(CompteurSerial);
```

### 2. Lecture sequentielle priorisee par profil

Au lieu de lire les 12 profils en une seule requete `ReadRowsByRange`, les lire un par un dans cet ordre avec timeout individuel :

| Priorite | Profil | OBIS | Intervalle | Timeout |
|----------|--------|------|------------|---------|
| 1 | Profil 3 | 1.0.99.3.0.255 | 24h | 30s |
| 2 | Profil 1 | 1.0.99.1.0.255 | 1h | 60s |
| 3 | Mensuel | (selon config) | mensuel | 30s |
| 4 | Profil 2 | 1.0.99.2.0.255 | 5min | 120s |
| 5 | Evenements | 0.0.99.98.0-7.255 | variable | 20s chacun |

**Regles** :
- Chaque profil a son propre timeout
- Si un profil timeout, on passe au suivant (pas d'abandon total)
- On reutilise la meme session HDLC (pas de reconnexion entre profils)
- On persiste `LastReadUpTo` pour chaque profil lu avec succes, meme si les suivants echouent

### 3. Lecture incrementale

Le `dateStart` de `ReadRowsByRange` vient de `MeterProfileReadHistory.LastReadUpTo` au lieu de `DateTime.Now.Date`.

```
Avant :  ReadRowsByRange("00:00" -> "14:00")  = 168 lignes pour Profil 2
Apres :  ReadRowsByRange("13:00" -> "14:00")  = 12 lignes pour Profil 2
```

**Fallback** : si `LastReadUpTo` est null ou date de plus de 48h, on lit les dernieres 48h max.

### 4. Suppression de la deduplication

La deduplication dans `ProcessAndSaveProfileDataAsync` (HashSet + SELECT existants) devient redondante avec la lecture incrementale. On la remplace par un `BulkInsert` avec gestion des conflits (`ON CONFLICT DO NOTHING` ou equivalent EF Core) comme filet de securite.

### 5. Flux modifie dans `ReadSingleMeterOnSessionAsync`

```
1. HDLC connect + auth (inchange)
2. Read instant data (inchange)
3. Pour chaque profil dans l'ordre de priorite :
   a. Recuperer LastReadUpTo depuis MeterProfileReadHistory
   b. Calculer dateStart = max(LastReadUpTo, now - 48h)
   c. Calculer dateEnd = DateTime.Now arrondi a l'heure
   d. ReadRowsByRange(profil, dateStart, dateEnd) avec timeout individuel
   e. Si succes -> ProcessAndSave + update LastReadUpTo = max(timestamps lus)
   f. Si timeout -> log warning, passer au profil suivant
4. HDLC disconnect
```

## Fichiers impactes

| Fichier | Modification |
|---------|-------------|
| `DLMS_SERVICE/Services/DLMSParallelReadService.cs` | Refactorer `ReadProfileDataAsync` en lecture sequentielle priorisee |
| `DLMS_COMMUNICATION/Reader/NonStaticReaderCommunication.cs` | Ajouter methode pour lire un seul profil avec timeout |
| `DLMS_DAL/` | Nouveau repository pour `MeterProfileReadHistory` |
| `DLMS_MODELS/` | Nouvelle entite `MeterProfileReadHistory` |
| `DLMS_SERVICE/Services/DLMSHardwareService.cs` | Modifier `ProcessAndSaveProfileDataAsync` — supprimer dedup, ajouter update LastReadUpTo |
| `scripts/` | Script de migration SQL pour creer la table |

## Ce qui ne change pas

- Connexion HDLC et authentification
- Lecture des donnees instantanees (registres)
- Multi-pass orchestrator (sessions, passes, budget)
- Cycle manager
- Format de stockage en base des profils (`Gxdlmsprofilgenericdetail`)

## Impact attendu

| Metrique | Avant | Apres |
|----------|-------|-------|
| Volume donnees Profil 2 par lecture | 288 lignes | ~12 lignes (incremental 1h) |
| Volume donnees Profil 1 par lecture | 24 lignes | ~1-2 lignes (incremental) |
| Retard Profil 1 | ~2h | <1h |
| Retard Profil 2 | ~12h | <1h |
| Temps lecture profils par compteur | 15-120s | 5-30s |
| Profils garantis meme si timeout | 0 (tout ou rien) | Profil 3 + Profil 1 minimum |
