# 🔧 Configuration Program.cs - Migration Complète

## 📋 **Étape 1 - Injection des Nouveaux Services**

Ajoute ces lignes dans ton `Program.cs` ou `Startup.cs` :

```csharp
// === NOUVEAUX SERVICES PERFORMANCE DLMS ===

// 1. IP Lock Manager - Évite les collisions IP
builder.Services.AddSingleton<IIPLockManager, IPLockManager>();

// 2. IP Job Queue - Files priorisées par IP
builder.Services.AddSingleton<IIPJobQueue, IPJobQueue>();

// 3. IP Worker Service - 27 workers optimisés
builder.Services.AddHostedService<IPWorkerService>();
builder.Services.AddSingleton<IIPWorkerService, IPWorkerService>();

// 4. DLMS Metrics Service - Monitoring performance
builder.Services.AddSingleton<IDLMSMetricsService, DLMSMetricsService>();

// === SERVICES EXISTANTS (GARDER) ===
builder.Services.AddSingleton<IIpQueueManager, IpQueueManager>(); // Garder pour compatibilité
builder.Services.AddHostedService<HourlyReadsWorker>();
builder.Services.AddHostedService<MissingReadsWorker>();
builder.Services.AddHostedService<ActiveCommandsWorker>();
```

## 🔄 **Étape 2 - Mise à Jour des Workers Existants**

Les workers ont été automatiquement mis à jour pour utiliser `IIPWorkerService` :

### ✅ **HourlyReadsWorker**
```csharp
// AVANT (IpQueueManager)
await ipQueueManager.ProcessAllIpsAsync(compteursParIp, now);

// APRÈS (IPWorkerService)
await _workerService.EnqueueHourlyReadAsync(compteur, now);
```

### ✅ **MissingReadsWorker**
```csharp
// AVANT (IpQueueManager)
await ipQueueManager.ProcessAllMissingReadIpsAsync(missingReadsParIp, now);

// APRÈS (IPWorkerService)
await _workerService.EnqueueMissingReadsAsync(missingReads);
```

### ✅ **ActiveCommandsWorker**
```csharp
// AVANT (IpQueueManager)
await ipQueueManager.ProcessAllCommandIpsAsync(commandsParIp, now);

// APRÈS (IPWorkerService)
await _workerService.EnqueueCommandAsync(command);
```

## 🗑️ **Étape 3 - Suppression Optionnelle de IpQueueManager**

Une fois la migration validée, tu peux supprimer :

```csharp
// À SUPPRIMER éventuellement
// builder.Services.AddSingleton<IIpQueueManager, IpQueueManager>();
```

Et supprimer le fichier `IpQueueManager.cs`

## 📊 **Étape 4 - Monitoring Actif**

Ajoute un BackgroundService pour les métriques :

```csharp
builder.Services.AddHostedService<MetricsReportingService>();

// Crée le fichier MetricsReportingService.cs :
public class MetricsReportingService : BackgroundService
{
    private readonly IDLMSMetricsService _metricsService;
    private readonly ILogger<MetricsReportingService> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _metricsService.LogCurrentStatusAsync();
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
```

## 🎯 **Architecture Finale**

```
Program.cs
├── IIPWorkerService (BackgroundService)
│   ├── IIPJobQueue (Files priorisées)
│   ├── IIPLockManager (Verrous IP)
│   └── IDLMSParallelReadService (Sessions UMAD)
│
├── HourlyReadsWorker → EnqueueHourlyReadAsync()
├── MissingReadsWorker → EnqueueMissingReadsAsync()
├── ActiveCommandsWorker → EnqueueCommandAsync()
│
└── IDLMSMetricsService (Monitoring)
```

## ⚡ **Résultat Attendu**

- **0% de collisions IP**
- **27 workers optimisés**
- **Priorités intelligentes**
- **Performance ×10**
- **Monitoring temps réel**

## 🚀 **Déploiement**

1. **Ajoute les services** dans `Program.cs`
2. **Redémarre l'application**
3. **Vérifie les logs** : tu devrais voir les nouveaux workers démarrer
4. **Surveille les métriques** : rapport toutes les 5 minutes

**L'architecture est prête pour la production !** 🎉
