using System;
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
                    session.Reader.GetAssociationView(session.OutputFile);
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
                    session.Reader.GetAssociationView(session.OutputFile);
                    session.AssociationLoaded = true;
                }

                // 🔴 CRITIQUE POUR LES SCALERS DLMS
                session.Reader.GetScalersAndUnits();

                var entries = new List<KeyValuePair<object[], object[]>>();

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

                    // Lecture lignes par date
                    object[] rows = session.Reader
                            .ReadRowsByRange(profile, datestart2, dateend2);

                    // Colonnes + scalers
                    object[] cols = new object[captureObjects.Length];

                    for (int i = 0; i < captureObjects.Length; i++)
                    {
                        var col = captureObjects[i];
                        cols[i] = col.Name;
                    }

                    var safeRows = SafeConvertRows(rows);

                    entries.Add(new KeyValuePair<object[], object[]>(safeRows, cols));
                }

                return JsonConvert.SerializeObject(entries, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception ex)
            {
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

                jsonText = JsonConvert.SerializeObject(session.Entries, new JsonSerializerSettings
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
