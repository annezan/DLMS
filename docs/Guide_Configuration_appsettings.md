# Guide de configuration - appsettings.json (DLMS_SERVICE)

Ce document explique chaque parametre du fichier `appsettings.json` utilise par le service DLMS. Ce fichier se trouve dans le repertoire d'installation du service, a cote de l'executable `DLMS_SERVICE.exe`.

---

## 1. Connexion a la base de donnees

```json
{
  "Serveur": "10.100.2.36",
  "DB": "db_ac2526_dlmsdb_Test",
  "Env": "dev"
}
```

| Parametre | Description | Exemples |
|-----------|-------------|----------|
| **Serveur** | Adresse IP ou nom du serveur SQL Server. Pour une instance nommee, utiliser le format `NomServeur\NomInstance`. | `"10.100.2.36"`, `"DG-TEST-PG-01\\SQLEXPRESS"`, `"localhost\\SQLEXPRESS"` |
| **DB** | Nom de la base de donnees DLMS. | `"db_ac2526_dlmsdb_Test"`, `"DLMSDB"` |
| **Env** | Environnement d'execution. Permet de distinguer les contextes. | `"dev"` pour developpement/test, `"prod"` pour production |

> **Note** : Les identifiants de connexion (user/password) sont geres automatiquement par la librairie `asc_connection.dll`. Il n'est pas necessaire de les renseigner dans ce fichier.

---

## 2. Section DLMS (legacy)

```json
{
  "DLMS": {
    "TaskConsumer": {
      "WorkerCount": 15,
      "TaskTimeoutSeconds": 300
    }
  }
}
```

| Parametre | Description | Valeur par defaut |
|-----------|-------------|-------------------|
| **WorkerCount** | Nombre de taches de lecture pouvant s'executer en parallele (ancien mode). | `15` |
| **TaskTimeoutSeconds** | Duree maximale (en secondes) d'une tache individuelle de lecture avant abandon. | `300` (5 minutes) |

> **Note** : Cette section est l'ancien mode de fonctionnement. Le service utilise desormais la section **MultiPass** (voir ci-dessous) pour la lecture parallele des compteurs. Cette section peut rester presente pour compatibilite.

---

## 3. Section MultiPass - Parametres globaux

C'est la section principale qui controle la lecture multi-pass des compteurs. Le service effectue plusieurs passes successives pour maximiser le nombre de compteurs lus.

```json
{
  "MultiPass": {
    "GlobalCeilingSeconds": 3600,
    "MaxConcurrentIps": 8,
    "AdaptiveTimeoutMultiplier": 2.5,
    "MinAdaptiveTimeoutSeconds": 90,
    "PacingDelayMs": 100,
    "TcpScanTimeoutSeconds": 8,
    "Passes": [ ... ]
  }
}
```

| Parametre | Description | Valeur par defaut | Conseils |
|-----------|-------------|-------------------|----------|
| **GlobalCeilingSeconds** | Duree maximale totale (en secondes) pour l'ensemble des passes, pauses incluses. Le service arrete les passes si ce plafond est atteint. | `3600` (1 heure) | Adapter selon la fenetre horaire disponible. Si les lectures sont declenchees toutes les heures, garder une marge (ex: 3600 pour un cycle d'1h). |
| **MaxConcurrentIps** | Nombre maximum de concentrateurs (IPs) traites en parallele. | `8` | Augmenter si le reseau le supporte (ex: 10-15). Reduire si vous observez des timeouts massifs en debut de pass. |
| **AdaptiveTimeoutMultiplier** | Multiplicateur applique a la latence du canary pour calculer le timeout des lectures suivantes. | `2.5` | Ex: si le canary prend 2s, le timeout adaptatif sera 2s x 2.5 = 5s. Augmenter si les compteurs ont des temps de reponse tres variables. |
| **MinAdaptiveTimeoutSeconds** | Plancher du timeout adaptatif (en secondes). Meme si le calcul donne un timeout plus court, cette valeur minimale sera utilisee. | `90` | Empeche les timeouts trop agressifs sur des concentrateurs rapides. |
| **PacingDelayMs** | Delai (en millisecondes) entre le lancement de chaque groupe de lectures sur un concentrateur. | `100` | Permet d'eviter de surcharger le reseau avec des connexions simultanees. Augmenter (200-500) si le reseau est instable. |
| **TcpScanTimeoutSeconds** | Timeout (en secondes) du pre-scan TCP. Avant chaque pass, le service verifie la joignabilite de chaque concentrateur via une connexion TCP. | `8` | Si des concentrateurs sont lents a repondre au TCP, augmenter a 10-15s. |

---

## 4. Section MultiPass - Configuration des passes

Chaque pass est une tentative de lecture de l'ensemble des compteurs non encore lus. Le service effectue 3 passes par defaut, avec des parametres de plus en plus tolerants.

```json
{
  "Passes": [
    {
      "PassNumber": 1,
      "BudgetSeconds": 1200,
      "CanaryTimeoutSeconds": 180,
      "CachedTimeoutSeconds": 180,
      "UncachedTimeoutSeconds": 300,
      "MaxConsecutiveFailures": 2,
      "CooldownCount": 0,
      "CooldownSeconds": 0,
      "PauseAfterSeconds": 300
    }
  ]
}
```

### Description de chaque parametre de pass

| Parametre | Description |
|-----------|-------------|
| **PassNumber** | Numero de la pass (1, 2 ou 3). Doit etre sequentiel. |
| **BudgetSeconds** | Duree maximale (en secondes) allouee a cette pass. Passe a la suite ou arrete si le budget est ecoule. |
| **CanaryTimeoutSeconds** | Timeout (en secondes) pour le test canary. Le canary est la premiere lecture tentee sur un concentrateur : si elle echoue, les autres compteurs du meme concentrateur sont differes a la pass suivante. |
| **CachedTimeoutSeconds** | Timeout pour la lecture d'un compteur dont les cles de chiffrement sont deja en cache (lecture plus rapide). |
| **UncachedTimeoutSeconds** | Timeout pour la lecture d'un compteur dont les cles doivent etre recuperees en base de donnees (premiere lecture, plus lente). |
| **MaxConsecutiveFailures** | Nombre d'echecs consecutifs toleres sur un concentrateur avant de le differer (pass 1/2) ou de l'abandonner (pass 3). |
| **CooldownCount** | Nombre de pauses de recuperation autorisees apres avoir atteint MaxConsecutiveFailures, avant de differer l'IP. `0` = pas de cooldown, on differe immediatement. |
| **CooldownSeconds** | Duree (en secondes) de chaque pause de recuperation. |
| **PauseAfterSeconds** | Pause (en secondes) entre cette pass et la suivante. Permet aux concentrateurs de se stabiliser. `0` pour la derniere pass (pas de pause apres). |

### Valeurs recommandees par pass

| Parametre | Pass 1 (rapide) | Pass 2 (retry) | Pass 3 (derniere chance) |
|-----------|-----------------|-----------------|--------------------------|
| **BudgetSeconds** | `1200` (20 min) | `900` (15 min) | `600` (10 min) |
| **CanaryTimeoutSeconds** | `180` (3 min) | `300` (5 min) | `420` (7 min) |
| **CachedTimeoutSeconds** | `180` | `240` | `300` |
| **UncachedTimeoutSeconds** | `300` | `360` | `420` |
| **MaxConsecutiveFailures** | `2` | `3` | `5` |
| **CooldownCount** | `0` | `1` | `1` |
| **CooldownSeconds** | `0` | `15` | `30` |
| **PauseAfterSeconds** | `300` (5 min) | `300` (5 min) | `0` |

**Logique** : La pass 1 est stricte et rapide (lit les compteurs faciles). La pass 2 est plus tolerante (retry des echecs). La pass 3 est la derniere chance avec des timeouts eleves.

---

## 5. Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

| Parametre | Description | Valeurs possibles |
|-----------|-------------|-------------------|
| **Default** | Niveau de log par defaut de l'application. | `"Verbose"`, `"Debug"`, `"Information"`, `"Warning"`, `"Error"`, `"Fatal"` |
| **Microsoft** | Niveau de log pour les composants internes Microsoft (.NET). | Garder a `"Warning"` pour eviter le bruit. |
| **Microsoft.Hosting.Lifetime** | Logs de demarrage/arret du service. | Garder a `"Information"` pour tracer les redemarrages. |

> **Conseil** : En production, garder `"Information"`. Passer temporairement a `"Debug"` uniquement pour diagnostiquer un probleme precis, car le volume de logs augmente significativement.

Les fichiers de logs sont generes dans le sous-dossier `Logs/` du repertoire d'installation du service.

---

## 6. Autres parametres

| Parametre | Description | Valeur par defaut |
|-----------|-------------|-------------------|
| **AllowedHosts** | Restriction d'acces reseau au service. | `"*"` (toutes les machines) |

---

## 7. Exemple complet

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "DLMS": {
    "TaskConsumer": {
      "WorkerCount": 15,
      "TaskTimeoutSeconds": 300
    }
  },
  "Env": "prod",
  "DB": "db_ac2526_dlmsdb",
  "Serveur": "10.100.2.36",
  "AllowedHosts": "*",
  "MultiPass": {
    "GlobalCeilingSeconds": 3600,
    "MaxConcurrentIps": 10,
    "AdaptiveTimeoutMultiplier": 2.5,
    "MinAdaptiveTimeoutSeconds": 90,
    "PacingDelayMs": 100,
    "TcpScanTimeoutSeconds": 8,
    "Passes": [
      {
        "PassNumber": 1,
        "BudgetSeconds": 1200,
        "CanaryTimeoutSeconds": 180,
        "CachedTimeoutSeconds": 180,
        "UncachedTimeoutSeconds": 300,
        "MaxConsecutiveFailures": 2,
        "CooldownCount": 0,
        "CooldownSeconds": 0,
        "PauseAfterSeconds": 300
      },
      {
        "PassNumber": 2,
        "BudgetSeconds": 900,
        "CanaryTimeoutSeconds": 300,
        "CachedTimeoutSeconds": 240,
        "UncachedTimeoutSeconds": 360,
        "MaxConsecutiveFailures": 3,
        "CooldownCount": 1,
        "CooldownSeconds": 15,
        "PauseAfterSeconds": 300
      },
      {
        "PassNumber": 3,
        "BudgetSeconds": 600,
        "CanaryTimeoutSeconds": 420,
        "CachedTimeoutSeconds": 300,
        "UncachedTimeoutSeconds": 420,
        "MaxConsecutiveFailures": 5,
        "CooldownCount": 1,
        "CooldownSeconds": 30,
        "PauseAfterSeconds": 0
      }
    ]
  }
}
```

---

## 8. Procedure de modification

1. **Arreter le service** DLMS_SERVICE (via `services.msc` ou `sc stop DLMS_SERVICE`).
2. **Ouvrir** le fichier `appsettings.json` avec un editeur de texte (Notepad, Notepad++).
3. **Modifier** les parametres souhaites en respectant la syntaxe JSON (guillemets, virgules, accolades).
4. **Sauvegarder** le fichier.
5. **Redemarrer le service** (via `services.msc` ou `sc start DLMS_SERVICE`).
6. **Verifier** dans les logs (`Logs/`) que le service demarre correctement avec la nouvelle configuration.

> **Attention** : Toujours faire une copie de sauvegarde du fichier avant modification. Un fichier JSON mal forme empechera le demarrage du service.
