# Guide Rapide - Utilisation du Logging DLMS Service

## 🚀 Démarrage Rapide (5 minutes)

### 1. Vérifier l'Installation
Le service utilise maintenant **Serilog** qui est déjà configuré. Rien à installer !

### 2. Lancer le Service
```bash
cd DLMS_SERVICE
dotnet run
```

### 3. Consulter les Logs
Les logs sont automatiquement créés dans :
```
DLMS_SERVICE/Logs/
├── service-2026-01-12.log     ← Tous les logs
└── errors-2026-01-12.log      ← Seulement les erreurs
```

---

## 📖 Utilisation dans le Code

### Dans un Nouveau Service/Controller

#### 1️⃣ Ajouter le using
```csharp
using Microsoft.Extensions.Logging;
```

#### 2️⃣ Injecter le logger
```csharp
public class MonService
{
    private readonly ILogger<MonService> _logger;

    public MonService(ILogger<MonService> logger)
    {
        _logger = logger;
    }
}
```

#### 3️⃣ Utiliser le logger
```csharp
public async Task TraiterDonnees(string id)
{
    _logger.LogInformation("Début du traitement pour {Id}", id);
    
    try
    {
        // Votre code ici
        _logger.LogInformation("✓ Traitement terminé avec succès");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur lors du traitement de {Id}", id);
        throw;
    }
}
```

---

## 🎯 Les 5 Niveaux de Log

| Niveau | Quand l'utiliser | Exemple |
|--------|------------------|---------|
| **Debug** | Débogage détaillé | `_logger.LogDebug("Valeur de X: {X}", x);` |
| **Information** | Opérations normales | `_logger.LogInformation("✓ Données sauvegardées");` |
| **Warning** | Attention nécessaire | `_logger.LogWarning("Timeout, nouvelle tentative...");` |
| **Error** | Erreurs récupérables | `_logger.LogError(ex, "Échec de connexion");` |
| **Fatal** | Erreurs critiques | `Log.Fatal(ex, "Impossible de démarrer");` |

---

## ✨ Bonnes Pratiques

### ✅ À FAIRE

```csharp
// 1. Logs structurés avec placeholders
_logger.LogInformation("Traitement de {Count} éléments pour {UserId}", count, userId);

// 2. Logger les exceptions avec contexte
catch (Exception ex)
{
    _logger.LogError(ex, "Erreur lors du traitement de {Id}", id);
}

// 3. Inclure des indicateurs visuels
_logger.LogInformation("✓ Opération réussie");
_logger.LogWarning("⚠ Donnée manquante");
_logger.LogError("✗ Échec de l'opération");

// 4. Logger début et fin des opérations importantes
_logger.LogInformation("Début du traitement batch");
// ... traitement ...
_logger.LogInformation("Fin du traitement batch - {Count} éléments", count);
```

### ❌ À ÉVITER

```csharp
// 1. Concaténation de strings (non structuré)
_logger.LogInformation("Traitement de " + count + " éléments"); // ❌

// 2. Ignorer les exceptions
catch (Exception ex) { } // ❌

// 3. Logs sans contexte
_logger.LogError("Une erreur s'est produite"); // ❌ Quelle erreur ?

// 4. Logger des données sensibles
_logger.LogInformation("Mot de passe: {Password}", password); // ❌ DANGER !
```

---

## 🔍 Rechercher dans les Logs

### Avec un Éditeur de Texte
Ouvrir `service-2026-01-12.log` et rechercher :
- `[ERR]` : Toutes les erreurs
- `NumeroCompteur: "12345678"` : Logs pour un compteur spécifique
- `✗` : Toutes les opérations échouées

### Avec PowerShell
```powershell
# Toutes les erreurs du jour
Get-Content Logs\service-2026-01-12.log | Select-String "\[ERR\]"

# Logs pour un compteur spécifique
Get-Content Logs\service-2026-01-12.log | Select-String "12345678"

# Dernières 50 lignes
Get-Content Logs\service-2026-01-12.log -Tail 50
```

### Avec CMD
```cmd
# Toutes les erreurs
findstr "[ERR]" Logs\service-2026-01-12.log

# Logs pour un compteur
findstr "12345678" Logs\service-2026-01-12.log
```

---

## 📊 Exemples Complets

### Exemple 1 : Traitement Simple
```csharp
public async Task<bool> SauvegarderCompteur(Compteur compteur)
{
    _logger.LogInformation("Début de la sauvegarde du compteur {NumeroCompteur}", compteur.NumeroCompteur);
    
    try
    {
        await _repository.SaveAsync(compteur);
        _logger.LogInformation("✓ Compteur {NumeroCompteur} sauvegardé avec succès", compteur.NumeroCompteur);
        return true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "✗ Erreur lors de la sauvegarde du compteur {NumeroCompteur}", compteur.NumeroCompteur);
        return false;
    }
}
```

### Exemple 2 : Traitement en Batch avec Statistiques
```csharp
public async Task<ResultatBatch> TraiterLot(List<Compteur> compteurs)
{
    var stopwatch = Stopwatch.StartNew();
    _logger.LogInformation("Début du traitement de {Count} compteurs", compteurs.Count);
    
    int success = 0;
    int errors = 0;
    
    foreach (var compteur in compteurs)
    {
        try
        {
            await TraiterCompteur(compteur);
            success++;
            _logger.LogDebug("Compteur {Index}/{Total} traité", success + errors, compteurs.Count);
        }
        catch (Exception ex)
        {
            errors++;
            _logger.LogError(ex, "Erreur compteur {NumeroCompteur}", compteur.NumeroCompteur);
        }
    }
    
    stopwatch.Stop();
    _logger.LogInformation(
        "Traitement terminé - Réussis: {Success}/{Total}, Erreurs: {Errors}, Durée: {Duration}ms",
        success, compteurs.Count, errors, stopwatch.ElapsedMilliseconds);
    
    return new ResultatBatch { Success = success, Errors = errors };
}
```

### Exemple 3 : Validation avec Warnings
```csharp
public ValidationResult ValiderCompteur(Compteur compteur)
{
    _logger.LogDebug("Validation du compteur {NumeroCompteur}", compteur.NumeroCompteur);
    
    var result = new ValidationResult();
    
    if (string.IsNullOrEmpty(compteur.NumeroCompteur))
    {
        _logger.LogWarning("⚠ Numéro de compteur manquant");
        result.AddError("Numéro requis");
    }
    
    if (compteur.DateInstallation > DateTime.Now)
    {
        _logger.LogWarning("⚠ Date d'installation dans le futur pour {NumeroCompteur}", 
            compteur.NumeroCompteur);
        result.AddWarning("Date suspecte");
    }
    
    if (result.IsValid)
    {
        _logger.LogInformation("✓ Validation réussie pour {NumeroCompteur}", compteur.NumeroCompteur);
    }
    else
    {
        _logger.LogWarning("✗ Validation échouée pour {NumeroCompteur} - {ErrorCount} erreurs", 
            compteur.NumeroCompteur, result.Errors.Count);
    }
    
    return result;
}
```

---

## 🛠️ Configuration

### Changer le Niveau de Log Minimum

Dans `Program.cs`, méthode `ConfigureSerilog()` :

```csharp
// Production : Information et plus
.MinimumLevel.Information()

// Développement : Tout voir
.MinimumLevel.Debug()

// Production silencieuse : Erreurs seulement
.MinimumLevel.Error()
```

### Modifier la Rétention des Fichiers

```csharp
.WriteTo.File(
    path: Path.Combine(logPath, "service-.log"),
    rollingInterval: RollingInterval.Day,      // Quotidien
    retainedFileCountLimit: 30,                 // Garder 30 jours
    // ...
)
```

Options pour `rollingInterval` :
- `RollingInterval.Day` : Un fichier par jour
- `RollingInterval.Hour` : Un fichier par heure
- `RollingInterval.Month` : Un fichier par mois

---

## 🆘 Problèmes Courants

### Problème : Les logs ne s'affichent pas

**Solution 1** : Vérifier le niveau de log
```csharp
// Dans ConfigureSerilog()
.MinimumLevel.Debug()  // Au lieu de Information
```

**Solution 2** : Vérifier les permissions
```bash
# Le dossier Logs doit être accessible en écriture
icacls Logs /grant Users:F
```

### Problème : Fichiers de logs énormes

**Solution** : Réduire la verbosité
```csharp
// Passer de Debug à Information
.MinimumLevel.Information()

// Ou réduire la rétention
retainedFileCountLimit: 7  // Au lieu de 30
```

### Problème : Logs manquants à l'arrêt

**Solution** : Toujours flush à la fin
```csharp
finally
{
    Log.CloseAndFlush();
}
```

---

## 📱 Monitoring en Temps Réel

### Windows : PowerShell
```powershell
# Suivre les logs en temps réel
Get-Content Logs\service-2026-01-12.log -Wait -Tail 20
```

### Windows : CMD
```cmd
# Afficher les dernières lignes
type Logs\service-2026-01-12.log | more
```

### Avec un Outil Externe
- **BareTail** : https://www.baremetalsoft.com/baretail/
- **Notepad++** : Menu Document → Monitor (tail -f)
- **Visual Studio Code** : Extension "Log Viewer"

---

## 🎓 Mémo Rapide

| Action | Code |
|--------|------|
| Log simple | `_logger.LogInformation("Message");` |
| Log avec variable | `_logger.LogInformation("User {Id}", userId);` |
| Log d'erreur | `_logger.LogError(ex, "Message");` |
| Début/Fin | `_logger.LogInformation("Début...");` |
| Mesurer durée | `var sw = Stopwatch.StartNew();` |
| Succès | `_logger.LogInformation("✓ Réussi");` |
| Échec | `_logger.LogError("✗ Échoué");` |
| Attention | `_logger.LogWarning("⚠ Attention");` |

---

## 📞 Support

En cas de problème avec le logging :

1. **Vérifier** : Les logs dans `Logs/errors-[date].log`
2. **Rechercher** : L'erreur spécifique avec `Select-String` ou `findstr`
3. **Documenter** : Le contexte (date, heure, opération en cours)
4. **Escalader** : Avec le fichier de log complet

---

## ✅ Checklist Avant Production

- [ ] Niveau de log configuré sur `Information` (pas `Debug`)
- [ ] Rétention des fichiers définie (30 jours généraux, 90 jours erreurs)
- [ ] Rotation automatique activée (RollingInterval.Day)
- [ ] Aucune donnée sensible loggée (mots de passe, clés, etc.)
- [ ] Tous les blocs `catch` incluent un log d'erreur
- [ ] Les opérations importantes ont des logs début/fin
- [ ] `Log.CloseAndFlush()` appelé à l'arrêt du service

---

**Date de création** : 12 janvier 2026  
**Version** : 1.0  
**Service** : DLMS_SERVICE
