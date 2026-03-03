using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects.Enums;
using Gurux.Net;
using Gurux.Serial;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System;
using Gurux.DLMS.Secure;
using Gurux.DLMS.Objects;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Newtonsoft.Json;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using System.Globalization;
using NuGet.Configuration;
using DLMS_MODELS;
using DLMS_BUSINESS;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
namespace API_DLMS
{
    public class Connexion
    {
        public IGXMedia media = null;
        public TraceLevel trace = TraceLevel.Info;
        public GXDLMSSecureClient client = new GXDLMSSecureClient(true);
        // Invocation counter (frame counter).
        public string invocationCounter = null;
        //Objects to read.
        public List<KeyValuePair<string, int>> readObjects = new List<KeyValuePair<string, int>>();
        public List<KeyValuePair<string, object>> resultObjects = new List<KeyValuePair<string, object>>();
        public List<KeyValuePair<object[], GXDLMSObject[]>> entries=new List<KeyValuePair<object[], GXDLMSObject[]>>();
        public List<KeyValuePair<string, object>>Unitscalers = new List<KeyValuePair<string, object>>();
        public List<Dictionary<string, string?>> queryParametersList = new List<Dictionary<string, string?>>();
        //Cache file.
        public string outputFile = null;


        public static void GetParameters(Connexion setting,string port, string serialport, string AddressIp, string ClientAddress, string SerialNumber, string interfaceType, string password, string AuthenticationKey, string UnicastKey, string Objects)
        {
            try
            {
                GXSerial serial;
                bool modeEDefaultValues = true;
                string[] tmp;
                // Configuration des paramètres de connexion en fonction des paramètres de la requête
                if (AddressIp!= null)
                {
                    if (setting.media == null)
                    {
                        setting.media = new GXNet();
                    }
                    if (setting.media is GXNet net)
                    {
                        net.HostName = AddressIp;
                    }

                }
                if (port!=null)
                {
                    //Port.
                    if (setting.media == null)
                    {
                        setting.media = new GXNet();
                    }
                    if (setting.media is GXNet net)
                    {
                        net.Port = int.Parse(port);
                    }
                }
                if (serialport != null)
                {
                    setting.media = new GXSerial();
                    serial = setting.media as GXSerial;
                    tmp = serialport.Split(':');
                    serial.PortName = tmp[0];
                    if (tmp.Length > 1)
                    {
                        modeEDefaultValues = false;
                        serial.BaudRate = int.Parse(tmp[1]);
                        serial.DataBits = int.Parse(tmp[2].Substring(0, 1));
                        serial.Parity = (Parity)Enum.Parse(typeof(Parity), tmp[2].Substring(1, tmp[2].Length - 2));
                        serial.StopBits = (StopBits)int.Parse(tmp[2].Substring(tmp[2].Length - 1, 1));
                    }
                    else
                    {
                        if (setting.client.InterfaceType == InterfaceType.HdlcWithModeE)
                        {
                            serial.BaudRate = 300;
                            serial.DataBits = 7;
                            serial.Parity = Parity.Even;
                            serial.StopBits = StopBits.One;
                        }
                        else
                        {
                            serial.BaudRate = 9600;
                            serial.DataBits = 8;
                            serial.Parity = Parity.None;
                            serial.StopBits = StopBits.One;
                        }
                    }
                }
                if (password != null)
                {
                    if (password.StartsWith("0x"))
                    {
                        setting.client.Password = GXCommon.HexToBytes(password.Substring(2));
                    }
                    else
                    {
                        setting.client.Password = ASCIIEncoding.ASCII.GetBytes(password);
                    }
                }
                if (interfaceType != null)
                {
                    try
                    {
                        setting.client.InterfaceType = (InterfaceType)Enum.Parse(typeof(InterfaceType), interfaceType);
                        if (setting.client.InterfaceType== InterfaceType.HDLC)
                        {
                            setting.client.HdlcSettings.WindowSizeRX = setting.client.HdlcSettings.WindowSizeTX = 1;
                            setting.client.HdlcSettings.MaxInfoRX = setting.client.HdlcSettings.MaxInfoTX = 128;
                        }
                        setting.client.Plc.Reset();
                        if (modeEDefaultValues && setting.client.InterfaceType == InterfaceType.HdlcWithModeE &&
                            setting.media is GXSerial)
                        {
                            serial = setting.media as GXSerial;
                            serial.BaudRate = 300;
                            serial.DataBits = 7;
                            serial.Parity = Parity.Even;
                            serial.StopBits = StopBits.One;
                        }
                        if (setting.client.InterfaceType == InterfaceType.CoAP)
                        {
                            if (setting.media is GXNet net)
                            {
                                net.Protocol = NetworkType.Udp;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Invalid interface type option. (HDLC, WRAPPER, HdlcWithModeE, Plc, PlcHdlc)");
                    }

                }
                if (AuthenticationKey != null)
                {
                    setting.client.Ciphering.AuthenticationKey = GXCommon.HexToBytes(AuthenticationKey);

                }
                if (UnicastKey != null)
                {
                    setting.client.Ciphering.BlockCipherKey = GXCommon.HexToBytes(UnicastKey);
                }
                if (ClientAddress != null)
                {
                    if (ClientAddress == "public")
                    {
                        setting.client.ClientAddress = 16;
                        setting.outputFile = "public.xml";
                    }
                    else if (ClientAddress == "read")
                    {
                        setting.client.ClientAddress = 2;
                        setting.client.Authentication = Authentication.HighGMAC;
                        setting.client.Ciphering.Security = Security.AuthenticationEncryption;
                        setting.outputFile = "Read.xml";
                    }
                    else if (ClientAddress == "managed")
                    {
                        setting.client.ClientAddress = 1;
                        setting.client.Authentication = Authentication.HighGMAC;
                        setting.client.Ciphering.Security = Security.AuthenticationEncryption;
                        setting.outputFile = "Managed.xml";
                    }
                    else if (ClientAddress == "fwu")
                    {
                        setting.client.ClientAddress = 3;
                        setting.client.Authentication = Authentication.HighGMAC;
                        setting.client.Ciphering.Security = Security.AuthenticationEncryption;
                        setting.outputFile = "fwu.xml";
                    }
                }
                if (SerialNumber != null)
                {
                    if (int.Parse(SerialNumber) != 1)
                    {
                        var PhysicalAddress = int.Parse(SerialNumber.Substring(SerialNumber.Length - 4)) + 100;
                        setting.client.ServerAddress = GXDLMSClient.GetServerAddress(1, PhysicalAddress);
                        
                    }
                    else
                    {
                        setting.client.ServerAddress = int.Parse(SerialNumber);
                    }
                }
                if (Objects != null)
                {
                    foreach (string o in Objects.Split(new char[] { ';', ',' }))
                    {
                        tmp = o.Split(new char[] { ':' });
                        if (tmp.Length != 2)
                        {
                            throw new ArgumentOutOfRangeException("Invalid Logical name or attribute index.");
                        }
                        setting.readObjects.Add(new KeyValuePair<string, int>(tmp[0].Trim(), int.Parse(tmp[1].Trim())));
                    }

                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        public static string Read(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey)
        {
            Connexion Settings = new Connexion();
            Gurux.DLMS.Reader.GXDLMSReader reader = null;
            GXDLMSObjectCollection Result = new GXDLMSObjectCollection();
            object ResultObis = null;
            int it_value = 0;
            string jsonText = "";

            try
            {
                ////////////////////////////////////////
                //Handle command line parameters.
                Connexion.GetParameters(Settings,port,serialport,AddressIp,ClientAddress, SerialNumber, interfaceType, password,AuthenticationKey, UnicastKey,null);

                ////////////////////////////////////////
                //Initialize connection settings.
                if (Settings.media is GXSerial)
                {
                }
                else if (Settings.media is GXNet)
                {
                }
                else
                {
                    throw new Exception("Unknown media type.");
                }
                ////////////////////////////////////////
                reader = new Gurux.DLMS.Reader.GXDLMSReader(Settings.client, Settings.media, Settings.trace, Settings.invocationCounter);
                reader.OnNotification += (data) =>
                {
                    var essai1 = data;
                    Console.WriteLine(data);
                };
                //Create manufacturer spesific custom COSEM object.
                Settings.client.OnCustomObject += (type, version) =>
                {
                    /*
                    if (type == 6001 && version == 0)
                    {
                        return new ManufacturerSpesificObject();
                    }
                    */
                    return null;
                };

                try
                {
                    Settings.media.Open();
                }
                catch (System.IO.IOException ex)
                {
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Available ports:");
                    Console.WriteLine(string.Join(" ", GXSerial.GetPortNames()));

                }
                //Some meters need a break here.
                //Thread.Sleep(1000);
                
                //Generate new client and server certificates and import them to the server.
                

                reader.ReadAll(Settings.outputFile);
                    
                
            }
            catch (GXDLMSException ex)
            {
                Console.WriteLine(ex.Message);
                
                return ex.Message + "500";
            }
            catch (GXDLMSExceptionResponse ex)
            {
                Console.WriteLine(ex.Message);
                
                return ex.Message + "500";
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                Console.WriteLine(ex.Message);
                
                return ex.Message + "500";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                
                return ex.Message + "500";
            }

            if (reader != null)
            {
                reader.Close();
            }
            if (File.Exists(Settings.outputFile))
            {
                string xmlFilePath = Settings.outputFile;

                // Leer el archivo XML
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);

                // Convertir XmlDocument a JSON
                jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
                Console.WriteLine(jsonText);
                //string jsonFilePath = "ruta/al/archivo.json";
                //System.IO.File.WriteAllText(jsonFilePath, jsonText);

            }
            return jsonText + "200";



            GXReplyData reply = new GXReplyData();
            //object val2 = reader.Write(Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255"), 2);
            //GXDLMSClock dlms= new GXDLMSClock("0.0.1.0.0.255", 0);
            //GXDLMSClock item = (GXDLMSClock)Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255");
            //var essai = item;
            //GXDLMSWrite
            //string dataToWrite = "Nouvelles données à écrire";

            // Créer une demande d'écriture.
            //GXDLMSData data = new GXDLMSData(dataToWrite);
            //GXDLMSObject objToWrite = new GXDLMSObject(0, ObjectType.Clock, 0, 0);
            //GXDLMSClient.WriteResult result = Settings.client.Write(objToWrite, data);

            
        }

        public static async Task<string> ReadObjectsProfile(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            Connexion Settings = new Connexion();
            Gurux.DLMS.Reader.GXDLMSReader reader = null;
            GXDLMSObjectCollection Result = new GXDLMSObjectCollection();
            object ResultObis = null;
            int it_value = 0;
            string jsonText = "";
            var detailprofilenr =false;

            try
            {
                ////////////////////////////////////////
                //Handle command line parameters.
                Connexion.GetParameters(Settings, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);

                ////////////////////////////////////////
                //Initialize connection settings.
                if (Settings.media is GXSerial)
                {
                }
                else if (Settings.media is GXNet)
                {
                }
                else
                {
                    throw new Exception("Unknown media type.");
                }
                ////////////////////////////////////////
                reader = new Gurux.DLMS.Reader.GXDLMSReader(Settings.client, Settings.media, Settings.trace, Settings.invocationCounter);
                reader.OnNotification += (data) =>
                {
                    var essai1 = data;
                    Console.WriteLine(data);
                };
                //Create manufacturer spesific custom COSEM object.
                Settings.client.OnCustomObject += (type, version) =>
                {
                    /*
                    if (type == 6001 && version == 0)
                    {
                        return new ManufacturerSpesificObject();
                    }
                    */
                    return null;
                };

                try
                {
                    Settings.media.Open();
                }
                catch (System.IO.IOException ex)
                {
                    return ex.Message;
                }
                //Some meters need a break here.
                //System.Threading.Tasks.Task.Delay(1000);


                //Generate new client and server certificates and import them to the server.
                if (Settings.readObjects.Count != 0)
                {
                    bool read = false;
                    if (Settings.outputFile != null)
                    {
                        try
                        {
                            Settings.client.Objects.Clear();
                            Settings.client.Objects.AddRange(GXDLMSObjectCollection.Load(Settings.outputFile));
                            read = true;
                        }
                        catch (Exception)
                        {
                            //It's OK if this fails.
                        }
                    }
                    reader.InitializeConnection();
                    if (!read)
                    {
                        reader.GetAssociationView(Settings.outputFile);
                    }
                    foreach (KeyValuePair<string, int> it in Settings.readObjects)
                    {
                        foreach (GXDLMSObject item in Settings.client.Objects.GetObjects(ObjectType.ProfileGeneric))
                        {
                            
                            if (item.LogicalName==it.Key)
                            {
                                //reader.Read(item, 3);

                                //long entriesInUse = -1;
                                //if ((item.GetAccess(7) & AccessMode.Read) != 0)
                                //{
                                //    entriesInUse = Convert.ToInt64(reader.Read(item, 7));
                                //}
                                //long entries = -1;
                                //if ((item.GetAccess(8) & AccessMode.Read) != 0)
                                //{
                                //    entries = Convert.ToUInt32(reader.Read(item, 8));
                                //}
                                ////If trace is info.

                                //Console.WriteLine("Entries: " + entriesInUse + "/" + entries);

                                ////If there are no columns or rows.
                                //if (entriesInUse == 0 || (item as GXDLMSProfileGeneric).CaptureObjects.Count == 0)
                                //{
                                //    continue;
                                //}
                                GXDLMSObject[] cols = (item as GXDLMSProfileGeneric).GetCaptureObject();
                                //var count = entriesInUse - 812;
                                var start = "05/12/2024 00:00:00";
                                DateTime datestart = DateTime.Parse(start);
                                DateTime dateend =  new DateTime(
                                            DateTime.Now.Year,  
                                            DateTime.Now.Month, 
                                            DateTime.Now.Day,   
                                            DateTime.Now.Hour,  
                                            0,                  
                                            0                   
                                );


                                object[] rows = reader.ReadRowsByRange(item as GXDLMSProfileGeneric,datestart,dateend);
                                //Console.WriteLine("Entries: "+ rows.Length);
                                //object[] rows = reader.ReadRowsByEntry(item as GXDLMSProfileGeneric, Convert.ToUInt32(1), Convert.ToUInt32(entriesInUse));
                                Settings.entries.Add(new KeyValuePair<object[], GXDLMSObject[]>(rows, cols));
                                Gxdlmsprofilgenericdetail detailprofil= new Gxdlmsprofilgenericdetail();
                                Gxdlmsprofilgenericdetailsevent detailprofilevent = new Gxdlmsprofilgenericdetailsevent();
                                GxdlmsProfilgenericBusiness profilgenericBusiness= new GxdlmsProfilgenericBusiness();
                                CodeObisBusiness codeObisBusiness = new CodeObisBusiness();
                                EventlistBusiness eventlistBusiness = new EventlistBusiness();
                                foreach (KeyValuePair<object[], GXDLMSObject[]> item1 in Settings.entries)
                                {

                                    if (it.Key == "1.0.99.1.0.255" || it.Key == "1.0.99.2.0.255" || it.Key == "1.0.99.3.0.255" || it.Key == "0.0.98.1.0.255")
                                    {

                                        var profilgenericList = await profilgenericBusiness.GetProfilegeneric(it.Key);

                                        // Parcourir chaque élément dans la clé (qui est un tableau d'objets)
                                        for (int j = 0; j < item1.Key.Length; j++)
                                        {
                                            var key = item1.Key[j];
                                            DateTime dateUtc = DateTime.Now;
                                            if (key is object[] array)
                                            {
                                                List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();


                                                for (int i = 0; i < array.Length; i++)
                                                {
                                                    var obj = item1.Value[i];

                                                    if (i == 0) // Première valeur
                                                    {

                                                        var codeobisId = await codeObisBusiness.GetCodeObisId(obj.Name.ToString());

                                                        // Si la première valeur est convertible en DateTime
                                                        long unixTimestamp = Convert.ToUInt32(array[i]);

                                                        // Convertir le timestamp en date UTC
                                                        dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                        detailprofil.DateEnr = dateUtc;
                                                        detailprofil.GxdlmsprofilgenericId = profilgenericList.Id;
                                                        detailprofil.Codeobisid = codeobisId.Id;
                                                        detailprofil.Codeobislibelle = codeobisId.Code;
                                                        detailprofil.Codeobisvalue = codeobisId.Value;
                                                        detailprofil.Profilgenericlibelle = profilgenericList.Type;

                                                        //detailprofil.Compteurid = "APAESX30" + SerialNumber;
                                                        detailprofil.Status = "actif";
                                                        detailprofil.Unit = "";
                                                        detailprofil.Value = array[i].ToString();
                                                        await profilgenericBusiness.PostGxdlmsprofilgeneric(detailprofil);


                                                    }
                                                    else
                                                    {
                                                        try
                                                        {

                                                            if (item1.Value[i] is GXDLMSRegister || item1.Value[i] is GXDLMSDemandRegister)
                                                            {

                                                                var datatype = array[i].GetType();
                                                                if (datatype == typeof(GXDateTime))
                                                                {
                                                                    var valconvert1 = array[i].ToString();
                                                                    detailprofil.Value = valconvert1;
                                                                }
                                                                else if (datatype == typeof(decimal))
                                                                {
                                                                    var valconvert = Convert.ToDecimal(array[i]);
                                                                    detailprofil.Value = valconvert.ToString();
                                                                }
                                                                                    
                                                                                    
                                                                var codeobisId = await codeObisBusiness.GetCodeObisId(obj.Name.ToString());

                                                                detailprofil.DateEnr = dateUtc;
                                                                detailprofil.GxdlmsprofilgenericId = profilgenericList.Id;
                                                                detailprofil.Codeobisid = codeobisId.Id;
                                                                detailprofil.Unit = codeobisId.TagType;
                                                                //detailprofil.Compteurid = "APAESX30" + SerialNumber;
                                                                detailprofil.Status = "actif";
                                                                detailprofil.Codeobislibelle = codeobisId.Code;
                                                                detailprofil.Codeobisvalue = codeobisId.Value;
                                                                detailprofil.Profilgenericlibelle = profilgenericList.Type;
                                                                await profilgenericBusiness.PostGxdlmsprofilgeneric(detailprofil);

                                                            }
                                                            else
                                                            {
                                                                detailprofil.Unit = "";
                                                                detailprofil.Value = array[i].ToString();
                                                                var codeobisId = await codeObisBusiness.GetCodeObisId(obj.Name.ToString());
                                                                detailprofil.DateEnr = dateUtc;
                                                                detailprofil.GxdlmsprofilgenericId = profilgenericList.Id;
                                                                detailprofil.Codeobisid = codeobisId.Id;
                                                                //detailprofil.Compteurid = "APAESX30" + SerialNumber;
                                                                detailprofil.Status = "actif";
                                                                detailprofil.Codeobislibelle = codeobisId.Code;
                                                                detailprofil.Codeobisvalue = codeobisId.Value;
                                                                detailprofil.Profilgenericlibelle = profilgenericList.Type;
                                                                await profilgenericBusiness.PostGxdlmsprofilgeneric(detailprofil);
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.ToString());
                                                            // Actaric SL7000 peut retourner une erreur ici. Continuer la lecture.
                                                        }



                                                    }


                                                }

                                            }


                                        }
                                    }
                                    else if (it.Key == "0.0.99.98.0.255" || it.Key == "0.0.99.98.1.255" || it.Key == "0.0.99.98.2.255" || it.Key == "0.0.99.98.3.255" || it.Key == "0.0.99.98.4.255" || it.Key == "0.0.99.98.5.255" || it.Key == "0.0.99.98.6.255" || it.Key == "0.0.99.98.7.255")
                                    {
                                        for (int j = 0; j < item1.Key.Length; j++)
                                        {
                                            DateTime dateUtc = DateTime.Now;
                                            var key = item1.Key[j];
                                            if (key is object[] array)
                                            {

                                                List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();
                                                var profilgenericList = await profilgenericBusiness.GetProfilegeneric(it.Key);


                                                for (int i = 0; i < array.Length; i++)
                                                {

                                                    if (item1.Value[i].Name.ToString() == "0.0.96.11.0.255")
                                                    {
                                                        var eventresult = await eventlistBusiness.GetEventlist(array[i].ToString());

                                                        detailprofilevent.Value = array[i].ToString();
                                                        detailprofilevent.Description = eventresult.Description2 == "" ? "Événement inconnu": eventresult.Description2;
                                                        detailprofilevent.Category = eventresult.Category == "" ? "Événement inconnu" : eventresult.Category;

                                                    }
                                                    else
                                                    {

                                                        detailprofilevent.Value = array[i].ToString();
                                                        detailprofilevent.Description = "";
                                                        detailprofilevent.Category = "";

                                                    }

                                                    if (i==0)
                                                    {
                                                        long unixTimestamp = Convert.ToUInt32(array[i]);
                                                        dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                    }
                                                    // Convertir le timestamp en date UTC
                                                    var obj = item1.Value[i];
                                                    var codeobisId = await codeObisBusiness.GetCodeObisId(obj.Name.ToString());
                                                    
                                                    detailprofilevent.DateEnr = dateUtc;
                                                    detailprofilevent.GxdlmsprofilgenericId = profilgenericList.Id;
                                                    detailprofilevent.Codeobisid = codeobisId.Id;
                                                    //detailprofilevent.Compteurid = "APAESX30" + SerialNumber;
                                                    detailprofilevent.Status = "actif";
                                                    detailprofilevent.Codeobislibelle = codeobisId.Code;
                                                    detailprofilevent.Codeobisvalue = codeobisId.Value;
                                                    detailprofilevent.Profilgenericlibelle = profilgenericList.Type;
                                                    await profilgenericBusiness.PostGxdlmsprofilgenericevent(detailprofilevent);
                                                }
                                            }
                                            
                                        }
                                    }

                                }



                            }
                        }

                    }
                    if (Settings.outputFile != null)
                    {
                        try
                        {
                            Settings.client.Objects.Save(Settings.outputFile, new GXXmlWriterSettings() { UseMeterTime = true, IgnoreDefaultValues = false });

                        }
                        catch (Exception)
                        {
                            //It's OK if this fails.
                        }
                    }
                }

            }
            catch (GXDLMSException ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (GXDLMSExceptionResponse ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }

            if (reader != null)
            {
                reader.Close();
            }
            if (detailprofilenr ==true)
            {
                jsonText= "lecture effectué";

            }
            return jsonText + "200";



            //GXReplyData reply = new GXReplyData();
            //object val2 = reader.Write(Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255"), 2);
            //GXDLMSClock dlms= new GXDLMSClock("0.0.1.0.0.255", 0);
            //GXDLMSClock item = (GXDLMSClock)Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255");
            //var essai = item;
            //GXDLMSWrite
            //string dataToWrite = "Nouvelles données à écrire";

            // Créer une demande d'écriture.
            //GXDLMSData data = new GXDLMSData(dataToWrite);
            //GXDLMSObject objToWrite = new GXDLMSObject(0, ObjectType.Clock, 0, 0);
            //GXDLMSClient.WriteResult result = Settings.client.Write(objToWrite, data);

        }

        public static async Task<string> ReadObjectsMajCompteur(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            Connexion Settings = new Connexion();
            Gurux.DLMS.Reader.GXDLMSReader reader = null;
            GXDLMSObjectCollection Result = new GXDLMSObjectCollection();
            CompteurBusiness  compteurBusiness = new CompteurBusiness();
            object ResultObis = null;
            int it_value = 0;
            string jsonText = "";

            try
            {
                ////////////////////////////////////////
                //Handle command line parameters.
                Connexion.GetParameters(Settings, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);

                ////////////////////////////////////////
                //Initialize connection settings.
                if (Settings.media is GXSerial)
                {
                }
                else if (Settings.media is GXNet)
                {
                }
                else
                {
                    throw new Exception("Unknown media type.");
                }
                ////////////////////////////////////////
                reader = new Gurux.DLMS.Reader.GXDLMSReader(Settings.client, Settings.media, Settings.trace, Settings.invocationCounter);
                reader.OnNotification += (data) =>
                {
                    var essai1 = data;
                    Console.WriteLine(data);
                };
                //Create manufacturer spesific custom COSEM object.
                Settings.client.OnCustomObject += (type, version) =>
                {
                    /*
                    if (type == 6001 && version == 0)
                    {
                        return new ManufacturerSpesificObject();
                    }
                    */
                    return null;
                };

                try
                {
                    Settings.media.Open();
                }
                catch (System.IO.IOException ex)
                {
                    throw new Exception(ex.Message);

                }
                //Some meters need a break here.
                //Thread.Sleep(1000);

                //Generate new client and server certificates and import them to the server.
                if (Settings.readObjects.Count != 0)
                {
                    bool read = false;
                    if (Settings.outputFile != null)
                    {
                        try
                        {
                            Settings.client.Objects.Clear();
                            Settings.client.Objects.AddRange(GXDLMSObjectCollection.Load(Settings.outputFile));
                            read = true;
                        }
                        catch (Exception)
                        {
                            //It's OK if this fails.
                        }
                    }
                    reader.InitializeConnection();
                    if (!read)
                    {
                        reader.GetAssociationView(Settings.outputFile);
                    }
                    Compteur compteur1 = new Compteur();
                    foreach (KeyValuePair<string, int> it in Settings.readObjects)
                    {
                        object val = reader.Read(Settings.client.Objects.FindByLN(ObjectType.None, it.Key), it.Value);
                        if (val is byte[] byteArray)
                        {
                            string decodedString = System.Text.Encoding.UTF8.GetString(byteArray);
                            Console.WriteLine(decodedString);
                            Settings.resultObjects.Add(new KeyValuePair<string, object>(it.Key, decodedString));

                        }
                        else
                        {
                            Settings.resultObjects.Add(new KeyValuePair<string, object>(it.Key, val));

                        }

                    }
                        
                        foreach (KeyValuePair<string, object> item in Settings.resultObjects)
                        {
                            if (item.Key == "0.0.42.0.0.255")
                            {
                                compteur1.Idfabricant = item.Value.ToString().Substring(0, 3);
                                compteur1.Typecompteur = item.Value.ToString().Substring(3, 4);
                            }
                            else if (item.Key == "0.0.96.2.128.255")
                            {
                                compteur1.DataConcentrator = item.Value.ToString();
                            }
                            else if (item.Key == "1.0.99.1.0.255")
                            {
                                var i = Convert.ToInt32(item.Value) / 60;
                                compteur1.EnergyProfilePeriod = i.ToString();
                            }
                            else if (item.Key == "1.0.99.2.0.255")
                            {
                                var i = Convert.ToInt32(item.Value) / 60;
                                compteur1.TechnicalProfilePeriod = i.ToString();
                            }
                            else if (item.Key == "0.0.0.2.8.255")
                            {
                                compteur1.CrcFirmware = item.Value.ToString();
                            }
                            else if (item.Key == "0.0.0.2.0.255")
                            {
                                compteur1.VersionFirmware = item.Value.ToString();
                            }
                            else if (item.Key == "1.0.0.2.2.255")
                            {
                                compteur1.Tarif = item.Value.ToString();
                            }


                        }

                    
                    compteur1.TypeOfTransport = "HDLC";
                    var compteurUpdate = await compteurBusiness.EditCompteur(compteur1);
                }

            }
            catch (GXDLMSException ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }

                return ex.Message + "500";
            }
            catch (GXDLMSExceptionResponse ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }

            

            return "Enregistrement réussi200";



            //GXReplyData reply = new GXReplyData();
            //object val2 = reader.Write(Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255"), 2);
            //GXDLMSClock dlms= new GXDLMSClock("0.0.1.0.0.255", 0);
            //GXDLMSClock item = (GXDLMSClock)Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255");
            //var essai = item;
            //GXDLMSWrite
            //string dataToWrite = "Nouvelles données à écrire";

            // Créer une demande d'écriture.
            //GXDLMSData data = new GXDLMSData(dataToWrite);
            //GXDLMSObject objToWrite = new GXDLMSObject(0, ObjectType.Clock, 0, 0);
            //GXDLMSClient.WriteResult result = Settings.client.Write(objToWrite, data);


        }

        public static async Task<string> ReadObjectsCommande(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects, int? Idcommande, int? Nombreentree, DateTime? DateDebut, DateTime? DateFin)
        {
            Connexion Settings = new Connexion();
            Gurux.DLMS.Reader.GXDLMSReader reader = null;
            GXDLMSObjectCollection Result = new GXDLMSObjectCollection();
            object ResultObis = null;
            int it_value = 0;
            string jsonText = "";
            var detailprofilenr = false;

            try
            {
                ////////////////////////////////////////
                //Handle command line parameters.
                Connexion.GetParameters(Settings, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);

                ////////////////////////////////////////
                //Initialize connection settings.
                if (Settings.media is GXSerial)
                {
                }
                else if (Settings.media is GXNet)
                {
                }
                else
                {
                    throw new Exception("Unknown media type.");
                }
                ////////////////////////////////////////
                reader = new Gurux.DLMS.Reader.GXDLMSReader(Settings.client, Settings.media, Settings.trace, Settings.invocationCounter);
                reader.OnNotification += (data) =>
                {
                    var essai1 = data;
                    Console.WriteLine(data);
                };
                //Create manufacturer spesific custom COSEM object.
                Settings.client.OnCustomObject += (type, version) =>
                {
                    /*
                    if (type == 6001 && version == 0)
                    {
                        return new ManufacturerSpesificObject();
                    }
                    */
                    return null;
                };

                try
                {
                    Settings.media.Open();
                }
                catch (System.IO.IOException ex)
                {
                    return ex.Message;
                }
                //Some meters need a break here.
                //System.Threading.Tasks.Task.Delay(1000);


                //Generate new client and server certificates and import them to the server.
                if (Settings.readObjects.Count != 0)
                {
                    bool read = false;
                    if (Settings.outputFile != null)
                    {
                        try
                        {
                            Settings.client.Objects.Clear();
                            Settings.client.Objects.AddRange(GXDLMSObjectCollection.Load(Settings.outputFile));
                            read = true;
                        }
                        catch (Exception)
                        {
                            //It's OK if this fails.
                        }
                    }
                    reader.InitializeConnection();
                    if (!read)
                    {
                        reader.GetAssociationView(Settings.outputFile);
                    }
                    foreach (KeyValuePair<string, int> it in Settings.readObjects)
                    {
                        foreach (GXDLMSObject item in Settings.client.Objects.GetObjects(ObjectType.ProfileGeneric))
                        {
                            object[] rows=new object[0];
                            GXDLMSObject[] cols;
                            if (item.LogicalName == it.Key)
                            {
                                if (it.Key== "0.0.98.1.0.255")
                                {
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
                                    //If trace is info.

                                    Console.WriteLine("Entries: " + entriesInUse + "/" + entries);

                                    //If there are no columns or rows.
                                    if (entriesInUse == 0 || (item as GXDLMSProfileGeneric).CaptureObjects.Count == 0)
                                    {
                                        continue;
                                    }
                                    var count = entriesInUse - Nombreentree;
                                    rows = reader.ReadRowsByEntry(item as GXDLMSProfileGeneric, Convert.ToUInt32(count), Convert.ToUInt32(Nombreentree));

                                }
                                else if (it.Key == "1.0.99.1.0.255:2" || it.Key == "1.0.99.2.0.255:2")
                                {
                                    var start = DateDebut;
                                    DateTime datestart = new DateTime(
                                                start.Value.Year,
                                                start.Value.Month,
                                                start.Value.Day,
                                                start.Value.Hour,
                                                0,
                                                0
                                    );
                                    var end = DateFin;
                                    DateTime dateend = new DateTime(
                                                end.Value.Year,
                                                end.Value.Month,
                                                end.Value.Day,
                                                end.Value.Hour,
                                                0,
                                                0
                                    );
                                    rows = reader.ReadRowsByRange(item as GXDLMSProfileGeneric, datestart, dateend);
                                }
                                else if (it.Key== "0.0.1.0.0.255:2")
                                {
                                    ResultObis = reader.Read(item , it.Value);
                                }
                                cols = (item as GXDLMSProfileGeneric).GetCaptureObject();
                                
                               
                                Settings.entries.Add(new KeyValuePair<object[], GXDLMSObject[]>(rows, cols));
                                Gxdlmsprofilgenericdetail detailprofil = new Gxdlmsprofilgenericdetail();
                                GxdlmsProfilgenericBusiness profilgenericBusiness = new GxdlmsProfilgenericBusiness();
                                Commande commande = new Commande();
                                Commandecompteur commandecompteur = new Commandecompteur();
                                CommandeBusiness commandeBusiness = new CommandeBusiness();
                                CodeObisBusiness codeObisBusiness = new CodeObisBusiness();
                                EventlistBusiness eventlistBusiness = new EventlistBusiness();
                                int t = 0;
                                foreach (KeyValuePair<object[], GXDLMSObject[]> item1 in Settings.entries)
                                {

                                    var profilgenericList = await profilgenericBusiness.GetProfilegeneric(it.Key);
                                    // Parcourir chaque élément dans la clé (qui est un tableau d'objets)
                                    for (int j = 0; j < item1.Key.Length; j++)
                                    {
                                        var key = item1.Key[j];
                                        DateTime dateUtc = DateTime.Now;
                                        if (key is object[] array)
                                        {
                                            List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();


                                            for (int i = 0; i < array.Length; i++)
                                            {
                                                var obj = item1.Value[i];

                                                if (i == 0) // Première valeur
                                                {
                                                    

                                                    // Si la première valeur est convertible en DateTime
                                                    long unixTimestamp = Convert.ToUInt32(array[i]);

                                                    // Convertir le timestamp en date UTC
                                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                    var queryParameters = new Dictionary<string, string?>
                                                    {
                                                        { "CodeObis", obj.Name.ToString() },
                                                        { "DateEnr", dateUtc.ToString() },
                                                        { "Value", array[i].ToString() },
                                                        { "Unit", "" }
                                                        
                                                    };
                                                    ResultObis += System.Text.Json.JsonSerializer.Serialize(queryParameters);

                                                }
                                                else
                                                {
                                                    try
                                                    {

                                                        if (item1.Value[i] is GXDLMSRegister || item1.Value[i] is GXDLMSDemandRegister)
                                                        {

                                                            var datatype = array[i].GetType();
                                                            if (datatype == typeof(GXDateTime))
                                                            {
                                                                var valconvert1 = array[i].ToString();
                                                                detailprofil.Value = valconvert1;
                                                            }
                                                            else if (datatype == typeof(decimal))
                                                            {
                                                                var valconvert = Convert.ToDecimal(array[i]);
                                                                detailprofil.Value = valconvert.ToString();
                                                            }


                                                            var codeobisId = await codeObisBusiness.GetCodeObisId(obj.Name.ToString());

                                                            var queryParameters = new Dictionary<string, string?>
                                                            {
                                                                { "CodeObis", obj.Name.ToString() },
                                                                { "DateEnr", dateUtc.ToString() },
                                                                { "Value", detailprofil.Value.ToString() },
                                                                { "Unit", codeobisId.TagType }

                                                            };
                                                            ResultObis += System.Text.Json.JsonSerializer.Serialize(queryParameters);

                                                        }
                                                        else
                                                        {
                                                            
                                                            var queryParameters = new Dictionary<string, string?>
                                                            {
                                                                { "CodeObis", obj.Name.ToString() },
                                                                { "DateEnr", dateUtc.ToString() },
                                                                { "Value", array[i].ToString().ToString() },
                                                                { "Unit", "" }

                                                            };
                                                            ResultObis += System.Text.Json.JsonSerializer.Serialize(queryParameters);

                                                            
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.ToString());
                                                        // Actaric SL7000 peut retourner une erreur ici. Continuer la lecture.
                                                    }



                                                }


                                            }

                                        }


                                    }

                                    ResultObis += System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>
                                    {
                                        { "Resultat" + t, ResultObis.ToString() }
                                    });

                                    t++;
                                }
                                commande.Statut = "";
                                commandecompteur.Resultats = ResultObis.ToString();
                                commandecompteur.Dateenrresultat= DateTime.Now;
                                await commandeBusiness.EditCommande(commande);
                                await commandeBusiness.EditCommandeCompteur(commandecompteur);


                            }
                        }

                    }
                    if (Settings.outputFile != null)
                    {
                        try
                        {
                            Settings.client.Objects.Save(Settings.outputFile, new GXXmlWriterSettings() { UseMeterTime = true, IgnoreDefaultValues = false });

                        }
                        catch (Exception)
                        {
                            //It's OK if this fails.
                        }
                    }
                }

            }
            catch (GXDLMSException ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (GXDLMSExceptionResponse ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return ex.Message + "500";
            }

            if (reader != null)
            {
                reader.Close();
            }
            if (detailprofilenr == true)
            {
                jsonText = "lecture et enregistrement effectués";

            }
            return jsonText + "200";



            //GXReplyData reply = new GXReplyData();
            //object val2 = reader.Write(Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255"), 2);
            //GXDLMSClock dlms= new GXDLMSClock("0.0.1.0.0.255", 0);
            //GXDLMSClock item = (GXDLMSClock)Settings.client.Objects.FindByLN(ObjectType.Clock, "0.0.1.0.0.255");
            //var essai = item;
            //GXDLMSWrite
            //string dataToWrite = "Nouvelles données à écrire";

            // Créer une demande d'écriture.
            //GXDLMSData data = new GXDLMSData(dataToWrite);
            //GXDLMSObject objToWrite = new GXDLMSObject(0, ObjectType.Clock, 0, 0);
            //GXDLMSClient.WriteResult result = Settings.client.Write(objToWrite, data);

        }

    }
}
