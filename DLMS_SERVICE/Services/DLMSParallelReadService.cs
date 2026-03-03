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

namespace DLMS_SERVICE.Services
{
    public interface IDLMSParallelReadService
    {
        Task ProcessUmadGroupAsync(string ip, int port, List<CompteurEquipement> meters, CancellationToken ct);
        Task ProcessUmadMissingReadsGroupAsync(string ip, int port, List<MissingReadInfo> missingReads, CancellationToken ct);
        Task ProcessUmadCommandGroupAsync(string ip, int port, List<ActiveCommandInfo> commands, CancellationToken ct);
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
        private readonly TimeSpan _readTimeout = TimeSpan.FromMinutes(3);

        public DLMSParallelReadService(
            IDLMSGuruxSessionFactory sessionFactory,
            IDLMSKeyService keyService,
            IDataProcessingServiceFactory dataProcessingServiceFactory,
            IDLMSHardwareService hardwareService,
            ILogger<DLMSParallelReadService> logger,
            IServiceProvider serviceProvider,
            IDLMSMetricsService metricsService)
        {
            _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            _keyService = keyService ?? throw new ArgumentNullException(nameof(keyService));
            _dataProcessingServiceFactory = dataProcessingServiceFactory ?? throw new ArgumentNullException(nameof(dataProcessingServiceFactory));
            _hardwareService = hardwareService ?? throw new ArgumentNullException(nameof(hardwareService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _metricsService = metricsService ?? throw new ArgumentNullException(nameof(metricsService));
        }

        public async Task ProcessUmadGroupAsync(
            string ip,
            int port,
            List<CompteurEquipement> meters,
            CancellationToken ct)
        {
            _logger.LogInformation("🚀 UMAD {Ip}:{Port} — {Count} compteurs", 
                ip, port, meters.Count);

            DLMSGuruxSession session = null;
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
                // 2️⃣ Boucle compteurs avec association DLMS individuelle
                // ===============================
                foreach (var meter in meters)
                {
                    ct.ThrowIfCancellationRequested();
                    
                    try
                    {
                        await ReadMeterWithExistingSessionAsync(
                            session,
                            meter,
                            ct,
                            ip);

                        // 🔥 pacing UMAD entre compteurs
                        await Task.Delay(200, ct);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Erreur compteur {Serial}", meter.Compteur?.NumeroCompteur);
                    }
                }
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

            DLMSGuruxSession session = null;
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
                        await Task.Delay(200, ct);
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
            DLMSGuruxSession session,
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

            DLMSGuruxSession session = null;
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
                        await Task.Delay(200, ct);
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
            DLMSGuruxSession session,
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
            DLMSGuruxSession session,
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

        private async Task ReadMeterWithExistingSessionAsync(
            DLMSGuruxSession session,
            CompteurEquipement meter,
            CancellationToken ct,
            string ip = null)
        {
            var serial = meter.Compteur?.NumeroCompteur;
            var clientAddress = "read";
            
            _logger.LogDebug("🔎 Lecture compteur {Serial}", serial);

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
            // 🔥 3️⃣ ASSOCIATION DLMS PAR COMPTEUR
            // ===============================
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
                        _logger.LogError(ex, "❌ Erreur lors de l'initialisation de la connexion DLMS pour le compteur {Serial}", serial);
                        // Ne pas bloquer - retourner directement
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
                    return; // Sort de la méthode si connexion échouée
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur lors de l'association DLMS pour le compteur {Serial}", serial);
                // Ne pas bloquer - retourner pour continuer avec les autres compteurs
                return;
            }

            // ===============================
            // 4️⃣ Lecture principale avec timeout global de 3 minutes
            // ===============================
            session.ReadObjects.Clear();
            session.ReadObjects.AddRange(ParseObjects("0.0.42.0.0.255:2;0.0.96.2.128.255:2;1.0.99.1.0.255:4;1.0.99.2.0.255:4;0.0.0.2.8.255:2;0.0.0.2.0.255:2;1.0.0.2.2.255:2"));

            // ⏱️ Timeout global pour tout le traitement du compteur (3 minutes)
            using var globalTimeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(3));
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

                // 3️⃣ Lecture des profils (sans timeout individuel)
                var profileStopwatch = System.Diagnostics.Stopwatch.StartNew();
                var now = DateTime.Now;
                var dateStart = now.Date;
                var dateEnd = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                
                session.ReadObjects.Clear();
                session.ReadObjects.AddRange(ParseObjects(
                    "1.0.99.1.0.255:2;1.0.99.2.0.255:2;1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2"));

                var profileResult = await ReadProfileDataAsync(session, dateStart, dateEnd, combinedCts.Token);
                profileStopwatch.Stop();
                
                _logger.LogInformation("⏱️ Lecture profils {Serial} terminée en {ElapsedMs}ms", serial, profileStopwatch.ElapsedMilliseconds);
                
                if (!profileResult.Success)
                {
                    _logger.LogWarning("⚠️ Profils échoués {Serial}: {Error}", serial, profileResult.ErrorMessage);
                }
                else
                {
                    var saveStopwatch = System.Diagnostics.Stopwatch.StartNew();
                    await _hardwareService.ProcessAndSaveProfileDataAsync(
                        profileResult.Data, serial);
                    saveStopwatch.Stop();
                    
                    _logger.LogInformation("⏱️ Sauvegarde profils {Serial} terminée en {ElapsedMs}ms", serial, saveStopwatch.ElapsedMilliseconds);
                    _logger.LogInformation("✅ Profils réussis {Serial}", serial);
                }

                // 📊 Enregistrer les métriques de succès
                try
                {
                    _metricsService?.RecordMeterRead(ip ?? "unknown", serial, globalStopwatch.Elapsed, true);
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
                _logger.LogWarning("⏰ Timeout global de 3 minutes atteint pour le compteur {Serial} après {ElapsedMs}ms", serial, globalStopwatch.ElapsedMilliseconds);
                
                // 📊 Enregistrer les métriques d'échec (timeout)
                try
                {
                    _metricsService?.RecordMeterRead(ip ?? "unknown", serial, globalStopwatch.Elapsed, false);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "⚠️ Erreur lors de l'enregistrement des métriques d'échec (timeout)");
                }
                
                return;
            }
        }

        private async Task<ReadResult> ReadProfileDataAsync(DLMSGuruxSession session, DateTime dateStart, DateTime dateEnd, CancellationToken ct = default)
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

        private async Task<ReadResult> ReadRowsByEntryAsync(DLMSGuruxSession session, int nombreEntree)
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
            ReadCompteurDataAsync(DLMSGuruxSession session, CompteurEquipement compteurEquipement, CancellationToken ct = default)
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

        private async Task<ReadResult> ExecuteSpecificCommandAsync(DLMSGuruxSession session, ActiveCommandInfo command)
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
