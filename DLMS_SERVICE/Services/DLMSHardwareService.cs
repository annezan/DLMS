using DLMS_DAL;
using DLMS_DAL.CodeObisDomainDal.Repositories.Queries;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using DLMS_DAL.EventDomainDal.Repositories.Queries;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.EventsDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSHardwareService
    {
        Task<DLMSReadResult> ReadInstantAsync(DLMSReadRequest request);
        Task<DLMSReadResult> ReadProfileAsync(DLMSReadRequest request);
        Task ProcessAndSaveProfileDataAsync(string data, string serialNumber);
        Task<int> ProcessAndSaveSingleProfileAsync(string data, string serialNumber, string profileObis);
        Task ProcessAndSaveCommandsDataAsync(string data, string serialNumber, int commandeCompteurId);
        Task<int> GetNextTentativeNumberAsync(int commandeCompteurId);
        Task<bool> CheckAndArchiveCommandIfAllCompteursArchivedAsync(int commandeId);
        Task ArchiveCommandeCompteurAsync(int commandeCompteurId);
    }

    public class DLMSReadRequest
    {
        public string? Port { get; set; }
        public string? SerialPort { get; set; }
        public string? AddressIp { get; set; }
        public string ClientAddress { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string InterfaceType { get; set; } = "HDLC";
        public string Objects { get; set; } = string.Empty;
        public DLMSKeys Keys { get; set; } = new();
    }

    public class DLMSReadResult
    {
        public bool Success { get; set; }
        public string Data { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public int EntryCount { get; set; }
    }

    public class DLMSHardwareService : IDLMSHardwareService
    {
        private readonly ILogger<DLMSHardwareService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const int BatchSize = 500;
        public DLMSHardwareService(ILogger<DLMSHardwareService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public async Task<DLMSReadResult> ReadInstantAsync(DLMSReadRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                _logger.LogDebug("Lecture instantanée pour le compteur {SerialNumber}", request.SerialNumber);

                if (!request.Keys.IsValid)
                {
                    return new DLMSReadResult
                    {
                        Success = false,
                        ErrorMessage = "Clés DLMS invalides",
                        Duration = stopwatch.Elapsed
                    };
                }

                var result = DLMS_COMMUNICATION.Reader.ReaderCommunication.Read(
                    request.Port,
                    request.SerialPort,
                    request.AddressIp,
                    request.ClientAddress,
                    request.SerialNumber,
                    request.InterfaceType,
                    request.Keys.Password,
                    request.Keys.AuthenticationKey,
                    request.Keys.UnicastKey,
                    request.Objects);

                stopwatch.Stop();

                var success = result != "Lecture impossible";

                return new DLMSReadResult
                {
                    Success = success,
                    Data = result,
                    Duration = stopwatch.Elapsed,
                    ErrorMessage = success ? string.Empty : "Lecture impossible"
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Erreur lors de la lecture instantanée pour le compteur {SerialNumber}", request.SerialNumber);

                return new DLMSReadResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Duration = stopwatch.Elapsed
                };
            }
        }

        public async Task<DLMSReadResult> ReadProfileAsync(DLMSReadRequest request)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                _logger.LogDebug("Lecture profil pour le compteur {SerialNumber}", request.SerialNumber);

                if (!request.Keys.IsValid)
                {
                    return new DLMSReadResult
                    {
                        Success = false,
                        ErrorMessage = "Clés DLMS invalides",
                        Duration = stopwatch.Elapsed
                    };
                }

                // Utiliser la logique optimisée du DAL pour la lecture du profil
                var result = await ReadProfileWithOptimizationAsync(request);

                // Si la lecture a réussi, traiter et enregistrer les données avec la version optimisée
                if (!string.IsNullOrEmpty(result) && result != "Lecture impossible")
                {
                    await ProcessAndSaveProfileDataAsync(result, request.SerialNumber);
                }

                stopwatch.Stop();

                var success = !string.IsNullOrEmpty(result) && result != "Lecture impossible";

                return new DLMSReadResult
                {
                    Success = success,
                    Data = result,
                    Duration = stopwatch.Elapsed,
                    ErrorMessage = success ? string.Empty : "Lecture profil impossible"
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Erreur lors de la lecture profil pour le compteur {SerialNumber}", request.SerialNumber);

                return new DLMSReadResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Duration = stopwatch.Elapsed
                };
            }
        }

        private async Task ArchiveExpiredCommandAsync(Commande commande, CommandeCompteur commandeCompteur)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var commandeCompteurRepo = scope.ServiceProvider.GetRequiredService<DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands.ICommandeCompteurCommandRepository>();
                var commandeRepo = scope.ServiceProvider.GetRequiredService<DLMS_DAL.CommandeDomainDal.Repositories.Commands.ICommandeCommandRepository>();

                // Archiver le CommandeCompteur lié à cette commande
                var commandeCompteurToArchive = new CommandeCompteur
                {
                    CommandeId = commande.Id,
                    Id = 0,
                    DeletedBy = "System"
                };
                await commandeCompteurRepo.DeleteCommandeCompteur(commandeCompteurToArchive);

                // Archiver la commande elle-même
                var commandeToArchive = new Commande
                {
                    Id = commande.Id,
                    DeletedBy = "System"
                };
                await commandeRepo.DeleteCommande(commandeToArchive);

                _logger.LogInformation("Commande expirée archivée: {CommandeId}", commande.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'archivage de la commande expirée {CommandeId}", commande.Id);
            }
        }

        private async Task ArchiveProcessedCommandAsync(int commandeCompteurId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var commandeCompteurRepo = scope.ServiceProvider.GetRequiredService<DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands.ICommandeCompteurCommandRepository>();

                // Archiver le CommandeCompteur après traitement réussi
                var commandeCompteurToArchive = new CommandeCompteur
                {
                    Id = commandeCompteurId,
                    DeletedBy = "System"
                };
                await commandeCompteurRepo.DeleteCommandeCompteur(commandeCompteurToArchive);

                _logger.LogInformation("CommandeCompteur archivée après traitement: {CommandeCompteurId}", commandeCompteurId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'archivage de la commande traitée {CommandeCompteurId}", commandeCompteurId);
            }
        }

        private async Task<bool> CheckAnyProfileDataAsync(string serialNumber)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

                var hasDetails = await context.Gxdlmsprofilgenericdetails
                    .AnyAsync(x => x.NumeroCompteur == serialNumber);

                var hasEvents = await context.Gxdlmsprofilgenericdetailsevents
                    .AnyAsync(x => x.NumeroCompteur == serialNumber);

                return hasDetails || hasEvents;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Impossible de vérifier les données existantes pour {SerialNumber}", serialNumber);
                return false;
            }
        }

        private async Task<string> ReadProfileWithOptimizationAsync(DLMSReadRequest request)
        {
            try
            {
                // Vérifier s'il existe des données (sans vérifier la date)
                //var hasData = await CheckAnyProfileDataAsync(request.SerialNumber);

                //if (!hasData)
                //{
                //    // Première lecture : lire toutes les données depuis le début
                //    _logger.LogDebug("Première lecture du profil pour {SerialNumber}", request.SerialNumber);

                //    // Lecture incrémentale : seulement les données des 48 dernières heures
                //    var datestart = DateTime.Now.AddMonths(-2).Date.ToString();
                //    var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

                //    return DLMS_COMMUNICATION.Reader.ReaderCommunication.ReadRowsByRange(
                //        datestart, dateend.ToString(),
                //        request.Port,
                //        request.SerialPort,
                //        request.AddressIp,
                //        request.ClientAddress,
                //        request.SerialNumber,
                //        request.InterfaceType,
                //        request.Keys.Password,
                //        request.Keys.AuthenticationKey,
                //        request.Keys.UnicastKey,
                //        "1.0.99.1.0.255:2;1.0.99.2.0.255:2;1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2");


                //}
                //else
                //{
                    // Lecture incrémentale : seulement les données des 48 dernières heures
                    var datestart = DateTime.Now.Date.ToString();
                    var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

                    _logger.LogDebug("Lecture incrémentale du profil pour {SerialNumber} ({Start} à {End})",
                        request.SerialNumber, datestart, dateend);

                    return DLMS_COMMUNICATION.Reader.ReaderCommunication.ReadRowsByRange(
                        datestart, dateend.ToString(),
                        request.Port,
                        request.SerialPort,
                        request.AddressIp,
                        request.ClientAddress,
                        request.SerialNumber,
                        request.InterfaceType,
                        request.Keys.Password,
                        request.Keys.AuthenticationKey,
                        request.Keys.UnicastKey,
                        "1.0.99.1.0.255:2;1.0.99.2.0.255:2;1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2");
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la lecture optimisée du profil pour {SerialNumber}", request.SerialNumber);
                return "Lecture impossible";
            }
        }

        public async Task ProcessAndSaveProfileDataAsync(string data, string serialNumber)
        {
            try
            {
                _logger.LogDebug("Traitement et enregistrement des données de profil pour {SerialNumber}", serialNumber);

                var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(data);

                if (entries == null || entries.Count == 0)
                {
                    _logger.LogWarning("Aucune donnée de profil à traiter pour {SerialNumber}", serialNumber);
                    return;
                }

                var profiles = new[]
                {
                "1.0.99.1.0.255", "1.0.99.2.0.255", "1.0.99.3.0.255",
                "0.0.98.1.0.255", "0.0.99.98.0.255", "0.0.99.98.1.255",
                "0.0.99.98.2.255", "0.0.99.98.3.255", "0.0.99.98.4.255",
                "0.0.99.98.5.255", "0.0.99.98.6.255", "0.0.99.98.7.255"
                };

                // 🔥 CORRECTION: Créer un scope séparé pour chaque profil pour éviter la concurrence DbContext
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();
                
                // 🔥 OPTIMISATION 1: Précharger tous les profils génériques avec leur CodeObis
                var profilGenericsDict = (await context.Gxdlmsprofilgenerics
                    .Join(context.CodeObis, 
                        pg => pg.CodeObisId, 
                        co => co.Id, 
                        (pg, co) => new { ProfilGeneric = pg, CodeObis = co })
                    .Where(x => profiles.Contains(x.CodeObis.Value))
                    .ToListAsync())
                    .GroupBy(x => x.CodeObis.Value)
                    .ToDictionary(g => g.Key, g => g.First().ProfilGeneric);

                // 🔥 OPTIMISATION 2: Précharger tous les CodeObis uniques
                var allObisValues = entries
                    .SelectMany(e => e.Value.Select(v => v?.ToString()))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                var codeObisDict = (await context.CodeObis
                    .Where(c => allObisValues.Contains(c.Value))
                    .ToListAsync())
                    .GroupBy(c => c.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // 🔥 OPTIMISATION 3: Précharger tous les Events possibles
                //var eventValues = entries
                //    .Where(e => e.Value.Any(v => v?.ToString().StartsWith("0.0.96.11.") == true))
                //    .SelectMany(e => e.Value)
                //    .Select(v => 
                //    {
                //        var str = v?.ToString();
                //        return str.StartsWith("0.0.96.11.") && long.TryParse(str?.Split('.').Last(), out var val) ? val : (long?)null;
                //    })
                //    .Where(v => v.HasValue)
                //    .Select(v => v.Value)
                //    .Distinct()
                //    .ToList();

                var eventsDict = (await context.Events
                    .Where(e => e.Category == "EVENTS_GROUP_ALL_REGISTERS")
                    .ToListAsync())
                    .GroupBy(e => e.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // 🔥 DEBUG: Logging pour diagnostiquer
                //_logger.LogInformation("eventValues trouvés: {EventValues}", string.Join(", ", eventValues));
                //_logger.LogInformation("eventsDict construit avec {Count} éléments: {DictKeys}", 
                //    eventsDict.Count, string.Join(", ", eventsDict.Keys));

                // 🔥 OPTIMISATION 4: Désactiver le tracking EF
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var detailsToAdd = new List<Gxdlmsprofilgenericdetail>();
                var eventsToAdd = new List<Gxdlmsprofilgenericdetailsevent>();

                int processedCount = 0;

                foreach (var entry in entries.Take(profiles.Length))
                {
                    try
                    {
                        var profileLN = profiles[processedCount];
                        
                        // 🔥 CORRECTION: Créer un scope séparé pour chaque profil
                        using var profileScope = _serviceScopeFactory.CreateScope();
                        var profileContext = profileScope.ServiceProvider.GetRequiredService<DLMSDBContext>();
                        
                        // 🔥 OPTIMISATION 5: Utiliser le dictionnaire au lieu de la DB
                        if (!profilGenericsDict.TryGetValue(profileLN, out var profilGeneric))
                        {
                            _logger.LogWarning("Profil non trouvé pour {ProfileLN}", profileLN);
                            processedCount++;
                            continue;
                        }

                        if (entry.Key.Length > 0 && entry.Value.Length > 0)
                        {
                            DateTime dateUtc = DateTime.MinValue;
                            bool dateValid = false;

                            foreach (var row in entry.Key)
                            {
                                if (row is IEnumerable<object> values)
                                {
                                    var array = values.ToArray();
                                    dateValid = false;

                                    for (int i = 0; i < entry.Value.Length; i++)
                                    {
                                        var objStr = entry.Value[i]?.ToString() ?? string.Empty;

                                        if (processedCount < 4)
                                        {
                                            // Création objet detailprofil
                                            var detailprofil = new Gxdlmsprofilgenericdetail();
                                            var realValue = ((JValue)array[i]).Value;
                                            bool isRegisterValue = realValue is decimal || realValue is double || realValue is float || realValue is int || realValue is long || realValue is Int64 || realValue is Int32;
                                            if (i == 0)
                                            {
                                                var dateStr = array[i]?.ToString() ?? "";
                                                if (string.IsNullOrEmpty(dateStr) || dateStr == "[]")
                                                {
                                                    break; // Séparateur inter-profil, skip silencieusement
                                                }
                                                else if (DateTime.TryParse(dateStr, out DateTime dateValue))
                                                {
                                                    long unixTimestamp = ((DateTimeOffset)dateValue).ToUnixTimeSeconds();
                                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                    dateValid = true;
                                                }
                                                else if (long.TryParse(dateStr, out long unixTs) && unixTs > 946684800)
                                                {
                                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTs).UtcDateTime;
                                                    dateValid = true;
                                                }
                                                else
                                                {
                                                    _logger.LogWarning("Format de date invalide pour {Serial}: {Date} — ligne ignorée", serialNumber, dateStr);
                                                    break; // Skip entire row
                                                }
                                            }

                                            if (!dateValid) continue;

                                            var rawStr = isRegisterValue ? Convert.ToDecimal(realValue).ToString() : realValue?.ToString();
                                            detailprofil.RawValue = rawStr;
                                            detailprofil.Value = rawStr;
                                            var resolvedCodeObisId = codeObisDict.TryGetValue(objStr, out var codeObis) ? codeObis.Id : 0;
                                            if (resolvedCodeObisId == 0)
                                            {
                                                _logger.LogWarning("CodeObis inconnu '{Obis}' pour {Serial} — ligne ignorée", objStr, serialNumber);
                                                continue;
                                            }
                                            detailprofil.CodeObisId = resolvedCodeObisId;
                                            detailprofil.DateEnr = dateUtc;
                                            detailprofil.GxdlmsprofilgenericId = profilGeneric.Id;
                                            detailprofil.NumeroCompteur = serialNumber;
                                            detailprofil.IsArchive = false;

                                            detailsToAdd.Add(detailprofil);
                                               

                                        }
                                        else
                                        {
                                            // Traitement des événements
                                            var detailprofilevent = new Gxdlmsprofilgenericdetailsevent();

                                            if (objStr.StartsWith("0.0.96.11."))
                                            {
                                                int valconvert = Convert.ToInt32(array[i]);

                                                _logger.LogDebug("Recherche événement: valconvert={Valconvert}, eventsDict.Count={Count}", 
                                                    valconvert, eventsDict.Count);
                                                
                                                if (eventsDict.TryGetValue(valconvert, out var eventResult))
                                                {
                                                    detailprofilevent.EventId = eventResult?.Id;
                                                    _logger.LogDebug("Événement trouvé: Id={EventId}, Description={Description}", 
                                                        eventResult?.Id, eventResult?.Code2);
                                                }
                                                else
                                                {
                                                    _logger.LogWarning("Événement non trouvé pour la valeur: {Valconvert}", valconvert);
                                                    detailprofilevent.EventId = null;
                                                }
                                                detailprofilevent.Value = array[i]?.ToString() ?? string.Empty;
                                                detailprofilevent.RawValue = detailprofilevent.Value;
                                            }
                                            else
                                            {
                                                detailprofilevent.Value = array[i]?.ToString() ?? string.Empty;
                                                detailprofilevent.RawValue = detailprofilevent.Value;
                                                detailprofilevent.EventId = null;
                                            }

                                            if (i == 0)
                                            {
                                                //long unixTimestamp = Convert.ToUInt32(array[i]);
                                                //dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                if (DateTime.TryParse(array[i].ToString(), out DateTime dateValue))
                                                {
                                                    long unixTimestamp = ((DateTimeOffset)dateValue).ToUnixTimeSeconds();
                                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                }
                                                else
                                                {
                                                    // Gérer le cas où la conversion échoue
                                                    _logger.LogWarning("Format de date invalide: {Date}", array[i]);
                                                    continue;
                                                }
                                            }

                                            detailprofilevent.DateEnr = dateUtc;
                                            detailprofilevent.CodeObisId = codeObisDict.TryGetValue(objStr, out var codeObis) ? codeObis.Id : 0;
                                            detailprofilevent.GxdlmsprofilgenericId = profilGeneric.Id;
                                            detailprofilevent.NumeroCompteur = serialNumber;
                                            detailprofilevent.IsArchive = false;

                                            eventsToAdd.Add(detailprofilevent);
                                        }

                                        // 🔥 OPTIMISATION 6: BulkInsert - plus de sauvegardes dans les boucles
                                        // Les données seront sauvegardées en une seule fois à la fin avec BulkInsert
                                    }
                                }
                            }
                        }

                        // BulkInsertOrUpdate — élimine les race conditions (duplicate key)
                        if (detailsToAdd.Any())
                        {
                            await profileContext.BulkInsertOrUpdateAsync(detailsToAdd, new BulkConfig
                            {
                                SetOutputIdentity = false,
                                BatchSize = BatchSize,
                                UpdateByProperties = new List<string>
                                {
                                    nameof(Gxdlmsprofilgenericdetail.NumeroCompteur),
                                    nameof(Gxdlmsprofilgenericdetail.GxdlmsprofilgenericId),
                                    nameof(Gxdlmsprofilgenericdetail.CodeObisId),
                                    nameof(Gxdlmsprofilgenericdetail.DateEnr)
                                },
                                PropertiesToExcludeOnUpdate = new List<string>
                                {
                                    nameof(Gxdlmsprofilgenericdetail.Value),
                                    nameof(Gxdlmsprofilgenericdetail.RawValue),
                                    nameof(Gxdlmsprofilgenericdetail.IsArchive)
                                }
                            });
                            detailsToAdd.Clear();
                        }
                        if (eventsToAdd.Any())
                        {
                            await profileContext.BulkInsertOrUpdateAsync(eventsToAdd, new BulkConfig
                            {
                                SetOutputIdentity = false,
                                BatchSize = BatchSize,
                                UpdateByProperties = new List<string>
                                {
                                    nameof(Gxdlmsprofilgenericdetailsevent.NumeroCompteur),
                                    nameof(Gxdlmsprofilgenericdetailsevent.GxdlmsprofilgenericId),
                                    nameof(Gxdlmsprofilgenericdetailsevent.CodeObisId),
                                    nameof(Gxdlmsprofilgenericdetailsevent.DateEnr)
                                },
                                PropertiesToExcludeOnUpdate = new List<string>
                                {
                                    nameof(Gxdlmsprofilgenericdetailsevent.Value),
                                    nameof(Gxdlmsprofilgenericdetailsevent.RawValue),
                                    nameof(Gxdlmsprofilgenericdetailsevent.IsArchive),
                                    nameof(Gxdlmsprofilgenericdetailsevent.EventId)
                                }
                            });
                            eventsToAdd.Clear();
                        }

                       // _logger.LogInformation("Profil traité et enregistré: {Processed} entrées pour {SerialNumber}", processedCount + 1, serialNumber);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Erreur traitement entrée profil {Index} pour {SerialNumber}", processedCount, serialNumber);
                    }
                    
                    processedCount++;
                }

                // 🔥 OPTIMISATION 7: Réactiver le tracking EF
                context.ChangeTracker.AutoDetectChangesEnabled = true;
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

                _logger.LogInformation("Profil traité et enregistré: {Processed} entrées pour {SerialNumber}", processedCount, serialNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement et de l'enregistrement des données de profil pour {SerialNumber}", serialNumber);
            }
        }

        public async Task<int> ProcessAndSaveSingleProfileAsync(string data, string serialNumber, string profileObis)
        {
            try
            {
                _logger.LogDebug("Traitement profil unique {ProfileObis} pour {SerialNumber}", profileObis, serialNumber);

                var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(data);

                if (entries == null || entries.Count == 0)
                {
                    _logger.LogWarning("Aucune donnée de profil à traiter pour {SerialNumber} / {ProfileObis}", serialNumber, profileObis);
                    return 0;
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

                // Lookup profile generic via Join on CodeObis
                var profilGenericResult = await context.Gxdlmsprofilgenerics
                    .Join(context.CodeObis,
                        pg => pg.CodeObisId,
                        co => co.Id,
                        (pg, co) => new { ProfilGeneric = pg, CodeObisValue = co.Value })
                    .FirstOrDefaultAsync(x => x.CodeObisValue == profileObis);

                if (profilGenericResult == null)
                {
                    _logger.LogWarning("Profil générique non trouvé pour {ProfileObis}", profileObis);
                    return 0;
                }

                var profilGeneric = profilGenericResult.ProfilGeneric;

                // Preload OBIS codes from entry.Value
                var allObisValues = entries
                    .SelectMany(e => e.Value.Select(v => v?.ToString()))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                var codeObisDict = (await context.CodeObis
                    .Where(c => allObisValues.Contains(c.Value))
                    .ToListAsync())
                    .GroupBy(c => c.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // Preload events
                var eventsDict = (await context.Events
                    .Where(e => e.Category == "EVENTS_GROUP_ALL_REGISTERS")
                    .ToListAsync())
                    .GroupBy(e => e.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // Disable EF tracking
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                // Determine if register or event profile
                var registerProfiles = new HashSet<string> { "1.0.99.1.0.255", "1.0.99.2.0.255", "1.0.99.3.0.255", "0.0.98.1.0.255" };
                bool isRegisterProfile = registerProfiles.Contains(profileObis);

                var detailsToAdd = new List<Gxdlmsprofilgenericdetail>();
                var eventsToAdd = new List<Gxdlmsprofilgenericdetailsevent>();

                foreach (var entry in entries)
                {
                    if (entry.Key.Length == 0 || entry.Value.Length == 0)
                        continue;

                    foreach (var row in entry.Key)
                    {
                        if (row is IEnumerable<object> values)
                        {
                            var array = values.ToArray();

                            // Étape 1 : extraire la date de la première colonne AVANT de traiter les données
                            DateTime dateUtc;
                            bool dateValid = false;
                            if (array.Length > 0)
                            {
                                var dateStr = array[0]?.ToString() ?? "";
                                if (DateTime.TryParse(dateStr, out DateTime dateValue))
                                {
                                    long unixTimestamp = ((DateTimeOffset)dateValue).ToUnixTimeSeconds();
                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                    dateValid = true;
                                }
                                else if (long.TryParse(dateStr, out long unixTs) && unixTs > 946684800)
                                {
                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTs).UtcDateTime;
                                    dateValid = true;
                                }
                                else
                                {
                                    dateUtc = DateTime.MinValue;
                                    _logger.LogWarning("Format de date invalide pour {Serial}/{Obis}: {Date} — ligne ignoree",
                                        serialNumber, profileObis, dateStr);
                                }
                            }
                            else
                            {
                                dateUtc = DateTime.MinValue;
                            }

                            // Si la date est invalide, ignorer TOUTE la ligne
                            if (!dateValid) continue;

                            for (int i = 0; i < entry.Value.Length; i++)
                            {
                                var objStr = entry.Value[i]?.ToString() ?? string.Empty;

                                if (isRegisterProfile)
                                {
                                    // Register profile processing
                                    var detailprofil = new Gxdlmsprofilgenericdetail();
                                    object realValue;
                                    try
                                    {
                                        realValue = ((JValue)array[i]).Value;
                                    }
                                    catch
                                    {
                                        realValue = array[i];
                                    }
                                    bool isRegisterValue = realValue is decimal || realValue is double || realValue is float || realValue is int || realValue is long || realValue is Int64 || realValue is Int32;

                                    var rawStr = isRegisterValue ? Convert.ToDecimal(realValue).ToString() : realValue?.ToString();
                                    detailprofil.RawValue = rawStr;
                                    detailprofil.Value = rawStr;
                                    detailprofil.CodeObisId = codeObisDict.TryGetValue(objStr, out var codeObis) ? codeObis.Id : 0;
                                    detailprofil.DateEnr = dateUtc;
                                    detailprofil.GxdlmsprofilgenericId = profilGeneric.Id;
                                    detailprofil.NumeroCompteur = serialNumber;
                                    detailprofil.IsArchive = false;

                                    detailsToAdd.Add(detailprofil);
                                }
                                else
                                {
                                    // Event profile processing
                                    var detailprofilevent = new Gxdlmsprofilgenericdetailsevent();

                                    if (objStr.StartsWith("0.0.96.11."))
                                    {
                                        int valconvert = Convert.ToInt32(array[i]);

                                        _logger.LogDebug("Recherche événement: valconvert={Valconvert}, eventsDict.Count={Count}",
                                            valconvert, eventsDict.Count);

                                        if (eventsDict.TryGetValue(valconvert, out var eventResult))
                                        {
                                            detailprofilevent.EventId = eventResult?.Id;
                                            _logger.LogDebug("Événement trouvé: Id={EventId}, Description={Description}",
                                                eventResult?.Id, eventResult?.Code2);
                                        }
                                        else
                                        {
                                            _logger.LogWarning("Événement non trouvé pour la valeur: {Valconvert}", valconvert);
                                            detailprofilevent.EventId = null;
                                        }
                                        detailprofilevent.Value = array[i]?.ToString() ?? string.Empty;
                                        detailprofilevent.RawValue = detailprofilevent.Value;
                                    }
                                    else
                                    {
                                        detailprofilevent.Value = array[i]?.ToString() ?? string.Empty;
                                        detailprofilevent.RawValue = detailprofilevent.Value;
                                        detailprofilevent.EventId = null;
                                    }

                                    // Date déjà extraite en amont pour toute la ligne
                                    detailprofilevent.DateEnr = dateUtc;
                                    detailprofilevent.CodeObisId = codeObisDict.TryGetValue(objStr, out var codeObisEvent) ? codeObisEvent.Id : 0;
                                    detailprofilevent.GxdlmsprofilgenericId = profilGeneric.Id;
                                    detailprofilevent.NumeroCompteur = serialNumber;
                                    detailprofilevent.IsArchive = false;

                                    eventsToAdd.Add(detailprofilevent);
                                }
                            }
                        }
                    }
                }

                // BulkInsertOrUpdate with no-op update = "insert if not exists"
                if (detailsToAdd.Count > 0)
                {
                    await context.BulkInsertOrUpdateAsync(detailsToAdd, new BulkConfig
                    {
                        SetOutputIdentity = false,
                        BatchSize = 500,
                        UpdateByProperties = new List<string>
                        {
                            nameof(Gxdlmsprofilgenericdetail.NumeroCompteur),
                            nameof(Gxdlmsprofilgenericdetail.GxdlmsprofilgenericId),
                            nameof(Gxdlmsprofilgenericdetail.CodeObisId),
                            nameof(Gxdlmsprofilgenericdetail.DateEnr)
                        },
                        PropertiesToExcludeOnUpdate = new List<string>
                        {
                            nameof(Gxdlmsprofilgenericdetail.Value),
                            nameof(Gxdlmsprofilgenericdetail.RawValue),
                            nameof(Gxdlmsprofilgenericdetail.IsArchive)
                        }
                    });
                }
                if (eventsToAdd.Count > 0)
                {
                    await context.BulkInsertOrUpdateAsync(eventsToAdd, new BulkConfig
                    {
                        SetOutputIdentity = false,
                        BatchSize = 500,
                        UpdateByProperties = new List<string>
                        {
                            nameof(Gxdlmsprofilgenericdetailsevent.NumeroCompteur),
                            nameof(Gxdlmsprofilgenericdetailsevent.GxdlmsprofilgenericId),
                            nameof(Gxdlmsprofilgenericdetailsevent.CodeObisId),
                            nameof(Gxdlmsprofilgenericdetailsevent.DateEnr)
                        },
                        PropertiesToExcludeOnUpdate = new List<string>
                        {
                            nameof(Gxdlmsprofilgenericdetailsevent.Value),
                            nameof(Gxdlmsprofilgenericdetailsevent.RawValue),
                            nameof(Gxdlmsprofilgenericdetailsevent.IsArchive),
                            nameof(Gxdlmsprofilgenericdetailsevent.EventId)
                        }
                    });
                }

                int totalInserted = detailsToAdd.Count + eventsToAdd.Count;
                _logger.LogInformation("ProcessAndSaveSingleProfileAsync: {Count} entrées traitées pour {SerialNumber} / {ProfileObis}",
                    totalInserted, serialNumber, profileObis);

                return totalInserted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement profil unique {ProfileObis} pour {SerialNumber}", profileObis, serialNumber);
                return 0;
            }
        }

        private static string TryConvertToDecimalString(object? val)
        {
            if (val is null) return string.Empty;

            return val switch
            {
                decimal d => d.ToString(),
                int i => i.ToString(),
                long l => l.ToString(),
                string s => s,
                _ => val.ToString() ?? string.Empty
            };
        }

        private Events? GetEventIdFromCacheOrNull(string obisValue, DLMSDBContext context)
        {
            // Méthode simplifiée d’accès au cache / base
            // A améliorer si tu veux mettre un cache plus robuste
            var eventValueMapping = new HashSet<string>
    {
        "0.0.96.11.0.255","0.0.96.11.1.255","0.0.96.11.2.255","0.0.96.11.3.255",
        "0.0.96.11.4.255","0.0.96.11.5.255","0.0.96.11.6.255","0.0.96.11.7.255"
    };

            if (!eventValueMapping.Contains(obisValue)) return null;

            return context.Events.FirstOrDefault(e => e.Value.ToString() == obisValue && e.Category == "EVENTS_GROUP_ALL_REGISTERS");
        }

        // Méthodes d'optimisation
        private static readonly Dictionary<string, CodeObis> _codeObisCache = new();
        private static readonly Dictionary<long, Events> _eventCache = new();

        private async Task<CodeObis> GetCodeObisAsync(DLMSDBContext context, string value)
        {
            if (_codeObisCache.TryGetValue(value, out var cached))
                return cached;

            var codeobis = await context.CodeObis.FirstOrDefaultAsync(x => x.Value == value);
            if (codeobis != null)
                _codeObisCache[value] = codeobis;

            return codeobis;
        }

        private async Task<Events> GetEventAsync(DLMSDBContext context, long value)
        {
            if (_eventCache.TryGetValue(value, out var cached))
                return cached;

            var eventResult = await context.Events.FirstOrDefaultAsync(x => x.Value == value && x.Category == "EVENTS_GROUP_ALL_REGISTERS");
            if (eventResult != null)
                _eventCache[value] = eventResult;

            return eventResult;
        }
        public async Task<int> GetNextTentativeNumberAsync(int commandeCompteurId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

                var commandeCompteur = await context.CommandeCompteur
                    .Where(cc => cc.Id == commandeCompteurId)
                    .FirstOrDefaultAsync();
                if (commandeCompteur != null)
                {
                    commandeCompteur.NumeroTentative = commandeCompteur.NumeroTentative + 1;
                    context.CommandeCompteur.Update(commandeCompteur);
                    await context.SaveChangesAsync();
                    return commandeCompteur.NumeroTentative;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }

        public async Task ProcessAndSaveCommandsDataAsync(string data, string serialNumber, int commandeCompteurId)
        {
            try
            {
                _logger.LogDebug("Traitement et enregistrement des données de commande pour {SerialNumber}", serialNumber);

                var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(data);

                if (entries == null || entries.Count == 0)
                {
                    _logger.LogWarning("Aucune donnée de commande à traiter pour {SerialNumber}", serialNumber);
                    return;
                }

                var profiles = new[]
                {
                    "1.0.99.1.0.255", "1.0.99.2.0.255", "1.0.99.3.0.255"
                };

                // Créer un scope séparé pour chaque profil pour éviter la concurrence DbContext
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();
                
                // Précharger tous les profils génériques avec leur CodeObis
                var profilGenericsDict = (await context.Gxdlmsprofilgenerics
                    .Join(context.CodeObis, 
                        pg => pg.CodeObisId, 
                        co => co.Id, 
                        (pg, co) => new { ProfilGeneric = pg, CodeObis = co })
                    .Where(x => profiles.Contains(x.CodeObis.Value))
                    .ToListAsync())
                    .GroupBy(x => x.CodeObis.Value)
                    .ToDictionary(g => g.Key, g => g.First().ProfilGeneric);

                // Précharger tous les CodeObis uniques
                var allObisValues = entries
                    .SelectMany(e => e.Value.Select(v => v?.ToString()))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                var codeObisDict = (await context.CodeObis
                    .Where(c => allObisValues.Contains(c.Value))
                    .ToListAsync())
                    .GroupBy(c => c.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // Désactiver le tracking EF
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var commandesToAdd = new List<ResultatCommandeCompteur>();

                int processedCount = 0;

                foreach (var entry in entries.Take(profiles.Length))
                {
                    try
                    {
                        var profileLN = profiles[processedCount];
                        
                        // Créer un scope séparé pour chaque profil
                        using var profileScope = _serviceScopeFactory.CreateScope();
                        var profileContext = profileScope.ServiceProvider.GetRequiredService<DLMSDBContext>();
                        
                        // Utiliser le dictionnaire au lieu de la DB
                        if (!profilGenericsDict.TryGetValue(profileLN, out var profilGeneric))
                        {
                            _logger.LogWarning("Profil non trouvé pour {ProfileLN}", profileLN);
                            processedCount++;
                            continue;
                        }

                        if (entry.Key.Length > 0 && entry.Value.Length > 0)
                        {
                            DateTime dateUtc = DateTime.UtcNow;

                            foreach (var row in entry.Key)
                            {
                                if (row is IEnumerable<object> values)
                                {
                                    var array = values.ToArray();

                                    for (int i = 0; i < entry.Value.Length; i++)
                                    {
                                        var objStr = entry.Value[i]?.ToString() ?? string.Empty;

                                        // Création objet ResultatCommande
                                        var commande = new ResultatCommandeCompteur();
                                        var realValue = ((JValue)array[i]).Value;
                                        bool isRegisterValue = realValue is decimal || realValue is double || realValue is float || realValue is int || realValue is long || realValue is Int64 || realValue is Int32;
                                        
                                        if (i == 0)
                                        {
                                            if (DateTime.TryParse(array[i].ToString(), out DateTime dateValue))
                                            {
                                                long unixTimestamp = ((DateTimeOffset)dateValue).ToUnixTimeSeconds();
                                                dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                            }
                                            else
                                            {
                                                // Gérer le cas où la conversion échoue
                                                _logger.LogWarning("Format de date invalide: {Date}", array[i]);
                                                continue;
                                            }
                                        }

                                        commande.Value = isRegisterValue ? Convert.ToDecimal(realValue).ToString() : realValue?.ToString();
                                        commande.CodeObisId = codeObisDict.TryGetValue(objStr, out var codeObis) ? codeObis.Id : 0;
                                        commande.DateEnr = dateUtc;
                                        commande.NumeroCompteur = serialNumber;
                                        commande.CommandeCompteurId = commandeCompteurId;
                                        commande.GxdlmsprofilgenericId = profilGeneric.Id;
                                        commande.IsArchive = false;

                                        commandesToAdd.Add(commande);
                                    }
                                }
                            }
                        }

                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erreur lors du traitement de l'entrée {ProfileLN} pour {SerialNumber}", profiles[processedCount], serialNumber);
                        processedCount++;
                    }
                }

                // Sauvegarde des commandes avec AddRangeAsync en évitant les doublons
                if (commandesToAdd.Any())
                {
                    // 🔥 OPTIMISATION: Approche par plage de dates (ChatGPT) pour les commandes
                    var minCommandeDate = commandesToAdd.Min(x => x.DateEnr);
                    var maxCommandeDate = commandesToAdd.Max(x => x.DateEnr);

                    var existingCommandes = await context.ResultatCommandeCompteurs
                        .AsNoTracking()
                        .Where(x =>
                            x.NumeroCompteur == serialNumber &&
                            x.DateEnr >= minCommandeDate &&
                            x.DateEnr <= maxCommandeDate)
                        .Select(x => new
                        {
                            x.Value,
                            x.DateEnr,
                            x.GxdlmsprofilgenericId,
                            x.NumeroCompteur,
                            x.CodeObisId,
                            x.CommandeCompteurId
                        })
                        .ToListAsync();

                    // HashSet en mémoire pour filtrage rapide
                    var existingCommandesKeysSet = new HashSet<string>(
                        existingCommandes.Select(e =>
                            $"{e.Value}_{e.DateEnr:yyyy-MM-dd HH:mm:ss.fffffff}_{e.GxdlmsprofilgenericId}_{e.NumeroCompteur}_{e.CodeObisId}_{e.CommandeCompteurId}"
                        ));

                    var commandesToInsert = commandesToAdd
                        .Where(c => !existingCommandesKeysSet.Contains(
                            $"{c.Value}_{c.DateEnr:yyyy-MM-dd HH:mm:ss.fffffff}_{c.GxdlmsprofilgenericId}_{c.NumeroCompteur}_{c.CodeObisId}_{c.CommandeCompteurId}"
                        ))
                        .ToList();

                    if (commandesToInsert.Any())
                    {
                        await context.ResultatCommandeCompteurs.AddRangeAsync(commandesToInsert);
                        await context.SaveChangesAsync();
                        
                        _logger.LogInformation("✅ {Count} enregistrements de commande sauvegardés pour {SerialNumber}", commandesToInsert.Count, serialNumber);
                        
                        // Archiver le CommandeCompteur après traitement réussi
                        //await ArchiveCommandeCompteurAsync(commandeCompteurId);
                    }
                    else
                    {
                        _logger.LogInformation("ℹ️ Tous les enregistrements de commande existent déjà pour {SerialNumber}", serialNumber);
                    }
                }
                else
                {
                    _logger.LogWarning("Aucune commande à sauvegarder pour {SerialNumber}", serialNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement et de l'enregistrement des données de commande pour {SerialNumber}", serialNumber);
                throw;
            }
        }


        public async Task<bool> CheckAndArchiveCommandIfAllCompteursArchivedAsync(int commandeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();
                var commandeRepo = scope.ServiceProvider.GetRequiredService<DLMS_DAL.CommandeDomainDal.Repositories.Commands.ICommandeCommandRepository>();

                // Récupérer la commande avec ses CommandeCompteur
                var commande = await context.Commandes
                    .Include(c => c.CommandeCompteur)
                    .FirstOrDefaultAsync(c => c.Id == commandeId);

                if (commande == null)
                {
                    _logger.LogWarning("Commande {CommandeId} non trouvée", commandeId);
                    return false;
                }

                if (!commande.CommandeCompteur.Any())
                {
                    _logger.LogWarning("Aucun CommandeCompteur trouvé pour la commande {CommandeId}", commandeId);
                    return false;
                }

                // Vérifier si tous les CommandeCompteur sont archivés (IsArchive = true)
                var allArchived = commande.CommandeCompteur.All(cc => cc.IsArchive);
                
                // Vérifier si la commande a expiré
                var isExpired = commande.Dateexp.HasValue && commande.Dateexp.Value <= DateTime.Now;

                if (allArchived || isExpired)
                {
                    // Archiver la commande
                    var commandeToArchive = new Commande
                    {
                        Id = commandeId,
                        DeletedBy = "System"
                    };
                    await commandeRepo.DeleteCommande(commandeToArchive);

                    var raison = allArchived ? "tous ses CommandeCompteur sont archivés" : "la commande a expiré";
                    _logger.LogInformation("Commande {CommandeId} archivée car {Raison}", commandeId, raison);
                    return true;
                }
                else
                {
                    _logger.LogDebug("La commande {CommandeId} n'est pas archivée - CommandeCompteurs archivés: {AllArchived}, Expirée: {IsExpired}", 
                        commandeId, allArchived, isExpired);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification et de l'archivage de la commande {CommandeId}", commandeId);
                return false;
            }
        }


        public async Task ArchiveCommandeCompteurAsync(int commandeCompteurId)
        {
            try
            {
                using var archiveScope = _serviceScopeFactory.CreateScope();
                var commandeCompteurRepo = archiveScope.ServiceProvider.GetRequiredService<DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands.ICommandeCompteurCommandRepository>();

                var commandeCompteurToArchive = new CommandeCompteur
                {
                    Id = commandeCompteurId,
                    DeletedBy = "System"
                };
                await commandeCompteurRepo.DeleteCommandeCompteur(commandeCompteurToArchive);

                _logger.LogInformation("📁 CommandeCompteur {CommandeCompteurId} archivée après traitement", commandeCompteurId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'archivage du CommandeCompteur {CommandeCompteurId}", commandeCompteurId);
                throw;
            }
        }
     }
}
