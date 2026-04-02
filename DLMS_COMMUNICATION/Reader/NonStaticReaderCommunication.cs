using System;
using System.Linq;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Secure;
using Gurux.Net;
using Gurux.Serial;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using DLMS_COMMUNICATION.Reader;
using GuruxUnit = Gurux.DLMS.Enums.Unit;

namespace DLMS_COMMUNICATION.Reader
{
    public class NonStaticReaderCommunication
    {
        private readonly ILogger<NonStaticReaderCommunication> _logger;

        public NonStaticReaderCommunication(ILogger<NonStaticReaderCommunication> logger = null)
        {
            _logger = logger;
        }

        public async Task<string> ReadAsync(IDLMSCommunicationSession session)
        {
            try
            {
                if (session.ReadObjects.Count == 0)
                    return "Lecture impossible";

                if (!session.AssociationLoaded)
                {
                    bool loadedFromFile = false;
                    if (!string.IsNullOrEmpty(session.OutputFile))
                    {
                        try
                        {
                            session.Client.Objects.Clear();
                            session.Client.Objects.AddRange(GXDLMSObjectCollection.Load(session.OutputFile));
                            loadedFromFile = true;
                        }
                        catch { }
                    }
                    if (!loadedFromFile)
                    {
                        session.Reader.GetAssociationView(session.OutputFile);
                    }
                    session.AssociationLoaded = true;
                }

                var dict = new Dictionary<string, object>();

                foreach (var it in session.ReadObjects)
                {
                    var obj = session.Client.Objects
                        .FindByLN(ObjectType.None, it.Key);

                    if (obj == null)
                        continue;

                    var val = session.Reader.Read(obj, it.Value);

                    dict[it.Key] = val is byte[] b
                        ? Encoding.UTF8.GetString(b).Trim()
                        : val?.ToString().Trim();
                }

                return JsonConvert.SerializeObject(dict);
            }
            catch(Exception ex)
            {
                _logger?.LogWarning(ex, "ReadAsync echec: {Message}", ex.Message);
                return "Lecture impossible";
            }
        }

        public async Task<string> ReadListAsync(IDLMSCommunicationSession session)
        {
            try
            {
                if (session.ReadObjects.Count == 0)
                    return "Lecture impossible";

                if (!session.AssociationLoaded)
                {
                    bool loadedFromFile = false;
                    if (!string.IsNullOrEmpty(session.OutputFile))
                    {
                        try
                        {
                            session.Client.Objects.Clear();
                            session.Client.Objects.AddRange(GXDLMSObjectCollection.Load(session.OutputFile));
                            loadedFromFile = true;
                        }
                        catch { }
                    }
                    if (!loadedFromFile)
                    {
                        session.Reader.GetAssociationView(session.OutputFile);
                    }
                    session.AssociationLoaded = true;
                }

                // Build list of (GXDLMSObject, attributeIndex) pairs
                var objectsToRead = new List<KeyValuePair<GXDLMSObject, int>>();
                foreach (var it in session.ReadObjects)
                {
                    var obj = session.Client.Objects.FindByLN(ObjectType.None, it.Key);
                    if (obj != null)
                        objectsToRead.Add(new KeyValuePair<GXDLMSObject, int>(obj, it.Value));
                }

                if (objectsToRead.Count == 0)
                    return "Lecture impossible";

                // Use ReadList if meter supports MultipleReferences, else fallback
                if (objectsToRead.Count > 1 &&
                    (session.Client.NegotiatedConformance & Conformance.MultipleReferences) != 0)
                {
                    session.Reader.ReadList(objectsToRead);
                }
                else
                {
                    foreach (var kv in objectsToRead)
                        session.Reader.Read(kv.Key, kv.Value);
                }

                // Extract values
                var dict = new Dictionary<string, object>();
                foreach (var kv in objectsToRead)
                {
                    var val = kv.Key.GetValues()[kv.Value - 1]; // attribute index is 1-based
                    dict[kv.Key.LogicalName] = val is byte[] b
                        ? Encoding.UTF8.GetString(b).Trim()
                        : val?.ToString()?.Trim();
                }

                return JsonConvert.SerializeObject(dict);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "ReadListAsync echec: {Message}", ex.Message);
                return "Lecture impossible";
            }
        }

        public async Task<string> ReadRowsByRangeAsync(IDLMSCommunicationSession session, string datestart, string dateend)
        {
            try
            {
                if (session.ReadObjects.Count == 0)
                    return "Lecture impossible";

                if (!DateTime.TryParse(datestart, out DateTime datestart2))
                    return "Lecture impossible";

                if (!DateTime.TryParse(dateend, out DateTime dateend2))
                    return "Lecture impossible";

                if (!session.AssociationLoaded)
                {
                    bool loadedFromFile = false;
                    if (!string.IsNullOrEmpty(session.OutputFile))
                    {
                        try
                        {
                            session.Client.Objects.Clear();
                            session.Client.Objects.AddRange(GXDLMSObjectCollection.Load(session.OutputFile));
                            loadedFromFile = true;
                        }
                        catch { }
                    }
                    if (!loadedFromFile)
                    {
                        session.Reader.GetAssociationView(session.OutputFile);
                    }
                    session.AssociationLoaded = true;
                }

                var entries = new List<KeyValuePair<object[], object[]>>();
                var scalers = new Dictionary<string, ScalerMetadata>();

                foreach (var it in session.ReadObjects)
                {
                    var profile = session.Client.Objects
                        .FindByLN(ObjectType.ProfileGeneric, it.Key)
                        as GXDLMSProfileGeneric;

                    if (profile == null)
                        continue;

                    // Lire capture objects (attribut 3)
                    session.Reader.Read(profile, 3);

                    var captureObjects = profile.GetCaptureObject();

                    // Lire les scalers EXPLICITEMENT par capture object,
                    // puis reset Scaler=1 pour empêcher Gurux d'auto-appliquer
                    ReadAndResetScalersForCaptureObjects(session, captureObjects, scalers);

                    // Lecture lignes par date — valeurs GARANTIES brutes (Scaler=1 sur tous les objets)
                    object[] rows = session.Reader
                            .ReadRowsByRange(profile, datestart2, dateend2);

                    // Colonnes
                    object[] cols = new object[captureObjects.Length];

                    for (int i = 0; i < captureObjects.Length; i++)
                    {
                        var col = captureObjects[i];
                        cols[i] = col.Name;
                    }

                    var safeRows = SafeConvertRows(rows);

                    entries.Add(new KeyValuePair<object[], object[]>(safeRows, cols));
                }

                // Lire TC/TT
                var tctt = ReadTcTtValues(session);
                var envelope = new ProfileDataEnvelope
                {
                    Entries = entries,
                    Scalers = scalers,
                    TcTtValues = tctt
                };

                return JsonConvert.SerializeObject(envelope, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "ReadRowsByRangeAsync echec: {Message}", ex.Message);
                return "Lecture impossible";
            }
        }

        public async Task<string> ReadRowsByEntryAsync(IDLMSCommunicationSession session, long count)
        {
            string jsonText = null;

            try
            {
                if (session == null)
                {
                    return "Session invalide";
                }

                if (session.ReadObjects.Count == 0)
                    return "Aucun objet à lire";

                bool loadedFromFile = false;

                if (!string.IsNullOrEmpty(session.OutputFile))
                {
                    try
                    {
                        session.Client.Objects.Clear();
                        session.Client.Objects.AddRange(GXDLMSObjectCollection.Load(session.OutputFile));
                        loadedFromFile = true;
                    }
                    catch
                    {
                        _logger?.LogDebug("Impossible de charger le fichier de cache");
                    }
                }
                
                if (!loadedFromFile)
                {
                    session.Reader.GetAssociationView(session.OutputFile);
                }

                var scalers = new Dictionary<string, ScalerMetadata>();

                foreach (KeyValuePair<string, int> it in session.ReadObjects)
                {
                    var item = session.Client.Objects.FindByLN(ObjectType.ProfileGeneric, it.Key);

                    if (item.LogicalName == it.Key)
                    {
                        session.Reader.Read(item, 3);

                        long entriesInUse = -1;
                        if ((item.GetAccess(7) & AccessMode.Read) != 0)
                        {
                            entriesInUse = Convert.ToInt64(session.Reader.Read(item, 7));
                        }

                        long entries = -1;
                        if ((item.GetAccess(8) & AccessMode.Read) != 0)
                        {
                            entries = Convert.ToUInt32(session.Reader.Read(item, 8));
                        }

                        _logger?.LogDebug("Entrées: {EntriesInUse}/{Entries}", entriesInUse, entries);

                        //If there are no columns or rows.
                        if (entriesInUse == 0 || (item as GXDLMSProfileGeneric).CaptureObjects.Count == 0)
                        {
                            continue;
                        }

                        GXDLMSObject[] cols1 = (item as GXDLMSProfileGeneric).GetCaptureObject();

                        // Lire scalers explicitement puis reset pour empêcher Gurux d'auto-appliquer
                        ReadAndResetScalersForCaptureObjects(session, cols1, scalers);

                        var index = count == 0 ? 1 : entriesInUse - (count - 1);
                        var count2 = count == 0 ? entriesInUse : count;

                        object[] rows = session.Reader.ReadRowsByEntry(item as GXDLMSProfileGeneric, Convert.ToUInt32(index), Convert.ToUInt32(count2));
                        object[] cols = new object[cols1.Length];

                        int i = 0;
                        foreach (GXDLMSObject col in cols1)
                        {
                            cols[i++] = col.Name.ToString();
                        }

                        var safeRows = SafeConvertRows(rows);

                        session.Entries.Add(new KeyValuePair<object[], object[]>(safeRows, cols));
                    }
                }

                // Lire TC/TT
                var tctt = ReadTcTtValues(session);
                var envelope = new ProfileDataEnvelope
                {
                    Entries = session.Entries,
                    Scalers = scalers,
                    TcTtValues = tctt
                };

                jsonText = JsonConvert.SerializeObject(envelope, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return jsonText;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Erreur lors de la lecture par entrée");
                return jsonText;
            }
        }

        private static readonly string[] TcTtObis = new[]
        {
            "1.0.0.4.2.255",  // CT numerator
            "1.0.0.4.3.255",  // CT denominator
            "1.0.0.4.5.255",  // VT numerator
            "1.0.0.4.6.255",  // VT denominator
        };

        /// <summary>
        /// Lit l'attribut 3 (scaler+unit) de chaque capture object Register/ExtendedRegister/DemandRegister,
        /// stocke le résultat dans le dictionnaire scalers, puis RESET le Scaler à 1 sur l'objet Gurux
        /// pour empêcher UpdateValue() d'auto-appliquer le scaler lors du parsing du buffer.
        /// Résultat : les valeurs du buffer seront TOUJOURS brutes.
        /// </summary>
        private void ReadAndResetScalersForCaptureObjects(
            IDLMSCommunicationSession session,
            GXDLMSObject[] captureObjects,
            Dictionary<string, ScalerMetadata> scalers)
        {
            foreach (var obj in captureObjects)
            {
                if (scalers.ContainsKey(obj.LogicalName))
                    continue; // Déjà lu pour un profil précédent dans la même session

                if (obj is GXDLMSRegister || obj is GXDLMSExtendedRegister || obj is GXDLMSDemandRegister)
                {
                    try
                    {
                        // Lire attribut 3 (scaler+unit) individuellement
                        int attrIndex = obj is GXDLMSDemandRegister ? 4 : 3;
                        session.Reader.Read(obj, attrIndex);

                        double scaler = 1;
                        int unitCode = 0;

                        if (obj is GXDLMSExtendedRegister ext)
                        {
                            scaler = ext.Scaler;
                            unitCode = (int)ext.Unit;
                            // RESET pour empêcher Gurux d'auto-appliquer
                            ext.Scaler = 1;
                            ext.Unit = GuruxUnit.None;
                        }
                        else if (obj is GXDLMSRegister reg)
                        {
                            scaler = reg.Scaler;
                            unitCode = (int)reg.Unit;
                            reg.Scaler = 1;
                            reg.Unit = GuruxUnit.None;
                        }
                        else if (obj is GXDLMSDemandRegister dem)
                        {
                            scaler = dem.Scaler;
                            unitCode = (int)dem.Unit;
                            dem.Scaler = 1;
                            dem.Unit = GuruxUnit.None;
                        }

                        int exponent = (scaler > 0 && scaler != 1)
                            ? (int)Math.Round(Math.Log10(scaler))
                            : 0;

                        scalers[obj.LogicalName] = new ScalerMetadata
                        {
                            Scaler = scaler,
                            Exponent = exponent,
                            Unit = ((GuruxUnit)unitCode).ToString(),
                            UnitCode = unitCode
                        };

                        _logger?.LogDebug("Scaler lu pour {Obis}: scaler={Scaler}, unit={Unit}",
                            obj.LogicalName, scaler, ((GuruxUnit)unitCode).ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning("Échec lecture scaler pour {Obis}: {Error}",
                            obj.LogicalName, ex.Message);

                        // Scaler inconnu — stocker 1 par défaut mais loguer l'échec
                        scalers[obj.LogicalName] = new ScalerMetadata
                        {
                            Scaler = 1,
                            Exponent = 0,
                            Unit = "None",
                            UnitCode = 0
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Lit l'horloge du compteur (OBIS 0.0.1.0.0.255, attribut 2).
        /// Retourne un JSON avec la date/heure du compteur.
        /// </summary>
        public async Task<string> ReadClockAsync(IDLMSCommunicationSession session)
        {
            try
            {
                if (!session.AssociationLoaded)
                {
                    bool loadedFromFile = false;
                    if (!string.IsNullOrEmpty(session.OutputFile))
                    {
                        try
                        {
                            session.Client.Objects.Clear();
                            session.Client.Objects.AddRange(GXDLMSObjectCollection.Load(session.OutputFile));
                            loadedFromFile = true;
                        }
                        catch { }
                    }
                    if (!loadedFromFile)
                    {
                        session.Reader.GetAssociationView(session.OutputFile);
                    }
                    session.AssociationLoaded = true;
                }

                var clockObj = session.Client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255");
                if (clockObj == null)
                {
                    return JsonConvert.SerializeObject(new { error = "Objet Clock non trouvé sur ce compteur" });
                }

                // Lire attribut 2 (time)
                var clockValue = session.Reader.Read(clockObj, 2);
                var clock = clockObj as GXDLMSClock;

                var result = new
                {
                    clock = clock?.Time?.ToFormatString() ?? clockValue?.ToString(),
                    deviation = clock?.Deviation,
                    status = clock?.Status.ToString(),
                    timeZone = clock?.TimeZone
                };

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "ReadClockAsync echec: {Message}", ex.Message);
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lit les 4 OBIS codes TC/TT depuis le compteur.
        /// Retourne null pour les OBIS non supportés.
        /// </summary>
        private Dictionary<string, double?> ReadTcTtValues(IDLMSCommunicationSession session)
        {
            var result = new Dictionary<string, double?>();
            foreach (var obis in TcTtObis)
            {
                try
                {
                    var obj = session.Client.Objects.FindByLN(ObjectType.None, obis);
                    if (obj != null)
                    {
                        var val = session.Reader.Read(obj, 2);
                        result[obis] = val != null && double.TryParse(val.ToString(), out var d) ? d : null;
                    }
                    else
                    {
                        result[obis] = null;
                    }
                }
                catch
                {
                    result[obis] = null;
                }
            }
            return result;
        }

        private object[] SafeConvertRows(object[] originalRows)
        {
            var result = new List<object>();

            foreach (var item in originalRows)
            {
                if (item == null)
                {
                    result.Add(null);
                }
                else if (item is System.Collections.IEnumerable enumerable && !(item is string))
                {
                    // Cas d'un sous-tableau ou d'une liste
                    var subList = new List<object>();
                    foreach (var subItem in enumerable)
                    {
                        subList.Add(ConvertItem(subItem));
                    }
                    result.Add(subList);
                }
                else
                {
                    // Cas d'un élément simple
                    result.Add(ConvertItem(item));
                }
            }

            return result.ToArray();
        }

        private object ConvertItem(object item)
        {
            if (item == null)
                return null;

            var type = item.GetType();

            if (type.FullName.StartsWith("Gurux.DLMS.GXEnum"))
            {
                // Conversion des GXEnum en int ou string
                return item.ToString(); // ou ((int)item) si possible
            }

            return item;
        }
    }
}
