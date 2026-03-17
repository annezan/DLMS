using DLMS_COMMUNICATION;
using DLMS_COMMUNICATION.Reader;
using DLMS_DAL;
using DLMS_MODELS;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.EquipementDomain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IO.Ports;
using DLMS_SERVICE.Services.MultiPass;
using DLMS_DAL.ReadingDomainDal.Repositories.Queries;
using DLMS_DAL.ReadingDomainDal.Repositories.Commands;
using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSParallelReadService
    {
        Task ProcessUmadGroupAsync(string ip, int port, List<CompteurEquipement> meters, CancellationToken ct, DateTime? cycleStartTime = null);
        Task ProcessUmadMissingReadsGroupAsync(string ip, int port, List<MissingReadInfo> missingReads, CancellationToken ct);
        Task ProcessUmadCommandGroupAsync(string ip, int port, List<ActiveCommandInfo> commands, CancellationToken ct);

        /// <summary>
        /// Reads a single meter on an already-open UMAD session. Used by the multi-pass orchestrator.
        /// Returns a MeterReadOutcome with timing metrics and success/failure status.
        /// </summary>
        Task<MultiPass.MeterReadOutcome> ReadSingleMeterOnSessionAsync(
            IDLMSCommunicationSession session,
            CompteurEquipement meter,
            TimeSpan timeout,
            CancellationToken ct);
    }

    public class DLMSParallelReadService : IDLMSParallelReadService
    {
        private readonly IDLMSGuruxSessionFactory _sessionFactory;
        private readonly IDLMSKeyService _keyService;
        private readonly IDataProcessingServiceFactory _dataProcessingServiceFactory;
        private readonly IDLMSHardwareService _hardwareService;
        private readonly ILogger<DLMSParallelReadService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDLMSMetricsService _metricsService;
        private readonly IMeterHealthTracker _healthTracker;
        private readonly IProfileReadingConfig _profileConfig;
        private readonly TimeSpan _readTimeout = TimeSpan.FromMinutes(3);

        public DLMSParallelReadService(
            IDLMSGuruxSessionFactory sessionFactory,
            IDLMSKeyService keyService,
            IDataProcessingServiceFactory dataProcessingServiceFactory,
            IDLMSHardwareService hardwareService,
            ILogger<DLMSParallelReadService> logger,
            IServiceProvider serviceProvider,
            IDLMSMetricsService metricsService,
            IMeterHealthTracker healthTracker,
            IProfileReadingConfig profileConfig)
        {
            _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));
            _dataProcessingServiceFactory = dataProcessingServiceFactory ?? throw new ArgumentNullException(nameof(dataProcessingServiceFactory));
            _hardwareService = hardwareService ?? throw new ArgumentNullException(nameof(hardwareService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _metricsService = metricsService ?? throw new ArgumentNullException(nameof(metricsService));
            _healthTracker = healthTracker ?? throw new ArgumentNullException(nameof(healthTracker));
            _profileConfig = profileConfig ?? throw new ArgumentNullException(nameof(profileConfig));
        }

        public async Task ProcessUmadGroupAsync(
            string ip,
            int port,
            List<CompteurEquipement> meters,
            CancellationToken ct,
            DateTime? cycleStartTime = null)
        {
            _logger.LogInformation("🚀 UMAD {Ip}:{Port} — {Count} compteurs",
                ip, port, meters.Count);

            IDLMSCommunicationSession session = null;
            try
            {
                // ===============================
                // 1️⃣ Création session UMAD (transport UNIQUEMENT)
                // ===============================
                var transportParams = new DLMSConnectionParameters
                {
                    AddressIp = ip,
                    Port = port.ToString(),
                    InterfaceType = "HDLC"
                };

                session = _sessionFactory.CreateSession(transportParams);
                var connected = await session.OpenTransportAsync(ct);
                if (!connected)
                {
                    _logger.LogWarning("⚠️ UMAD inaccessible {Ip}:{Port}", ip, port);
                    return;
                }

                _logger.LogInformation("✅ Transport UMAD ouvert {Ip}:{Port}", ip, port);

                // ===============================
                // 2️⃣ Tri intelligent des compteurs par score de priorité
                // ===============================
                var orderedMeters = meters
                    .OrderBy(m => _healthTracker.GetPriorityScore(m.Compteur?.NumeroCompteur))
                    .ToList();

                _logger.LogInformation("📊 Ordre de lecture pour {Ip}: {Order}",
                    ip, string.Join(", ", orderedMeters.Take(5).Select(m =>
                    {
                        var serial = m.Compteur?.NumeroCompteur ?? "?";
                        var cat = _healthTracker.GetCategory(serial);
                        return $"{serial}({cat})";
                    })));

                // ===============================
                // 3️⃣ Budget temps par concentrateur
                // ===============================
                var cycleDeadline = (cycleStartTime ?? DateTime.Now).AddMinutes(55); // 5 min de marge
                var concentratorDegraded = false;

                // ===============================
                // 4️⃣ Test canari : tester le premier compteur avec timeout serré
                // ===============================
                if (orderedMeters.Count > 1)
                {
                    var canaryMeter = orderedMeters.First();
                    var canarySerial = canaryMeter.Compteur?.NumeroCompteur;
                    var canaryHealth = _healthTracker.GetHealthInfo(canarySerial);

                    // Canary avec timeout de 30s
                    using var canaryCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    using var canaryLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, canaryCts.Token);

                    try
                    {
                        var canaryStopwatch = System.Diagnostics.Stopwatch.StartNew();
                        await ReadMeterWithExistingSessionAsync(session, canaryMeter, canaryLinkedCts.Token, ip, canaryHealth);
                        canaryStopwatch.Stop();

                        _logger.LogInformation("🐤 Canari {Serial} OK en {ElapsedMs}ms", canarySerial, canaryStopwatch.ElapsedMilliseconds);
                        orderedMeters.RemoveAt(0); // Already processed
                    }
                    catch (OperationCanceledException) when (canaryCts.Token.IsCancellationRequested)
                    {
                        _logger.LogWarning("🐤 Canari {Serial} timeout 30s - concentrateur dégradé {Ip}", canarySerial, ip);
                        concentratorDegraded = true;
                        orderedMeters.RemoveAt(0); // Already attempted
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "🐤 Canari {Serial} échec - concentrateur dégradé {Ip}", canarySerial, ip);
                        concentratorDegraded = true;
                        orderedMeters.RemoveAt(0);
                    }

                    // 🔥 pacing UMAD entre compteurs
                    await Task.Delay(100, ct);
                }

                // ===============================
                // 5️⃣ Boucle compteurs avec timeout adaptatif et budget temps
                // ===============================
                var processedCount = 0;
                var skippedCount = 0;

                for (var i = 0; i < orderedMeters.Count; i++)
                {
                    ct.ThrowIfCancellationRequested();

                    var meter = orderedMeters[i];
                    var serial = meter.Compteur?.NumeroCompteur;
                    var remainingCount = orderedMeters.Count - i;

                    // Vérification du budget temps
                    var timeLeft = cycleDeadline - DateTime.Now;
                    if (timeLeft <= TimeSpan.FromSeconds(30))
                    {
                        skippedCount = remainingCount;
                        _logger.LogWarning("⏰ Budget épuisé pour {Ip}, {Count} compteurs non traités", ip, remainingCount);
                        break;
                    }

                    try
                    {
                        // Obtenir les paramètres adaptatifs
                        var healthInfo = _healthTracker.GetHealthInfo(serial);

                        // Si concentrateur dégradé, utiliser des timeouts ultra-courts
                        if (concentratorDegraded)
                        {
                            healthInfo = new MeterHealthInfo
                            {
                                AdaptiveTimeout = TimeSpan.FromSeconds(15),
                                WaitTime = 2000,
                                RetryCount = 1,
                                PriorityScore = healthInfo.PriorityScore,
                                Category = healthInfo.Category
                            };
                        }

                        // Timeout = min(timeout adaptatif, temps restant / compteurs restants)
                        var maxTimePerMeter = timeLeft.TotalSeconds / remainingCount;
                        var effectiveTimeout = TimeSpan.FromSeconds(
                            Math.Min(healthInfo.AdaptiveTimeout.TotalSeconds, maxTimePerMeter));

                        // Minimum 15s pour avoir une chance
                        if (effectiveTimeout < TimeSpan.FromSeconds(15))
                            effectiveTimeout = TimeSpan.FromSeconds(15);

                        _logger.LogDebug("⏱️ {Serial}: timeout={Timeout}s, catégorie={Category}, dégradé={Degraded}",
                            serial, effectiveTimeout.TotalSeconds, healthInfo.Category, concentratorDegraded);

                        await ReadMeterWithExistingSessionAsync(session, meter, ct, ip, healthInfo, effectiveTimeout);
                        processedCount++;

                        // 🔥 pacing UMAD entre compteurs
                        await Task.Delay(100, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur compteur {Serial}", serial);
                    }
                }

                _logger.LogInformation("🏁 UMAD {Ip}:{Port} terminé: {Processed} traités, {Skipped} ignorés (budget)",
                    ip, port, processedCount + 1, skippedCount); // +1 for canary
            }
            finally
            {
                session?.Dispose();
            }
        }

        public async Task ProcessUmadMissingReadsGroupAsync(
            string ip,
            int port,
            List<MissingReadInfo> missingReads,
            CancellationToken ct)
        {
            _logger.LogInformation(
                "🚀 UMAD Missing Reads {Ip}:{Port} — {Count} lectures",
                ip, port, missingReads.Count);

            IDLMSCommunicationSession session = null;
            try
            {
                // ===============================
                // 1️⃣ Création session UMAD (transport UNIQUEMENT)
                // ===============================
                var transportParams = new DLMSConnectionParameters
                {
                    AddressIp = ip,
                    Port = port.ToString(),
                    InterfaceType = "HDLC"
                };

                session = _sessionFactory.CreateSession(transportParams);
                var connected = await session.OpenTransportAsync(ct);
                if (!connected)
                {
                    _logger.LogWarning("⚠️ UMAD inaccessible {Ip}:{Port}", ip, port);
                    return;
                }

                _logger.LogInformation("✅ Transport UMAD ouvert {Ip}:{Port}", ip, port);

                // ===============================
                // 2️⃣ Grouper les lectures par compteur
                // ===============================
                var missingReadsByCompteur = missingReads
                    .GroupBy(m => new { m.NumeroCompteur })
                    .ToList();

                // ===============================
                // 3️⃣ Boucle compteurs avec association DLMS individuelle
                // ===============================
                foreach (var compteurGroup in missingReadsByCompteur)
                {
                    ct.ThrowIfCancellationRequested();
                    
                    try
                    {
                        var compteurEquipement = CreateCompteurEquipementFromInfo(compteurGroup.First());
                        
                        await ReadMissingReadsWithExistingSessionAsync(
                            session, 
                            compteurGroup.Key.NumeroCompteur, 
                            compteurGroup.ToList(), 
                            ct);
                        
                        // 🔥 pacing UMAD entre compteurs
                        await Task.Delay(100, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur lecture manquante compteur {Serial}", compteurGroup.Key.NumeroCompteur);
                    }
                }
            }
            finally
            {
                session?.Dispose();
            }
        }

        private async Task ReadMissingReadsWithExistingSessionAsync(
            IDLMSCommunicationSession session,
            string numeroCompteur,
            List<MissingReadInfo> missingReads,
            CancellationToken ct)
        {
            _logger.LogInformation("🔍 Rattrapage de {Count} heures pour le compteur {Numero}", 
                missingReads.Count, numeroCompteur);

            // ===============================
            // 🔥 1️⃣ ASSOCIATION DLMS POUR CE COMPTEUR
            // ===============================
            var clientAddress = "read";
            var keys = await _keyService.GetKeysAsync(clientAddress, numeroCompteur, "read");
            if (keys == null || !keys.IsValid)
            {
                _logger.LogWarning("⚠️ Clés invalides {Serial}", numeroCompteur);
                return;
            }

            var meterParams = new DLMSConnectionParameters
            {
                ClientAddress = clientAddress,
                SerialNumber = numeroCompteur,
                Password = keys.Password,
                AuthenticationKey = keys.AuthenticationKey,
                UnicastKey = keys.UnicastKey,
                InterfaceType = "HDLC"
            };

            // 🔴 GESTION DES ERREURS D'ASSOCIATION DLMS SANS BLOCAGE
            var connectionSuccess = false;
            try
            {
                session.InitializeMeterClient(meterParams);
                
                await Task.Run(() =>
                {
                    try
                    {
                        session.Reader!.InitializeConnection();
                        connectionSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur lors de l'initialisation de la connexion DLMS pour le compteur {Serial}", numeroCompteur);
                        // Ne pas bloquer - retourner directement
                        return;
                    }
                }, ct);

                if (connectionSuccess)
                {
                    _logger.LogInformation("✅ Association DLMS établie {Serial}", numeroCompteur);
                }
                else
                {
                    _logger.LogWarning("⚠️ Connexion DLMS échouée pour le compteur {Serial} - passage au compteur suivant", numeroCompteur);
                    return; // Sort de la méthode si connexion échouée
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur lors de l'association DLMS pour le compteur {Serial}", numeroCompteur);
                // Ne pas bloquer - retourner pour continuer avec les autres compteurs
                return;
            }

            // ===============================
            // 2️⃣ Traitement des lectures manquantes
            // ===============================
            var successCount = 0;
            var failureCount = 0;

            // Grouper les heures par plages de 12 heures pour optimiser les lectures
            var groupedHours = GroupHoursByRange(missingReads.OrderByDescending(mr => mr.MissingHour).ToList(), 12);
            
            foreach (var hourGroup in groupedHours)
            {
                try
                {
                    var groupStart = hourGroup.Last();
                    var groupEnd = hourGroup.First();
                        
                    _logger.LogInformation("⚡ Rattrapage plage de {Start} à {End} ({Count} heures) pour le compteur {compteur}", 
                        groupStart.MissingHour, groupEnd.MissingHour, hourGroup.Count, numeroCompteur);

                    // Configuration des objets pour ReadRowsByRangeAsync
                    session.ReadObjects.Clear();
                    session.ReadObjects.AddRange(ParseObjects("1.0.99.1.0.255:2;1.0.99.2.0.255:2;1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2"));

                    // Lecture des profils par plage pour le groupe d'heures
                    var dateStart = new DateTime(groupStart.MissingHour.Year, groupStart.MissingHour.Month, groupStart.MissingHour.Day, groupStart.MissingHour.Hour, 0, 0);
                    var dateEnd = new DateTime(groupEnd.MissingHour.Year, groupEnd.MissingHour.Month, groupEnd.MissingHour.Day, groupEnd.MissingHour.Hour, 0, 0);

                    var profileResult = await ReadProfileDataAsync(session, dateStart, dateEnd);
                    if (!profileResult.Success)
                    {
                        _logger.LogWarning("⚠️ Échec de la lecture des profils pour la plage {Start}-{End} du compteur {compteur}: {Error}", 
                            dateStart, dateEnd, profileResult.ErrorMessage, numeroCompteur);
                        failureCount += hourGroup.Count;
                    }
                    else
                    {
                        // Traitement des données de profil avec enregistrement via DLMSHardwareService
                        await _hardwareService.ProcessAndSaveProfileDataAsync(
                            profileResult.Data, numeroCompteur);

                        _logger.LogInformation("✅ Rattrapage réussi pour la plage {Start}-{End} ({Count} heures) pour le compteur {compteur}", 
                            dateStart, dateEnd, hourGroup.Count, numeroCompteur);
                        successCount += hourGroup.Count;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors du rattrapage de la plage pour le compteur {Numero}", numeroCompteur);
                    failureCount += hourGroup.Count;
                }
            }

            _logger.LogInformation("🏁 Lecture manquante terminée pour {Numero}: {Success} succès, {Failure} échecs", 
                numeroCompteur, successCount, failureCount);
        }

        public async Task ProcessUmadCommandGroupAsync(
            string ip,
            int port,
            List<ActiveCommandInfo> commands,
            CancellationToken ct)
        {
            _logger.LogInformation(
                "🚀 UMAD Commands {Ip}:{Port} — {Count} commandes",
                ip, port, commands.Count);

            IDLMSCommunicationSession session = null;
            try
            {
                // ===============================
                // 1️⃣ Création session UMAD (transport UNIQUEMENT)
                // ===============================
                var transportParams = new DLMSConnectionParameters
                {
                    AddressIp = ip,
                    Port = port.ToString(),
                    InterfaceType = "HDLC"
                };

                session = _sessionFactory.CreateSession(transportParams);
                var connected = await session.OpenTransportAsync(ct);
                if (!connected)
                {
                    _logger.LogWarning("⚠️ UMAD inaccessible {Ip}:{Port}", ip, port);
                    return;
                }

                _logger.LogInformation("✅ Transport UMAD ouvert {Ip}:{Port}", ip, port);

                // ===============================
                // 2️⃣ Grouper les commandes par compteur
                // ===============================
                var commandsByCompteur = commands
                    .GroupBy(cmd => new { cmd.CompteurId, cmd.NumeroCompteur, cmd.AdresseIp, cmd.Port })
                    .ToList();

                // ===============================
                // 3️⃣ Boucle compteurs avec association DLMS individuelle
                // ===============================
                foreach (var commandGroup in commandsByCompteur)
                {
                    ct.ThrowIfCancellationRequested();
                    
                    try
                    {
                        var firstCommand = commandGroup.First();
                        var compteurEquipement = CreateCompteurEquipementFromCommand(firstCommand);
                        
                        await ReadCommandsWithExistingSessionAsync(
                            session, 
                            commandGroup.ToList(), 
                            ct);
                        
                        // 🔥 pacing UMAD entre compteurs
                        await Task.Delay(100, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur commande compteur {Serial}", commandGroup.Key.NumeroCompteur);
                    }
                }

                _logger.LogInformation("✅ UMAD Commands {Ip}:{Port} terminé avec succès", ip, port);
            }
            finally
            {
                session?.Dispose();
            }
        }

        private async Task ReadCommandsWithExistingSessionAsync(
            IDLMSCommunicationSession session,
            List<ActiveCommandInfo> commands,
            CancellationToken ct)
        {
            var firstCommand = commands.First();
            var numeroCompteur = firstCommand.NumeroCompteur;
            
            // ===============================
            // 🔥 1️⃣ ASSOCIATION DLMS POUR CE COMPTEUR
            // ===============================
            var clientAddress = "read";
            var keys = await _keyService.GetKeysAsync(clientAddress, numeroCompteur, "read");
            if (keys == null || !keys.IsValid)
            {
                _logger.LogWarning("⚠️ Clés invalides {Serial}", numeroCompteur);
                return;
            }

            var meterParams = new DLMSConnectionParameters
            {
                ClientAddress = clientAddress,
                SerialNumber = numeroCompteur,
                Password = keys.Password,
                AuthenticationKey = keys.AuthenticationKey,
                UnicastKey = keys.UnicastKey,
                InterfaceType = "HDLC"
            };

            // 🔴 GESTION DES ERREURS D'ASSOCIATION DLMS SANS BLOCAGE
            var connectionSuccess = false;
            try
            {
                session.InitializeMeterClient(meterParams);
                
                await Task.Run(() =>
                {
                    try
                    {
                        session.Reader!.InitializeConnection();
                        connectionSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur lors de l'initialisation de la connexion DLMS pour le compteur {Serial}", numeroCompteur);
                        // Ne pas bloquer - retourner directement
                        return;
                    }
                }, ct);

                if (connectionSuccess)
                {
                    _logger.LogInformation("✅ Association DLMS établie {Serial}", numeroCompteur);
                }
                else
                {
                    _logger.LogWarning("⚠️ Connexion DLMS échouée pour le compteur {Serial} - passage au compteur suivant", numeroCompteur);
                    return; // Sort de la méthode si connexion échouée
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur lors de l'association DLMS pour le compteur {Serial}", numeroCompteur);
                // Ne pas bloquer - retourner pour continuer avec les autres compteurs
                return;
            }

            // ===============================
            // 2️⃣ Boucle sur les commandes du compteur
            // ===============================
            foreach (var command in commands)
            {
                try
                {
                    await ReadCommandWithExistingSessionAsync(session, command, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erreur commande {CommandId} compteur {Serial}", command.CommandId, numeroCompteur);
                }
            }
        }

        private async Task ReadCommandWithExistingSessionAsync(
            IDLMSCommunicationSession session,
            ActiveCommandInfo command,
            CancellationToken ct)
        {
            _logger.LogInformation("⚡ Exécution de la commande {CommandId} pour le compteur {Numero}", 
                command.CommandId, command.NumeroCompteur);

            // Configuration des objets de lecture selon le profil
            if (command.Numeroprofile == 1)
            {
                session.ReadObjects.Clear();
                session.ReadObjects.AddRange(ParseObjects("1.0.99.1.0.255:2"));
            }
            else if (command.Numeroprofile == 2)
            {
                session.ReadObjects.Clear();
                session.ReadObjects.AddRange(ParseObjects("1.0.99.2.0.255:2"));
            }
            else if (command.Numeroprofile == 3)
            {
                session.ReadObjects.Clear();
                session.ReadObjects.AddRange(ParseObjects("1.0.99.3.0.255:2"));
            }

            // Grouper les heures par 24h si des heures sont définies
            var heuresGroupes = new List<List<DateTime>>();
            if (command.Heures != null && command.Heures.Any())
            {
                heuresGroupes = GrouperHeures(command.Heures, 24);
                for (int i = 0; i < heuresGroupes.Count; i++)
                {
                    var hourGroup = heuresGroupes[i];
                    try
                    {
                        var groupStart = hourGroup.First();
                        var groupEnd = hourGroup.Last();

                        _logger.LogInformation("⚡ Rattrapage des commandes plage de {Start} à {End} ({Count} heures) pour le compteur {compteur}",
                            groupStart, groupEnd, hourGroup.Count, command.NumeroCompteur);

                        // Lecture des profils par plage pour le groupe d'heures
                        var dateStart = new DateTime(groupStart.Year, groupStart.Month, groupStart.Day, groupStart.Hour, 0, 0);
                        var dateEnd = new DateTime(groupEnd.Year, groupEnd.Month, groupEnd.Day, groupEnd.Hour, 0, 0);

                        var profileResult = await ReadProfileDataAsync(session, dateStart, dateEnd);
                        if (!profileResult.Success)
                        {
                            _logger.LogWarning("⚠️ Échec de la lecture des profils pour la plage {Start}-{End} du compteur {compteur}: {Error}",
                                dateStart, dateEnd, profileResult.ErrorMessage, command.NumeroCompteur);
                        }
                        else
                        {
                            // Traitement des données de profil avec enregistrement via DLMSHardwareService
                            await _hardwareService.ProcessAndSaveCommandsDataAsync(profileResult.Data, command.NumeroCompteur, command.CommandeCompteurId);
                            _logger.LogInformation("✅ Rattrapage de la commande réussi pour la plage {Start}-{End} ({Count} heures) pour le compteur {compteur}",
                                dateStart, dateEnd, hourGroup.Count, command.NumeroCompteur);
                            
                            // Vérifier si c'est le dernier hourGroup et archiver le CommandeCompteur
                            if (i == heuresGroupes.Count - 1)
                            {
                                await _hardwareService.ArchiveCommandeCompteurAsync(command.CommandeCompteurId);
                            }
                            
                            await _hardwareService.CheckAndArchiveCommandIfAllCompteursArchivedAsync(command.CommandId);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erreur lors du rattrapage de la commande de la plage pour le compteur {Numero}", command.NumeroCompteur);
                    }
                }
            }

            _logger.LogInformation("✅ Commande {CommandId} exécutée avec succès pour {Numero}", 
                command.CommandId, command.NumeroCompteur);
        }

        public async Task<MeterReadOutcome> ReadSingleMeterOnSessionAsync(
            IDLMSCommunicationSession session,
            CompteurEquipement meter,
            TimeSpan timeout,
            CancellationToken ct)
        {
            var serial = meter.Compteur?.NumeroCompteur ?? "";
            var outcome = new MeterReadOutcome
            {
                Serial = serial,
                Ip = meter.Equipement?.AdresseIp ?? "",
                Port = meter.Equipement?.Port ?? "",
                CompteurEquipementId = meter.Id,
                TimeoutApplied = (int)timeout.TotalSeconds
            };

            var totalSw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                // 1. Retrieve keys
                var keySw = System.Diagnostics.Stopwatch.StartNew();
                var keys = await _keyService.GetKeysAsync("read", serial, "read");
                keySw.Stop();
                outcome.KeysRetrievalMs = keySw.ElapsedMilliseconds;

                if (keys == null || !keys.IsValid)
                {
                    outcome.Error = "Cles DLMS invalides ou manquantes";
                    outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.CleManquante;
                    return outcome;
                }

                // 2. Initialize meter client
                var meterParams = new DLMSConnectionParameters
                {
                    ClientAddress = "read",
                    SerialNumber = serial,
                    Password = keys.Password,
                    AuthenticationKey = keys.AuthenticationKey,
                    UnicastKey = keys.UnicastKey,
                    InterfaceType = "HDLC"
                };

                // 3. HDLC association with timeout
                using var globalTimeoutCts = new CancellationTokenSource(timeout);
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, globalTimeoutCts.Token);

                var hdlcSw = System.Diagnostics.Stopwatch.StartNew();
                var connectionSuccess = false;

                session.InitializeMeterClient(meterParams, waitTime: 3000, retryCount: 1);

                await Task.Run(() =>
                {
                    try
                    {
                        session.Reader!.InitializeConnection();
                        connectionSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "Association DLMS echouee {Serial}", serial);
                    }
                }, combinedCts.Token);

                hdlcSw.Stop();
                outcome.HdlcMs = hdlcSw.ElapsedMilliseconds;

                if (!connectionSuccess)
                {
                    outcome.Error = "Association DLMS echouee";
                    outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.EchecLecture;
                    _healthTracker.RecordResult(serial, TimeSpan.Zero, false);
                    return outcome;
                }

                // 4. Read data + profiles
                session.ReadObjects.Clear();
                session.ReadObjects.AddRange(ParseObjects("0.0.42.0.0.255:2;0.0.96.2.128.255:2;1.0.99.1.0.255:4;1.0.99.2.0.255:4;0.0.0.2.8.255:2;0.0.0.2.0.255:2;1.0.0.2.2.255:2"));

                var readSw = System.Diagnostics.Stopwatch.StartNew();
                var readResult = await ReadCompteurDataAsync(session, meter, combinedCts.Token);
                readSw.Stop();
                outcome.ReadMs = readSw.ElapsedMilliseconds;

                if (!readResult.Success)
                {
                    outcome.Error = readResult.ErrorMessage;
                    outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.EchecLecture;
                    _healthTracker.RecordResult(serial, totalSw.Elapsed, false);
                    _metricsService?.RecordMeterRead(outcome.Ip, serial, totalSw.Elapsed, false);
                    return outcome;
                }

                // 5. Process data
                var dataProcessingService = _dataProcessingServiceFactory.Create();
                await dataProcessingService.ProcessCompteurDataAsync(readResult.Data, meter.CompteurId);

                // 6. Read profiles — sequential, prioritized, incremental
                var profileResults = await ReadProfilesSequentialAsync(session, serial, combinedCts.Token);
                outcome.ProfileResults = profileResults;

                // Success
                outcome.Success = true;
                outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.Lu;
                totalSw.Stop();
                outcome.TotalMs = totalSw.ElapsedMilliseconds;

                _healthTracker.RecordResult(serial, totalSw.Elapsed, true);
                _metricsService?.RecordMeterRead(outcome.Ip, serial, totalSw.Elapsed, true);
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                totalSw.Stop();
                outcome.TotalMs = totalSw.ElapsedMilliseconds;
                outcome.Error = $"Timeout ({timeout.TotalSeconds}s)";
                outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.EchecLecture;
                _healthTracker.RecordResult(serial, totalSw.Elapsed, false);
                _metricsService?.RecordMeterRead(outcome.Ip, serial, totalSw.Elapsed, false);
            }
            catch (Exception ex)
            {
                totalSw.Stop();
                outcome.TotalMs = totalSw.ElapsedMilliseconds;
                outcome.Error = ex.Message;
                outcome.ResultCategory = DLMS_MODELS.ReadingDomain.Enums.MeterReadingResult.EchecLecture;
                _healthTracker.RecordResult(serial, totalSw.Elapsed, false);
            }
            finally
            {
                try { session.Reader?.Disconnect(); } catch { }
            }

            return outcome;
        }

        private async Task ReadMeterWithExistingSessionAsync(
            IDLMSCommunicationSession session,
            CompteurEquipement meter,
            CancellationToken ct,
            string ip = null,
            MeterHealthInfo healthInfo = null,
            TimeSpan? effectiveTimeout = null)
        {
            var serial = meter.Compteur?.NumeroCompteur;
            var clientAddress = "read";

            // Utiliser le health info fourni ou obtenir un nouveau
            healthInfo ??= _healthTracker.GetHealthInfo(serial);
            var timeout = effectiveTimeout ?? healthInfo.AdaptiveTimeout;

            _logger.LogDebug("🔎 Lecture compteur {Serial} (timeout={Timeout}s, cat={Category})",
                serial, timeout.TotalSeconds, healthInfo.Category);

            // ===============================
            // 1️⃣ Récupérer clés DLMS
            // ===============================
            var keys = await _keyService.GetKeysAsync(clientAddress, serial, "read");
            if (keys == null || !keys.IsValid)
            {
                _logger.LogWarning("⚠️ Clés invalides {Serial}", serial);
                return;
            }

            // ===============================
            // 2️⃣ Paramètres compteur
            // ===============================
            var meterParams = new DLMSConnectionParameters
            {
                ClientAddress = clientAddress,
                SerialNumber = serial,
                Password = keys.Password,
                AuthenticationKey = keys.AuthenticationKey,
                UnicastKey = keys.UnicastKey,
                InterfaceType = "HDLC"
            };

            // ===============================
            // 🔥 3️⃣ ASSOCIATION DLMS PAR COMPTEUR (config dynamique via health tracker)
            // ===============================
            var connectionSuccess = false;
            try
            {
                session.InitializeMeterClient(meterParams, healthInfo.WaitTime, healthInfo.RetryCount);

                await Task.Run(() =>
                {
                    try
                    {
                        session.Reader!.InitializeConnection();
                        connectionSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur lors de l'initialisation de la connexion DLMS pour le compteur {Serial}", serial);
                        return;
                    }
                }, ct);

                if (connectionSuccess)
                {
                    _logger.LogInformation("✅ Association DLMS établie {Serial}", serial);
                }
                else
                {
                    _logger.LogWarning("⚠️ Connexion DLMS échouée pour le compteur {Serial} - passage au compteur suivant", serial);
                    _healthTracker.RecordResult(serial, TimeSpan.Zero, false);
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur lors de l'association DLMS pour le compteur {Serial}", serial);
                _healthTracker.RecordResult(serial, TimeSpan.Zero, false);
                return;
            }

            // ===============================
            // 4️⃣ Lecture principale avec timeout adaptatif
            // ===============================
            session.ReadObjects.Clear();
            session.ReadObjects.AddRange(ParseObjects("0.0.42.0.0.255:2;0.0.96.2.128.255:2;1.0.99.1.0.255:4;1.0.99.2.0.255:4;0.0.0.2.8.255:2;0.0.0.2.0.255:2;1.0.0.2.2.255:2"));

            // ⏱️ Timeout adaptatif basé sur la santé du compteur
            using var globalTimeoutCts = new CancellationTokenSource(timeout);
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, globalTimeoutCts.Token);
            
            var globalStopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                // 1️⃣ Lecture des données principales (sans timeout individuel)
                var readStopwatch = System.Diagnostics.Stopwatch.StartNew();
                var readResult = await ReadCompteurDataAsync(session, meter, combinedCts.Token);
                readStopwatch.Stop();
                
                _logger.LogInformation("⏱️ Lecture compteur {Serial} terminée en {ElapsedMs}ms", serial, readStopwatch.ElapsedMilliseconds);
                
                if (!readResult.Success)
                {
                    _logger.LogWarning("⚠️ Lecture échouée {Serial}: {Error}", serial, readResult.ErrorMessage);

                    // 📊 Enregistrer les métriques d'échec
                    try
                    {
                        _metricsService?.RecordMeterRead(ip ?? "unknown", serial, readStopwatch.Elapsed, false);
                        _healthTracker.RecordResult(serial, readStopwatch.Elapsed, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "⚠️ Erreur lors de l'enregistrement des métriques d'échec");
                    }

                    return;
                }

                // 2️⃣ Traitement des données principales (sans timeout individuel)
                var processingStopwatch = System.Diagnostics.Stopwatch.StartNew();
                var dataProcessingService = _dataProcessingServiceFactory.Create();
                var processingResult = await dataProcessingService.ProcessCompteurDataAsync(
                    readResult.Data, meter.CompteurId);
                processingStopwatch.Stop();
                
                _logger.LogInformation("⏱️ Traitement compteur {Serial} terminé en {ElapsedMs}ms", serial, processingStopwatch.ElapsedMilliseconds);
                
                if (!processingResult)
                {
                    _logger.LogWarning("⚠️ Traitement échoué {Serial}", serial);
                }
                else
                {
                    _logger.LogInformation("✅ Lecture principale réussie {Serial}", serial);
                }

                // Profile reading — sequential, prioritized, incremental
                var profileResults = await ReadProfilesSequentialAsync(session, serial, combinedCts.Token);
                var profilesRead = profileResults.Count(p => p.Success);
                _logger.LogDebug("{Serial}: {Count}/{Total} profils lus via ReadMeterWithExistingSession",
                    serial, profilesRead, profileResults.Count);

                // 📊 Enregistrer les métriques de succès
                try
                {
                    _metricsService?.RecordMeterRead(ip ?? "unknown", serial, globalStopwatch.Elapsed, true);
                    _healthTracker.RecordResult(serial, globalStopwatch.Elapsed, true);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Erreur lors de l'enregistrement des métriques de succès");
                }

                globalStopwatch.Stop();
                _logger.LogInformation("🏁 Traitement complet du compteur {Serial} terminé en {ElapsedMs}ms", serial, globalStopwatch.ElapsedMilliseconds);
            }
            catch (OperationCanceledException) when (globalTimeoutCts.Token.IsCancellationRequested)
            {
                globalStopwatch.Stop();
                _logger.LogWarning("⏰ Timeout adaptatif ({Timeout}s) atteint pour le compteur {Serial} après {ElapsedMs}ms",
                    timeout.TotalSeconds, serial, globalStopwatch.ElapsedMilliseconds);

                // 📊 Enregistrer les métriques d'échec (timeout)
                try
                {
                    _metricsService?.RecordMeterRead(ip ?? "unknown", serial, globalStopwatch.Elapsed, false);
                    _healthTracker.RecordResult(serial, globalStopwatch.Elapsed, false);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Erreur lors de l'enregistrement des métriques d'échec (timeout)");
                }

                return;
            }
        }

        private async Task<ReadResult> ReadProfileDataAsync(IDLMSCommunicationSession session, DateTime dateStart, DateTime dateEnd, CancellationToken ct = default)
        {
            // 🔴 PAS DE TIMEOUT INDIVIDUEL - utilise le timeout global passé
            try
            {
                var reader = new NonStaticReaderCommunication();
                
                // Utiliser Task.Run avec le timeout global pour éviter les blocages infinis
                var readTask = Task.Run(() => reader.ReadRowsByRangeAsync(session, dateStart.ToString(), dateEnd.ToString()), ct);
                
                // Attendre la lecture avec le timeout global
                var result = await readTask;

                return new ReadResult
                {
                    Success = !string.IsNullOrEmpty(result) && result != "Lecture impossible",
                    Data = result,
                    ErrorMessage = result == "Lecture impossible" ? "Échec de lecture des profils" : string.Empty
                };
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogWarning("⏰ Timeout global atteint lors de la lecture des profils pour la plage {Start} à {End}", 
                    dateStart, dateEnd);
                
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = "Timeout global atteint"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la lecture des profils pour la plage {Start} à {End}", 
                    dateStart, dateEnd);
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<ReadResult> ReadSingleProfileAsync(
            IDLMSCommunicationSession session,
            string profileObis,
            DateTime dateStart,
            DateTime dateEnd,
            int timeoutSeconds,
            CancellationToken ct)
        {
            try
            {
                session.ReadObjects.Clear();
                session.ReadObjects.AddRange(ParseObjects($"{profileObis}:2"));

                using var profileCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
                using var combined = CancellationTokenSource.CreateLinkedTokenSource(ct, profileCts.Token);

                var reader = new DLMS_COMMUNICATION.Reader.NonStaticReaderCommunication();
                var readTask = Task.Run(
                    () => reader.ReadRowsByRangeAsync(session, dateStart.ToString(), dateEnd.ToString()),
                    combined.Token);

                var result = await readTask;

                return new ReadResult
                {
                    Success = !string.IsNullOrEmpty(result) && result != "Lecture impossible",
                    Data = result,
                    ErrorMessage = result == "Lecture impossible" ? $"Echec lecture profil {profileObis}" : ""
                };
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                _logger.LogWarning("Timeout ({Timeout}s) lecture profil {Obis}", timeoutSeconds, profileObis);
                return new ReadResult { Success = false, Data = "", ErrorMessage = $"Timeout {timeoutSeconds}s" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lecture profil {Obis}", profileObis);
                return new ReadResult { Success = false, Data = "", ErrorMessage = ex.Message };
            }
        }

        private async Task<List<ProfileReadResult>> ReadProfilesSequentialAsync(
            IDLMSCommunicationSession session,
            string serial,
            CancellationToken ct)
        {
            var results = new List<ProfileReadResult>();
            var profiles = await _profileConfig.GetOrderedProfilesAsync();
            var now = DateTime.Now;

            // Load all history for this meter in one query
            List<MeterProfileReadHistory> histories;
            using (var scope = _serviceProvider.CreateScope())
            {
                var queryRepo = scope.ServiceProvider.GetRequiredService<IMeterProfileReadHistoryQueryRepository>();
                histories = await queryRepo.GetAllForMeterAsync(serial);
            }

            foreach (var profile in profiles)
            {
                if (ct.IsCancellationRequested) break;

                var profileResult = new ProfileReadResult { ProfileObis = profile.ProfileObis };
                var sw = System.Diagnostics.Stopwatch.StartNew();

                try
                {
                    // Calculate incremental date range
                    var history = histories.FirstOrDefault(h => h.ProfileObis == profile.ProfileObis);
                    var fallbackLimit = now.AddHours(-profile.FallbackMaxHours);

                    DateTime dateStart;
                    if (history != null)
                    {
                        // Clock drift guard
                        if (history.LastReadUpTo > now.AddMinutes(15))
                        {
                            _logger.LogWarning("Clock drift {Serial}/{Obis}: LastReadUpTo={Last} > now+15min, reset",
                                serial, profile.ProfileObis, history.LastReadUpTo);
                            dateStart = fallbackLimit;
                        }
                        else
                        {
                            dateStart = history.LastReadUpTo > fallbackLimit ? history.LastReadUpTo : fallbackLimit;
                        }
                    }
                    else
                    {
                        dateStart = fallbackLimit;
                    }

                    // Round dateEnd based on profile interval
                    DateTime dateEnd;
                    if (profile.ProfileObis == "1.0.99.2.0.255") // 5-min profile
                        dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, (now.Minute / 5) * 5, 0);
                    else if (profile.ProfileObis == "1.0.99.1.0.255") // 1-hour profile
                        dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                    else if (profile.ProfileObis == "1.0.99.3.0.255") // 24-hour profile
                        dateEnd = now.Date;
                    else
                        dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

                    // Skip if nothing new
                    if (dateStart >= dateEnd)
                    {
                        _logger.LogDebug("Skip {Obis} pour {Serial}: rien de nouveau ({Start} >= {End})",
                            profile.ProfileObis, serial, dateStart, dateEnd);
                        profileResult.Success = true;
                        profileResult.RowsRead = 0;
                        sw.Stop();
                        profileResult.DurationMs = sw.ElapsedMilliseconds;
                        results.Add(profileResult);
                        continue;
                    }

                    // Read this single profile
                    var readResult = await ReadSingleProfileAsync(session, profile.ProfileObis, dateStart, dateEnd, profile.TimeoutSeconds, ct);

                    if (readResult.Success)
                    {
                        // Save to DB
                        var rowsInserted = await _hardwareService.ProcessAndSaveSingleProfileAsync(
                            readResult.Data, serial, profile.ProfileObis);

                        sw.Stop();
                        profileResult.Success = true;
                        profileResult.RowsRead = rowsInserted;
                        profileResult.DurationMs = sw.ElapsedMilliseconds;

                        // Update LastReadUpTo after successful persistence
                        using var scope = _serviceProvider.CreateScope();
                        var cmdRepo = scope.ServiceProvider.GetRequiredService<IMeterProfileReadHistoryCommandRepository>();
                        await cmdRepo.UpsertAsync(serial, profile.ProfileObis, dateEnd, rowsInserted, sw.ElapsedMilliseconds);

                        _logger.LogDebug("Profil {Obis} lu pour {Serial}: {Rows} lignes en {Ms}ms ({Start} -> {End})",
                            profile.ProfileObis, serial, rowsInserted, sw.ElapsedMilliseconds, dateStart, dateEnd);
                    }
                    else
                    {
                        sw.Stop();
                        profileResult.Error = readResult.ErrorMessage;
                        profileResult.DurationMs = sw.ElapsedMilliseconds;

                        _logger.LogWarning("Echec profil {Obis} pour {Serial}: {Error}",
                            profile.ProfileObis, serial, readResult.ErrorMessage);

                        // If timeout, try HDLC recovery
                        if (readResult.ErrorMessage.Contains("Timeout"))
                        {
                            try
                            {
                                session.Reader?.Disconnect();
                                session.Reader?.InitializeConnection();
                                _logger.LogDebug("HDLC reconnecte apres timeout profil {Obis}", profile.ProfileObis);
                            }
                            catch (Exception reconnEx)
                            {
                                _logger.LogWarning(reconnEx, "HDLC reconnexion echouee apres timeout {Obis}, arret profils", profile.ProfileObis);
                                results.Add(profileResult);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    profileResult.Error = ex.Message;
                    profileResult.DurationMs = sw.ElapsedMilliseconds;
                    _logger.LogError(ex, "Exception profil {Obis} pour {Serial}", profile.ProfileObis, serial);
                }

                results.Add(profileResult);
            }

            return results;
        }

        private async Task<ReadResult> ReadRowsByEntryAsync(IDLMSCommunicationSession session, int nombreEntree)
        {
            try
            {
                var reader = new NonStaticReaderCommunication();
                var result = await reader.ReadRowsByEntryAsync(session, nombreEntree);

                return new ReadResult
                {
                    Success = !string.IsNullOrEmpty(result) && result != "Lecture impossible",
                    Data = result,
                    ErrorMessage = result == "Lecture impossible" ? "Échec de lecture par entrée" : string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la lecture par entrée {Entree} pour le compteur", nombreEntree, session.Parameters.SerialNumber);
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = ex.Message
                };
            }
        }

        private List<List<MissingReadInfo>> GroupHoursByRange(List<MissingReadInfo> missingReads, int rangeHours)
        {
            var groups = new List<List<MissingReadInfo>>();
            
            if (missingReads.Count == 0)
                return groups;

            var currentGroup = new List<MissingReadInfo>();
            var groupStartHour = missingReads.First().MissingHour;

            foreach (var read in missingReads)
            {
                var hoursDifference = (read.MissingHour - groupStartHour).TotalHours;
                
                if (hoursDifference < rangeHours && currentGroup.Count < rangeHours)
                {
                    currentGroup.Add(read);
                }
                else
                {
                    if (currentGroup.Count > 0)
                    {
                        groups.Add(currentGroup);
                    }
                    
                    currentGroup = new List<MissingReadInfo> { read };
                    groupStartHour = read.MissingHour;
                }
            }

            if (currentGroup.Count > 0)
            {
                groups.Add(currentGroup);
            }

            return groups;
        }

        private List<KeyValuePair<string, int>> ParseObjects(string objectsString)
        {
            var result = new List<KeyValuePair<string, int>>();
            
            foreach (string o in objectsString.Split(new char[] { ';', ',' }))
            {
                var tmp = o.Split(new char[] { ':' });
                if (tmp.Length == 2)
                {
                    result.Add(new KeyValuePair<string, int>(tmp[0].Trim(), int.Parse(tmp[1].Trim())));
                }
            }
            
            return result;
        }
        private async Task<ReadResult> 
            ReadCompteurDataAsync(IDLMSCommunicationSession session, CompteurEquipement compteurEquipement, CancellationToken ct = default)
        {
            // 🔴 PAS DE TIMEOUT INDIVIDUEL - utilise le timeout global passé
            try
            {
                var reader = new NonStaticReaderCommunication();
                
                // Utiliser Task.Run avec le timeout global pour éviter les blocages infinis
                var readTask = Task.Run(() => reader.ReadAsync(session), ct);
                
                // Attendre la lecture avec le timeout global
                var result = await readTask;

                return new ReadResult
                {
                    Success = !string.IsNullOrEmpty(result) && result != "Lecture impossible",
                    Data = result,
                    ErrorMessage = result == "Lecture impossible" ? "Échec de lecture" : string.Empty
                };
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogWarning("⏰ Timeout global atteint lors de la lecture du compteur {Numero}", 
                    compteurEquipement.Compteur?.NumeroCompteur);
                
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = "Timeout global atteint"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la lecture des données pour {Numero}", 
                    compteurEquipement.Compteur?.NumeroCompteur);
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<ReadResult> ExecuteSpecificCommandAsync(IDLMSCommunicationSession session, ActiveCommandInfo command)
        {
            // 🔴 AJOUT DU TIMEOUT DLMS MANQUANT pour les commandes
            using var timeoutCts = new CancellationTokenSource(_readTimeout);
            
            try
            {
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token);
                
                // Implémentation spécifique selon le type de commande
                // À adapter selon vos besoins spécifiques
                _logger.LogDebug("Exécution de la commande de type {Type} avec paramètres: {Params}", 
                    command.CommandType);

                // Utiliser Task.Run avec timeout pour éviter les blocages infinis
                var commandTask = Task.Run(async () => {
                    // Simulation pour l'instant
                    await Task.Delay(1000);
                    return $"Commande {command.CommandType} exécutée avec succès";
                }, combinedCts.Token);
                
                // Attendre la commande avec timeout
                var result = await commandTask;

                return new ReadResult
                {
                    Success = true,
                    Data = result,
                    ErrorMessage = string.Empty
                };
            }
            catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
            {
                _logger.LogWarning("⏰ Timeout DLMS atteint pour la commande {CommandId} de type {Type} après {Timeout} minutes", 
                    command.CommandId, command.CommandType, _readTimeout.TotalMinutes);
                
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = $"Timeout DLMS commande après {_readTimeout.TotalMinutes} minutes"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'exécution de la commande {CommandId}", command.CommandId);
                return new ReadResult
                {
                    Success = false,
                    Data = string.Empty,
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<bool> ProcessCommandResultAsync(ActiveCommandInfo command, string result)
        {
            try
            {
                // Implémentation du traitement du résultat de commande
                // Mise à jour du statut en base, etc.
                _logger.LogDebug("Traitement du résultat de la commande {CommandId}: {Result}", 
                    command.CommandId, result);

                await Task.Delay(100); // Simulation
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement du résultat de la commande {CommandId}", command.CommandId);
                return false;
            }
        }

        private CompteurEquipement CreateCompteurEquipementFromInfo(MissingReadInfo info)
        {
            // Création d'un objet CompteurEquipement à partir de MissingReadInfo
            // À adapter selon votre modèle exact
            return new CompteurEquipement
            {
                CompteurId = info.CompteurId,
                Compteur = new Compteur
                {
                    NumeroCompteur = info.NumeroCompteur
                },
                Equipement = new Equipement
                {
                    AdresseIp = info.AdresseIp,
                    Port=info.Port,
                    SerialPort=info.SerialPort
                }
            };
        }

        private CompteurEquipement CreateCompteurEquipementFromCommand(ActiveCommandInfo command)
        {
            // Création d'un objet CompteurEquipement à partir de ActiveCommandInfo
            // À adapter selon votre modèle exact
            return new CompteurEquipement
            {
                CompteurId = command.CompteurId,
                Compteur = new Compteur
                {
                    NumeroCompteur = command.NumeroCompteur
                    // ClientAddress et SerialNumber à récupérer depuis la base si nécessaire
                },
                Equipement = new Equipement
                {
                    AdresseIp = command.AdresseIp,
                    Port=command.Port,
                    SerialPort=command.SerialPort
                }
            };
        }

        private List<List<DateTime>> GrouperHeures(List<DateTime> heures,int rangeHours)
        {
            var groupes = new List<List<DateTime>>();
            if (heures == null || !heures.Any())
                return groupes;

            var groupeActuel = new List<DateTime>();
            
            foreach (var heure in heures)
            {
                groupeActuel.Add(heure);
                
                // Si le groupe atteint 12 heures, l'ajouter à la liste et en créer un nouveau
                if (groupeActuel.Count == rangeHours)
                {
                    groupes.Add(new List<DateTime>(groupeActuel));
                    groupeActuel.Clear();
                }
            }
            
            // Ajouter le dernier groupe s'il n'est pas vide
            if (groupeActuel.Any())
            {
                groupes.Add(groupeActuel);
            }
            
            // Log des groupes créés
            //for (int i = 0; i < groupes.Count; i++)
            //{
            //    var heuresString = groupes[i].Select(h => h.ToString("HH:mm")).ToList();
            //    _logger.LogInformation("📅 Groupe {Groupe}: {Heures}", i + 1, string.Join(", ", heuresString));
            //}
            
            return groupes;
        }

    }

    public class ReadResult
    {
        public bool Success { get; set; }
        public string Data { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
