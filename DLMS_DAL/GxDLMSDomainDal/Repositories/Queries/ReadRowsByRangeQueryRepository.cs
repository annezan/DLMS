using DLMS_DAL.Bases;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using DLMS_COMMUNICATION;
using DLMS_COMMUNICATION.Reader;
using Gurux.DLMS.Objects;
using Newtonsoft.Json;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using Microsoft.Extensions.DependencyInjection;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using Newtonsoft.Json.Linq;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class ReadRowsByRangeQueryRepository : QueryBaseRepository<AssociationKey>, IReadRowsByRangeQueryRepository
    {
        public ReadRowsByRangeQueryRepository(DLMSDBContext _context)
        :
            base(_context)
        {
        }


        public string GetReadRowsByRange(string datestart, string dateend, string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            try
            {
                //DateTime datestart2 = DateTime.Parse(datestart);
                //var esssai = datestart2;
                var authentication = _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "authentication" && x.CompteurId.Contains(SerialNumber)).FirstOrDefault();
                var unicast = _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "unicast" && x.CompteurId.Contains(SerialNumber)).FirstOrDefault();

                if (authentication == null || unicast == null)
                {
                    throw new Exception("Authentication ou Unicast key non trouvée");
                }

                var AuthenticationKey = Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS");
                var UnicastKey = Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS");
                var result=ReaderCommunication.ReadRowsByRange(datestart,dateend, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, authentication.Pwd, AuthenticationKey, UnicastKey, Objects);

                // Désérialisation en liste d'objets
                var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(result);


                if (entries == null || entries.Count == 0)
                {
                    Console.WriteLine("Aucune donnée de profil à traiter pour {SerialNumber}", SerialNumber);
                    return "";
                }

                var profiles = new[]
                {
            "1.0.99.1.0.255", "1.0.99.2.0.255", "1.0.99.3.0.255",
            "0.0.98.1.0.255", "0.0.99.98.0.255", "0.0.99.98.1.255",
            "0.0.99.98.2.255", "0.0.99.98.3.255", "0.0.99.98.4.255",
            "0.0.99.98.5.255", "0.0.99.98.6.255", "0.0.99.98.7.255"
        };


                // 🔥 OPTIMISATION 1: Précharger tous les profils génériques avec leur CodeObis
                var profilGenericsDict = (_context.Gxdlmsprofilgenerics
                    .Join(_context.CodeObis,
                        pg => pg.CodeObisId,
                        co => co.Id,
                        (pg, co) => new { ProfilGeneric = pg, CodeObis = co })
                    .Where(x => profiles.Contains(x.CodeObis.Value))
                    .ToList())
                    .GroupBy(x => x.CodeObis.Value)
                    .ToDictionary(g => g.Key, g => g.First().ProfilGeneric);

                // 🔥 OPTIMISATION 2: Précharger tous les CodeObis uniques
                var allObisValues = entries
                    .SelectMany(e => e.Value.Select(v => v?.ToString()))
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                var codeObisDict = (_context.CodeObis
                    .Where(c => allObisValues.Contains(c.Value))
                    .ToList())
                    .GroupBy(c => c.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // 🔥 OPTIMISATION 3: Précharger tous les Events possibles
                var eventValues = entries
                    .Where(e => e.Value.Any(v => v?.ToString().StartsWith("0.0.96.11.") == true))
                    .SelectMany(e => e.Value)
                    .Select(v =>
                    {
                        var str = v?.ToString();
                        return str.StartsWith("0.0.96.11.") && long.TryParse(str?.Split('.').Last(), out var val) ? val : (long?)null;
                    })
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                    .Distinct()
                    .ToList();

                var eventsDict = (_context.Events
                    .Where(e => eventValues.Contains(e.Value) && e.Category == "EVENTS_GROUP_ALL_REGISTERS")
                    .ToList())
                    .GroupBy(e => e.Value)
                    .ToDictionary(g => g.Key, g => g.First());

                // 🔥 OPTIMISATION 4: Désactiver le tracking EF
                _context.ChangeTracker.AutoDetectChangesEnabled = false;
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var detailsToAdd = new List<Gxdlmsprofilgenericdetail>();
                var eventsToAdd = new List<Gxdlmsprofilgenericdetailsevent>();

                int processedCount = 0;

                foreach (var entry in entries.Take(profiles.Length))
                {
                    try
                    {
                        var profileLN = profiles[processedCount];

                        // 🔥 OPTIMISATION 5: Utiliser le dictionnaire au lieu de la DB
                        if (!profilGenericsDict.TryGetValue(profileLN, out var profilGeneric))
                        {
                            Console.WriteLine("Profil non trouvé pour {ProfileLN}", profileLN);
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

                                        if (processedCount < 4)
                                        {
                                            // Création objet detailprofil
                                            var detailprofil = new Gxdlmsprofilgenericdetail();
                                            var realValue = ((JValue)array[i]).Value;
                                            bool isRegisterValue = realValue is decimal || realValue is int || realValue is long || realValue is Int64 || realValue is Int32;
                                            if (i == 0)
                                            {
                                                long unixTimestamp = Convert.ToUInt32(array[i]);
                                                dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;

                                            }

                                            detailprofil.Value = isRegisterValue ? Convert.ToDecimal(realValue).ToString() : realValue?.ToString();
                                            detailprofil.CodeObisId = codeObisDict.TryGetValue(objStr, out var codeObis) ? codeObis.Id : 0;
                                            detailprofil.DateEnr = dateUtc;
                                            detailprofil.GxdlmsprofilgenericId = profilGeneric.Id;
                                            detailprofil.NumeroCompteur = SerialNumber;
                                            detailprofil.IsArchive = false;

                                            detailsToAdd.Add(detailprofil);


                                        }
                                        else
                                        {
                                            // Traitement des événements
                                            var detailprofilevent = new Gxdlmsprofilgenericdetailsevent();

                                            if (objStr.StartsWith("0.0.96.11."))
                                            {
                                                long valconvert = Convert.ToInt64(array[i]);
                                                eventsDict.TryGetValue((int)valconvert, out var eventResult);
                                                detailprofilevent.Value = array[i]?.ToString() ?? string.Empty;
                                                detailprofilevent.EventId = eventResult?.Id;
                                            }
                                            else
                                            {
                                                detailprofilevent.Value = array[i]?.ToString() ?? string.Empty;
                                                detailprofilevent.EventId = null;
                                            }

                                            if (i == 0)
                                            {
                                                long unixTimestamp = Convert.ToUInt32(array[i]);
                                                dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                            }

                                            detailprofilevent.DateEnr = dateUtc;
                                            detailprofilevent.CodeObisId = codeObisDict.TryGetValue(objStr, out var codeObis) ? codeObis.Id : 0;
                                            detailprofilevent.GxdlmsprofilgenericId = profilGeneric.Id;
                                            detailprofilevent.NumeroCompteur = SerialNumber;
                                            detailprofilevent.IsArchive = false;

                                            eventsToAdd.Add(detailprofilevent);
                                        }

                                        // 🔥 OPTIMISATION 6: BulkInsert - plus de sauvegardes dans les boucles
                                        // Les données seront sauvegardées en une seule fois à la fin avec BulkInsert
                                    }
                                }
                            }
                        }

                        // 🔥 OPTIMISATION 6: Sauvegarde par profil avec AddRangeAsync
                        if (detailsToAdd.Any())
                        {
                            // 🔥 OPTIMISATION: Vérification en une seule requête au lieu de N requêtes
                            var existingDetailsKeys = detailsToAdd.Select(d => new
                            {
                                d.Value,
                                d.DateEnr,
                                d.GxdlmsprofilgenericId,
                                d.NumeroCompteur,
                                d.CodeObisId
                            }).ToList();

                            // 🔥 OPTIMISATION: Utilisation de Contains avec des listes séparées
                            var values = existingDetailsKeys.Select(k => k.Value).Distinct().ToList();
                            var dates = existingDetailsKeys.Select(k => k.DateEnr).Distinct().ToList();
                            var ids = existingDetailsKeys.Select(k => k.GxdlmsprofilgenericId).Distinct().ToList();
                            var numbers = existingDetailsKeys.Select(k => k.NumeroCompteur).Distinct().ToList();
                            var codes = existingDetailsKeys.Select(k => k.CodeObisId).Distinct().ToList();

                            var existingDetails = _context.Gxdlmsprofilgenericdetails.AsNoTracking()
                                .Where(x => values.Contains(x.Value) &&
                                           dates.Contains(x.DateEnr) &&
                                           ids.Contains(x.GxdlmsprofilgenericId) &&
                                           numbers.Contains(x.NumeroCompteur) &&
                                           codes.Contains(x.CodeObisId))
                                .Select(x => new
                                {
                                    x.Value,
                                    x.DateEnr,
                                    x.GxdlmsprofilgenericId,
                                    x.NumeroCompteur,
                                    x.CodeObisId
                                })
                                .ToList();

                            var existingKeysSet = new HashSet<string>(
                                existingDetails.Select(e => $"{e.Value}_{e.DateEnr:yyyy-MM-dd HH:mm:ss.fffffff}_{e.GxdlmsprofilgenericId}_{e.NumeroCompteur}_{e.CodeObisId}"));

                            var detailsToInsert = detailsToAdd
                                .Where(d => !existingKeysSet.Contains($"{d.Value}_{d.DateEnr:yyyy-MM-dd HH:mm:ss.fffffff}_{d.GxdlmsprofilgenericId}_{d.NumeroCompteur}_{d.CodeObisId}"))
                                .ToList();

                            if (detailsToInsert.Any())
                            {
                                _context.Gxdlmsprofilgenericdetails.AddRange(detailsToInsert);
                                _context.SaveChanges();
                            }
                            _context.ChangeTracker.Clear();
                            detailsToAdd.Clear();
                        }
                        if (eventsToAdd.Any())
                        {
                            // 🔥 OPTIMISATION: Vérification en une seule requête au lieu de N requêtes pour les événements
                            var existingEventsKeys = eventsToAdd.Select(e => new
                            {
                                e.Value,
                                e.DateEnr,
                                e.GxdlmsprofilgenericId,
                                e.NumeroCompteur,
                                e.CodeObisId,
                                e.EventId
                            }).ToList();

                            // 🔥 OPTIMISATION: Utilisation de Contains avec des listes séparées
                            var eventValueList = existingEventsKeys.Select(k => k.Value).Distinct().ToList();
                            var eventDateList = existingEventsKeys.Select(k => k.DateEnr).Distinct().ToList();
                            var eventIdList = existingEventsKeys.Select(k => k.GxdlmsprofilgenericId).Distinct().ToList();
                            var eventNumberList = existingEventsKeys.Select(k => k.NumeroCompteur).Distinct().ToList();
                            var eventCodeList = existingEventsKeys.Select(k => k.CodeObisId).Distinct().ToList();
                            var eventEventIdList = existingEventsKeys.Select(k => k.EventId).Distinct().ToList();

                            var existingEvents = _context.Gxdlmsprofilgenericdetailsevents.AsNoTracking()
                                .Where(x => eventValueList.Contains(x.Value) &&
                                           eventDateList.Contains(x.DateEnr) &&
                                           eventIdList.Contains(x.GxdlmsprofilgenericId) &&
                                           eventNumberList.Contains(x.NumeroCompteur) &&
                                           eventCodeList.Contains(x.CodeObisId) &&
                                           (x.EventId == null || eventEventIdList.Contains(x.EventId)))
                                .Select(x => new
                                {
                                    x.Value,
                                    x.DateEnr,
                                    x.GxdlmsprofilgenericId,
                                    x.NumeroCompteur,
                                    x.CodeObisId,
                                    x.EventId
                                })
                                .ToList();

                            var existingEventsKeysSet = new HashSet<string>(
                                existingEvents.Select(e => $"{e.Value}_{e.DateEnr:yyyy-MM-dd HH:mm:ss.fffffff}_{e.GxdlmsprofilgenericId}_{e.NumeroCompteur}_{e.CodeObisId}_{e.EventId}"));

                            var eventsToInsert = eventsToAdd
                                .Where(e => !existingEventsKeysSet.Contains($"{e.Value}_{e.DateEnr:yyyy-MM-dd HH:mm:ss.fffffff}_{e.GxdlmsprofilgenericId}_{e.NumeroCompteur}_{e.CodeObisId}_{e.EventId}"))
                                .ToList();

                            if (eventsToInsert.Any())
                            {
                                _context.Gxdlmsprofilgenericdetailsevents.AddRangeAsync(eventsToInsert);
                                _context.SaveChangesAsync();
                            }
                            _context.ChangeTracker.Clear();
                            eventsToAdd.Clear();
                        }

                        processedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString(), "Erreur traitement entrée profil {Index} pour {SerialNumber}", processedCount, SerialNumber);
                    }
                }

                // 🔥 OPTIMISATION 7: Plus besoin de sauvegarder ici - déjà fait par profil

                // 🔥 OPTIMISATION 7: Réactiver le tracking EF
                _context.ChangeTracker.AutoDetectChangesEnabled = true;
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

                Console.WriteLine("Profil traité et enregistré: {Processed} entrées pour {SerialNumber}", processedCount, SerialNumber);

                return result.ToString();
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

    }
}
