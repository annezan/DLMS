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
using System.Reflection.PortableExecutable;
using System.Linq;
using System.Drawing;

namespace DLMS_COMMUNICATION.Reader
{
    public class ReaderCommunication
    {
        // Méthode d'instance (plus de static)
        public async Task<string> ReadAsync(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            string jsonText = "";
            
            // Création d'une instance pour la lecture
            var connexion = new ConnexionCommunication();
            connexion.readObjects = new List<KeyValuePair<string, int>>();

            try
            {
                var connect = await connexion.ConnectAsync(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
                if (connect == "Connexion réussie")
                {
                    if (connexion.readObjects.Count != 0)
                    {
                        Dictionary<string, object> resultDict = new Dictionary<string, object>();

                        bool read = false;
                        if (connexion.outputFile != null)
                        {
                            try
                            {
                                connexion.client.Objects.Clear();
                                connexion.client.Objects.AddRange(GXDLMSObjectCollection.Load(connexion.outputFile));
                                read = true;
                            }
                            catch (Exception ex)
                            {
                                //It's OK if this fails.
                            }
                        }
                        if (!read)
                        {
                            var reader = new Gurux.DLMS.Reader.GXDLMSReader(connexion.client, connexion.media, connexion.trace, connexion.invocationCounter);
                            reader.GetAssociationView(connexion.outputFile);
                        }
                        
                        foreach (KeyValuePair<string, int> it in connexion.readObjects)
                        {
                            var reader = new Gurux.DLMS.Reader.GXDLMSReader(connexion.client, connexion.media, connexion.trace, connexion.invocationCounter);
                            object val = reader.Read(connexion.client.Objects.FindByLN(ObjectType.None, it.Key), it.Value);
                            
                            if (val is byte[] byteArray)
                            {
                                string decodedString = System.Text.Encoding.UTF8.GetString(byteArray);
                                resultDict[it.Key] = decodedString.Trim();
                            }
                            else
                            {
                                resultDict[it.Key] = val.ToString().Trim();
                            }
                        }
                        
                        // Sérialisation finale en JSON
                        jsonText = JsonConvert.SerializeObject(resultDict, Newtonsoft.Json.Formatting.None);
                    }
                    else
                    {
                        var reader = new Gurux.DLMS.Reader.GXDLMSReader(connexion.client, connexion.media, connexion.trace, connexion.invocationCounter);
                        reader.ReadAll(connexion.outputFile);
                        if (File.Exists(connexion.outputFile))
                        {
                            string xmlFilePath = connexion.outputFile;

                            // Lire le fichier XML
                            XmlDocument doc = new XmlDocument();
                            doc.Load(xmlFilePath);

                            // Convertir XmlDocument en JSON
                            jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                        }
                    }

                    // Fermeture propre
                    if (connexion.media != null)
                    {
                        connexion.media.Close();
                    }

                    return jsonText;
                }

                return "Lecture impossible";
            }
            catch (GXDLMSException ex)
            {
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }
                return "Lecture impossible";
            }
            catch (GXDLMSExceptionResponse ex)
            {
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }
                return "Lecture impossible";
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }
                return "Lecture impossible";
            }
            catch (Exception ex)
            {
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }
                return "Lecture impossible";
            }
        }

        // Méthode statique conservée pour compatibilité (à remplacer progressivement)
        public static string Read(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            // CRÉATION D'UNE INSTANCE POUR MAINTENIR LA COMPATIBILITÉ
            var instance = new ReaderCommunication();
            
            // Utilisation de la version asynchrone en synchrone pour compatibilité
            var task = instance.ReadAsync(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
            task.Wait(); // Attendre la completion (à éviter dans le nouveau code)
            
            return task.Result;
        }

        // Méthode d'instance pour ReadRowsByRange (plus de static)
        public async Task<string> ReadRowsByRangeAsync(string? datestart, string? dateend, string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            string jsonText = "";

            // Création d'une instance pour la lecture
            var connexion = new ConnexionCommunication();
            connexion.readObjects = new List<KeyValuePair<string, int>>();
            connexion.entries2 = new List<KeyValuePair<object[], object[]>>();

            try
            {
                // =========================
                // CONNEXION
                // =========================
                var connect = await connexion.ConnectAsync(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);

                if (connect != "Connexion réussie")
                    return "Lecture impossible";

                if (connexion.readObjects.Count == 0)
                    return "Lecture impossible";

                bool loadedFromFile = false;

                // =========================
                // ASSOCIATION VIEW
                // =========================
                if (connexion.outputFile != null)
                {
                    try
                    {
                        connexion.client.Objects.Clear();
                        connexion.client.Objects.AddRange(
                            GXDLMSObjectCollection.Load(connexion.outputFile));

                        loadedFromFile = true;
                    }
                    catch { }
                }

                if (!loadedFromFile)
                {
                    var reader = new Gurux.DLMS.Reader.GXDLMSReader(connexion.client, connexion.media, connexion.trace, connexion.invocationCounter);
                    reader.GetAssociationView(connexion.outputFile);
                }

                // 🔴 CRITIQUE POUR LES SCALERS DLMS
                var reader2 = new Gurux.DLMS.Reader.GXDLMSReader(connexion.client, connexion.media, connexion.trace, connexion.invocationCounter);
                reader2.GetScalersAndUnits();

                // =========================
                // DATES
                // =========================
                if (!DateTime.TryParse(datestart, out DateTime datestart2))
                    return "Lecture impossible";

                if (!DateTime.TryParse(dateend, out DateTime dateend2))
                    return "Lecture impossible";

                // =========================
                // LECTURE DES PROFILS
                // =========================
                foreach (var it in connexion.readObjects)
                {
                    var profile = connexion.client.Objects
                        .FindByLN(ObjectType.ProfileGeneric, it.Key)
                        as GXDLMSProfileGeneric;

                    if (profile == null)
                        continue;

                    // Lire capture objects (attribut 3)
                    reader2.Read(profile, 3);

                    var captureObjects = profile.GetCaptureObject();

                    // =========================
                    // LECTURE LIGNES PAR DATE
                    // =========================
                    object[] rows = reader2.ReadRowsByRange(profile, datestart2, dateend2);

                    // =========================
                    // COLONNES + SCALERS
                    // =========================
                    object[] cols = new object[captureObjects.Length];

                    for (int i = 0; i < captureObjects.Length; i++)
                    {
                        var col = captureObjects[i];
                        cols[i] = col.Name;
                    }

                    // =========================
                    // SAFE ROWS
                    // =========================
                    var safeRows = SafeConvertRows(rows);

                    connexion.entries2.Add(new KeyValuePair<object[], object[]>(safeRows, cols));
                }

                // =========================
                // JSON
                // =========================
                jsonText = JsonConvert.SerializeObject(
                    connexion.entries2,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                // Fermeture propre
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }

                return jsonText;
            }
            catch (Exception ex)
            {
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }
                return "Lecture impossible";
            }
        }

        // Méthode statique conservée pour compatibilité (à remplacer progressivement)
        public static string ReadRowsByRange(string? datestart, string? dateend, string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            string jsonText = "";

            // Création d'une instance pour la lecture
            var connexion = new ConnexionCommunication();
            connexion.readObjects = new List<KeyValuePair<string, int>>();

            // CRÉATION D'UNE INSTANCE POUR MAINTENIR LA COMPATIBILITÉ
            var instance = new ReaderCommunication();
            
            // Utilisation de la version asynchrone en synchrone pour compatibilité
            var task = instance.ReadRowsByRangeAsync(datestart, dateend, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
            task.Wait(); // Attendre la completion (à éviter dans le nouveau code)
            
            return task.Result;
        }

        // Méthode d'instance pour ReadRowsByEntry (plus de static)
        public async Task<string?> ReadRowsByEntryAsync(
            long count,
            string? port,
            string? serialport,
            string? AddressIp,
            string? ClientAddress,
            string? SerialNumber,
            string? interfaceType,
            string? password,
            string? AuthenticationKey,
            string? UnicastKey,
            string? Objects)
        {
            string? jsonText = null;

            var connexion = new ConnexionCommunication
            {
                readObjects = new List<KeyValuePair<string, int>>(),
                entries2 = new List<KeyValuePair<object[], object[]>>()
            };

            try
            {
                var connect = await connexion.ConnectAsync(
                    port, serialport, AddressIp, ClientAddress,
                    SerialNumber, interfaceType, password,
                    AuthenticationKey, UnicastKey, Objects);

                if (connect != "Connexion réussie")
                    return null;

                if (connexion.readObjects.Count == 0)
                    return null;

                var reader = new Gurux.DLMS.Reader.GXDLMSReader(
                    connexion.client,
                    connexion.media,
                    connexion.trace,
                    connexion.invocationCounter);

                bool associationLoaded = false;

                // 🔹 Tentative chargement depuis fichier
                if (connexion.outputFile != null)
                {
                    try
                    {
                        connexion.client.Objects.Clear();
                        connexion.client.Objects.AddRange(
                            GXDLMSObjectCollection.Load(connexion.outputFile));
                        associationLoaded = true;
                    }
                    catch
                    {
                        // Ignoré volontairement
                    }
                }

                // 🔹 Sinon lecture association
                if (!associationLoaded)
                {
                    reader.GetAssociationView(connexion.outputFile);
                }

                // 🔴 IMPORTANT : charger scalers
                reader.GetScalersAndUnits();

                foreach (var it in connexion.readObjects)
                {
                    var item = connexion.client.Objects
                        .FindByLN(ObjectType.ProfileGeneric, it.Key) as GXDLMSProfileGeneric;

                    if (item == null)
                        continue;

                    reader.Read(item, 3);

                    long entriesInUse = -1;
                    if ((item.GetAccess(7) & AccessMode.Read) != 0)
                    {
                        entriesInUse = Convert.ToInt64(reader.Read(item, 7));
                    }

                    long entries = -1;
                    if ((item.GetAccess(8) & AccessMode.Read) != 0)
                    {
                        entries = Convert.ToUInt32(reader.Read(item, 8));
                    }

                    Console.WriteLine($"Entries: {entriesInUse}/{entries}");

                    if (entriesInUse == 0 || item.CaptureObjects.Count == 0)
                        continue;

                    var colsObjects = item.GetCaptureObject();

                    long startIndex = count == 0 ? 1 : entriesInUse - (count - 1);
                    long countToRead = count == 0 ? entriesInUse : count;

                    var rows = reader.ReadRowsByEntry(
                        item,
                        Convert.ToUInt32(startIndex),
                        Convert.ToUInt32(countToRead));

                    // 🔹 Colonnes
                    object[] cols = colsObjects
                        .Select(c => (object)c.Name)
                        .ToArray();

                    var safeRows = SafeConvertRows(rows);

                    connexion.entries2.Add(
                        new KeyValuePair<object[], object[]>(safeRows, cols));
                }

                jsonText = JsonConvert.SerializeObject(
                    connexion.entries2,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                return jsonText;
            }
            catch
            {
                if (connexion.media != null)
                {
                    connexion.media.Close();
                }
                return "Lecture impossible";
            }

        }

        // Méthode statique conservée pour compatibilité (à remplacer progressivement)
        public static string ReadRowsByEntry(long count, string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            // CRÉATION D'UNE INSTANCE POUR MAINTENIR LA COMPATIBILITÉ
            var instance = new ReaderCommunication();
            
            // Utilisation de la version asynchrone en synchrone pour compatibilité
            var task = instance.ReadRowsByEntryAsync(count, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
            task.Wait(); // Attendre la completion (à éviter dans le nouveau code)
            
            return task.Result;
        }

        private static object[] SafeConvertRows(object[] originalRows)
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

        private static object ConvertItem(object item)
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
