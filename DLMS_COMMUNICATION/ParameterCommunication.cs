using DLMS_COMMUNICATION.Reader;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using Gurux.Net;
using Gurux.Serial;
using System.IO.Ports;
using System.Text;
using DLMS_MODELS;

namespace DLMS_COMMUNICATION
{
    /// <summary>
    /// Configuration des paramètres DLMS avec séparation claire des responsabilités
    /// </summary>
    public class ParameterCommunication
    {
        #region Méthodes de configuration de la connexion IP/Réseau

        /// <summary>
        /// Configure uniquement les paramètres de connexion TCP/IP
        /// </summary>
        public static void ConfigureTcpConnection(ConnexionCommunication setting, string addressIp, string port)
        {
            try
            {
                if (string.IsNullOrEmpty(addressIp))
                {
                    throw new ArgumentException("L'adresse IP est requise pour la connexion TCP");
                }

                if (setting.media == null)
                {
                    setting.media = new GXNet();
                }

                if (setting.media is GXNet net)
                {
                    net.HostName = addressIp;
                    
                    if (!string.IsNullOrEmpty(port) && int.TryParse(port, out int portNumber))
                    {
                        net.Port = portNumber;
                    }
                    else
                    {
                        // Port par défaut DLMS
                        net.Port = 4059;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erreur configuration connexion TCP: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Configure uniquement les paramètres de connexion série
        /// </summary>
        public static void ConfigureSerialConnection(ConnexionCommunication setting, string serialPort, InterfaceType interfaceType)
        {
            try
            {
                if (string.IsNullOrEmpty(serialPort))
                {
                    throw new ArgumentException("Le port série est requis pour la connexion série");
                }

                setting.media = new GXSerial();
                var serial = setting.media as GXSerial;
                string[] tmp = serialPort.Split(':');

                serial.PortName = tmp[0];

                if (tmp.Length > 1)
                {
                    // Configuration personnalisée
                    serial.BaudRate = int.Parse(tmp[1]);
                    serial.DataBits = int.Parse(tmp[2].Substring(0, 1));
                    serial.Parity = (Parity)Enum.Parse(typeof(Parity), tmp[2].Substring(1, tmp[2].Length - 2));
                    serial.StopBits = (StopBits)int.Parse(tmp[2].Substring(tmp[2].Length - 1, 1));
                }
                else
                {
                    // Configuration par défaut selon le type d'interface
                    SetDefaultSerialSettings(serial, interfaceType);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erreur configuration connexion série: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Applique les paramètres par défaut pour le port série
        /// </summary>
        private static void SetDefaultSerialSettings(GXSerial serial, InterfaceType interfaceType)
        {
            if (interfaceType == InterfaceType.HdlcWithModeE)
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

        #endregion

        #region Méthodes de configuration des lectures DLMS

        /// <summary>
        /// Configure uniquement les paramètres de lecture DLMS (authentification, objets)
        /// </summary>
        public static void ConfigureReadingParameters(ConnexionCommunication setting, 
            string clientAddress, string serialNumber, string password, 
            string authenticationKey, string unicastKey, string interfaceType, string objects)
        {
            try
            {
                // Configuration du type d'interface
                ConfigureInterfaceType(setting, interfaceType);

                // Configuration des adresses client et serveur
                ConfigureAddresses(setting, clientAddress, serialNumber);

                // Configuration de l'authentification
                ConfigureAuthentication(setting, password, authenticationKey, unicastKey);

                // Configuration des objets à lire
                ConfigureReadObjects(setting, objects);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erreur configuration paramètres de lecture: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Configure le type d'interface DLMS
        /// </summary>
        public static void ConfigureInterfaceType(ConnexionCommunication setting, string interfaceType)
        {
            if (!string.IsNullOrEmpty(interfaceType))
            {
                try
                {
                    setting.client.InterfaceType = (InterfaceType)Enum.Parse(typeof(InterfaceType), interfaceType);
                    setting.client.Plc.Reset();

                    // Configuration spéciale pour CoAP
                    if (setting.client.InterfaceType == InterfaceType.CoAP && setting.media is GXNet net)
                    {
                        net.Protocol = NetworkType.Udp;
                    }

                    // Ajustement des paramètres série si nécessaire
                    if (setting.media is GXSerial serial && setting.client.InterfaceType == InterfaceType.HdlcWithModeE)
                    {
                        SetDefaultSerialSettings(serial, InterfaceType.HdlcWithModeE);
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException("Type d'interface invalide. Valeurs valides: HDLC, WRAPPER, HdlcWithModeE, Plc, PlcHdlc");
                }
            }
        }

        /// <summary>
        /// Configure les adresses client et serveur
        /// </summary>
        private static void ConfigureAddresses(ConnexionCommunication setting, string clientAddress, string serialNumber)
        {
            // Configuration adresse client
            if (!string.IsNullOrEmpty(clientAddress))
            {
                switch (clientAddress.ToLower())
                {
                    case "public":
                        setting.client.ClientAddress = 16;
                        setting.outputFile = "public.xml";
                        break;
                    case "read":
                        setting.client.ClientAddress = 2;
                        setting.client.Authentication = Authentication.HighGMAC;
                        setting.client.Ciphering.Security = Security.AuthenticationEncryption;
                        setting.outputFile = "Read.xml";
                        break;
                    case "managed":
                        setting.client.ClientAddress = 1;
                        setting.client.Authentication = Authentication.HighGMAC;
                        setting.client.Ciphering.Security = Security.AuthenticationEncryption;
                        setting.outputFile = "Managed.xml";
                        break;
                    case "fwu":
                        setting.client.ClientAddress = 3;
                        setting.client.Authentication = Authentication.HighGMAC;
                        setting.client.Ciphering.Security = Security.AuthenticationEncryption;
                        setting.outputFile = "fwu.xml";
                        break;
                    default:
                        if (int.TryParse(clientAddress, out int address))
                        {
                            setting.client.ClientAddress = address;
                        }
                        else
                        {
                            throw new ArgumentException($"Adresse client invalide: {clientAddress}");
                        }
                        break;
                }
            }

            // Configuration adresse serveur (numéro de série)
            if (!string.IsNullOrEmpty(serialNumber))
            {
                if (int.TryParse(serialNumber, out int serialNum))
                {
                    if (serialNum != 1)
                    {
                        var physicalAddress = int.Parse(serialNumber.Substring(serialNumber.Length - 4)) + 100;
                        setting.client.ServerAddress = GXDLMSClient.GetServerAddress(1, physicalAddress);
                    }
                    else
                    {
                        setting.client.ServerAddress = serialNum;
                    }
                }
                else
                {
                    throw new ArgumentException($"Numéro de série invalide: {serialNumber}");
                }
            }
        }

        /// <summary>
        /// Configure l'authentification et les clés de sécurité
        /// </summary>
        private static void ConfigureAuthentication(ConnexionCommunication setting, string password, 
            string authenticationKey, string unicastKey)
        {
            // Configuration du mot de passe
            if (!string.IsNullOrEmpty(password))
            {
                if (password.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    setting.client.Password = GXCommon.HexToBytes(password.Substring(2));
                }
                else
                {
                    setting.client.Password = ASCIIEncoding.ASCII.GetBytes(password);
                }
            }

            // Configuration de la clé d'authentification
            if (!string.IsNullOrEmpty(authenticationKey))
            {
                setting.client.Ciphering.AuthenticationKey = GXCommon.HexToBytes(authenticationKey);
            }

            // Configuration de la clé unicast
            if (!string.IsNullOrEmpty(unicastKey))
            {
                setting.client.Ciphering.BlockCipherKey = GXCommon.HexToBytes(unicastKey);
            }
        }

        /// <summary>
        /// Configure les objets à lire
        /// </summary>
        private static void ConfigureReadObjects(ConnexionCommunication setting, string objects)
        {
            if (!string.IsNullOrEmpty(objects))
            {
                string[] tmp;
                foreach (string obj in objects.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    tmp = obj.Split(new char[] { ':' });
                    if (tmp.Length == 2)
                    {
                        var logicalName = tmp[0].Trim();
                        if (int.TryParse(tmp[1].Trim(), out int attributeIndex))
                        {
                            setting.readObjects.Add(new KeyValuePair<string, int>(logicalName, attributeIndex));
                        }
                        else
                        {
                            throw new ArgumentException($"Index d'attribut invalide pour l'objet {logicalName}: {tmp[1]}");
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Format d'objet invalide: {obj}");
                    }
                }
            }
        }

        #endregion

        #region Méthodes de combinaison (compatibilité)

        /// <summary>
        /// Configure les paramètres complets (méthode legacy pour compatibilité)
        /// </summary>
        public static void GetParameters(ConnexionCommunication setting, string port, string serialport, 
            string AddressIp, string ClientAddress, string SerialNumber, string interfaceType, 
            string password, string AuthenticationKey, string UnicastKey, string? Objects)
        {
            try
            {
                // 1. Configuration de la connexion (TCP/IP ou série)
                if (!string.IsNullOrEmpty(AddressIp))
                {
                    ConfigureTcpConnection(setting, AddressIp, port);
                }
                else if (!string.IsNullOrEmpty(serialport))
                {
                    ConfigureSerialConnection(setting, serialport, 
                        string.IsNullOrEmpty(interfaceType) ? InterfaceType.HDLC : 
                        (InterfaceType)Enum.Parse(typeof(InterfaceType), interfaceType));
                }

                // 2. Configuration des paramètres de lecture DLMS
                ConfigureReadingParameters(setting, ClientAddress, SerialNumber, password, 
                    AuthenticationKey, UnicastKey, interfaceType, Objects ?? "");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erreur configuration paramètres DLMS: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Configure une communication complète (version moderne)
        /// </summary>
        public static ConnexionCommunication ConfigureCompleteCommunication(
            string addressIp, string port, string serialPort,
            string clientAddress = "read", string serialNumber = "",
            string password = "", string authenticationKey = "", string unicastKey = "",
            string interfaceType = "HDLC", string objects = "")
        {
            var setting = new ConnexionCommunication();

            // Configuration de la connexion
            if (!string.IsNullOrEmpty(addressIp))
            {
                ConfigureTcpConnection(setting, addressIp, port);
            }
            else if (!string.IsNullOrEmpty(serialPort))
            {
                ConfigureSerialConnection(setting, serialPort, 
                    (InterfaceType)Enum.Parse(typeof(InterfaceType), interfaceType));
            }

            // Configuration des paramètres de lecture
            ConfigureReadingParameters(setting, clientAddress, serialNumber, password, 
                authenticationKey, unicastKey, interfaceType, objects);

            return setting;
        }

        #endregion

        #region Méthodes utilitaires

        /// <summary>
        /// Parse la chaîne d'objets DLMS
        /// </summary>
        public static List<KeyValuePair<string, int>> ParseReadObjects(string objectsString)
        {
            var readObjects = new List<KeyValuePair<string, int>>();

            if (string.IsNullOrEmpty(objectsString)) return readObjects;

            try
            {
                foreach (string obj in objectsString.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var tmp = obj.Split(new char[] { ':' });
                    if (tmp.Length == 2)
                    {
                        var logicalName = tmp[0].Trim();
                        if (int.TryParse(tmp[1].Trim(), out int attributeIndex))
                        {
                            readObjects.Add(new KeyValuePair<string, int>(logicalName, attributeIndex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Format des objets invalide: {objectsString}", ex);
            }

            return readObjects;
        }

        /// <summary>
        /// Applique les paramètres DLMS à un client et média existants
        /// </summary>
        public static void ApplyParameters(GXDLMSSecureClient client, IGXMedia media, DLMSConnectionParameters parameters)
        {
            try
            {
                // Apply interface type
                if (!string.IsNullOrEmpty(parameters.InterfaceType))
                {
                    client.InterfaceType = (InterfaceType)Enum.Parse(typeof(InterfaceType), parameters.InterfaceType);
                }

                // Apply client address
                if (!string.IsNullOrEmpty(parameters.ClientAddress))
                {
                    if (parameters.ClientAddress == "public")
                    {
                        client.ClientAddress = 16;
                    }
                    else if (parameters.ClientAddress == "read")
                    {
                        client.ClientAddress = 2;
                        client.Authentication = Authentication.HighGMAC;
                        client.Ciphering.Security = Security.AuthenticationEncryption;
                    }
                    else if (parameters.ClientAddress == "managed")
                    {
                        client.ClientAddress = 1;
                        client.Authentication = Authentication.HighGMAC;
                        client.Ciphering.Security = Security.AuthenticationEncryption;
                    }
                    else if (parameters.ClientAddress == "fwu")
                    {
                        client.ClientAddress = 3;
                        client.Authentication = Authentication.HighGMAC;
                        client.Ciphering.Security = Security.AuthenticationEncryption;
                    }
                    else
                    {
                        client.ClientAddress = int.Parse(parameters.ClientAddress);
                    }
                }

                // Apply server address from serial number
                if (!string.IsNullOrEmpty(parameters.SerialNumber))
                {
                    if (int.Parse(parameters.SerialNumber) != 1)
                    {
                        var PhysicalAddress = int.Parse(parameters.SerialNumber.Substring(parameters.SerialNumber.Length - 4)) + 100;
                        client.ServerAddress = GXDLMSClient.GetServerAddress(1, PhysicalAddress);
                    }
                    else
                    {
                        client.ServerAddress = int.Parse(parameters.SerialNumber);
                    }
                }

                // Apply password
                if (!string.IsNullOrEmpty(parameters.Password))
                {
                    if (parameters.Password.StartsWith("0x"))
                    {
                        client.Password = GXCommon.HexToBytes(parameters.Password.Substring(2));
                    }
                    else
                    {
                        client.Password = ASCIIEncoding.ASCII.GetBytes(parameters.Password);
                    }
                }

                // Apply authentication key
                if (!string.IsNullOrEmpty(parameters.AuthenticationKey))
                {
                    client.Ciphering.AuthenticationKey = GXCommon.HexToBytes(parameters.AuthenticationKey);
                }

                // Apply unicast key
                if (!string.IsNullOrEmpty(parameters.UnicastKey))
                {
                    client.Ciphering.BlockCipherKey = GXCommon.HexToBytes(parameters.UnicastKey);
                }

                // Configure media based on interface type
                if (media is GXNet net && !string.IsNullOrEmpty(parameters.AddressIp))
                {
                    net.HostName = parameters.AddressIp;
                    if (!string.IsNullOrEmpty(parameters.Port))
                    {
                        net.Port = int.Parse(parameters.Port);
                    }
                }

                if (media is GXSerial serial && !string.IsNullOrEmpty(parameters.SerialPort))
                {
                    string[] tmp = parameters.SerialPort.Split(':');
                    serial.PortName = tmp[0];
                    if (tmp.Length > 1)
                    {
                        serial.BaudRate = int.Parse(tmp[1]);
                        serial.DataBits = int.Parse(tmp[2].Substring(0, 1));
                        serial.Parity = (Parity)Enum.Parse(typeof(Parity), tmp[2].Substring(1, tmp[2].Length - 2));
                        serial.StopBits = (StopBits)int.Parse(tmp[2].Substring(tmp[2].Length - 1, 1));
                    }
                    else
                    {
                        if (client.InterfaceType == InterfaceType.HdlcWithModeE)
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
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error applying DLMS parameters: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Configure uniquement le media (transport)
        /// </summary>
        public static void ConfigureMedia(IGXMedia media, DLMSConnectionParameters parameters)
        {
            try
            {
                if (media == null)
                    throw new ArgumentNullException(nameof(media));

                if (media is GXNet net && !string.IsNullOrEmpty(parameters.AddressIp))
                {
                    net.HostName = parameters.AddressIp;
                    if (!string.IsNullOrEmpty(parameters.Port) && int.TryParse(parameters.Port, out int portNumber))
                    {
                        net.Port = portNumber;
                    }
                    else
                    {
                        net.Port = 4059; // Port par défaut DLMS
                    }
                    
                   
                }
                else if (media is GXSerial serial && !string.IsNullOrEmpty(parameters.SerialPort))
                {
                    string[] tmp = parameters.SerialPort.Split(':');
                    serial.PortName = tmp[0];
                    
                    if (tmp.Length > 1)
                    {
                        serial.BaudRate = int.Parse(tmp[1]);
                        serial.DataBits = int.Parse(tmp[2].Substring(0, 1));
                        serial.Parity = (Parity)Enum.Parse(typeof(Parity), tmp[2].Substring(1, tmp[2].Length - 2));
                        serial.StopBits = (StopBits)int.Parse(tmp[2].Substring(tmp[2].Length - 1, 1));
                    }
                    else
                    {
                        // Configuration par défaut selon le type d'interface
                        var interfaceType = string.IsNullOrEmpty(parameters.InterfaceType) 
                            ? InterfaceType.HDLC 
                            : (InterfaceType)Enum.Parse(typeof(InterfaceType), parameters.InterfaceType, true);
                        
                        if (interfaceType == InterfaceType.HdlcWithModeE)
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
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erreur configuration media: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Configure uniquement le client DLMS (association par compteur)
        /// </summary>
        public static void ConfigureClient(GXDLMSSecureClient client, DLMSConnectionParameters parameters)
        {
            try
            {
                if (client == null)
                    throw new ArgumentNullException(nameof(client));

                // Configuration du type d'interface
                if (!string.IsNullOrEmpty(parameters.InterfaceType))
                {
                    client.InterfaceType = (InterfaceType)Enum.Parse(typeof(InterfaceType), parameters.InterfaceType, true);
                    client.Plc.Reset();

                    // Configuration spéciale pour CoAP
                    if (client.InterfaceType == InterfaceType.CoAP)
                    {
                        // Note: Le media doit être de type GXNet pour CoAP
                        // Cette configuration doit être faite au niveau du media
                    }
                }

                // Configuration adresse client
                if (!string.IsNullOrEmpty(parameters.ClientAddress))
                {
                    switch (parameters.ClientAddress.ToLower())
                    {
                        case "public":
                            client.ClientAddress = 16;
                            break;
                        case "read":
                            client.ClientAddress = 2;
                            client.Authentication = Authentication.HighGMAC;
                            client.Ciphering.Security = Security.AuthenticationEncryption;
                            break;
                        case "managed":
                            client.ClientAddress = 1;
                            client.Authentication = Authentication.HighGMAC;
                            client.Ciphering.Security = Security.AuthenticationEncryption;
                            break;
                        case "fwu":
                            client.ClientAddress = 3;
                            client.Authentication = Authentication.HighGMAC;
                            client.Ciphering.Security = Security.AuthenticationEncryption;
                            break;
                        default:
                            if (int.TryParse(parameters.ClientAddress, out int address))
                            {
                                client.ClientAddress = address;
                            }
                            else
                            {
                                throw new ArgumentException($"Adresse client invalide: {parameters.ClientAddress}");
                            }
                            break;
                    }
                }

                // Configuration adresse serveur (numéro de série)
                if (!string.IsNullOrEmpty(parameters.SerialNumber))
                {
                    if (int.TryParse(parameters.SerialNumber, out int serialNum))
                    {
                        if (serialNum != 1)
                        {
                            var physicalAddress = int.Parse(parameters.SerialNumber.Substring(parameters.SerialNumber.Length - 4)) + 100;
                            client.ServerAddress = GXDLMSClient.GetServerAddress(1, physicalAddress);
                        }
                        else
                        {
                            client.ServerAddress = serialNum;
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Numéro de série invalide: {parameters.SerialNumber}");
                    }
                }

                // Configuration authentification
                if (!string.IsNullOrEmpty(parameters.Password))
                {
                    if (parameters.Password.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    {
                        client.Password = GXCommon.HexToBytes(parameters.Password.Substring(2));
                    }
                    else
                    {
                        client.Password = ASCIIEncoding.ASCII.GetBytes(parameters.Password);
                    }
                }

                // Configuration clé d'authentification
                if (!string.IsNullOrEmpty(parameters.AuthenticationKey))
                {
                    client.Ciphering.AuthenticationKey = GXCommon.HexToBytes(parameters.AuthenticationKey);
                }

                // Configuration clé unicast
                if (!string.IsNullOrEmpty(parameters.UnicastKey))
                {
                    client.Ciphering.BlockCipherKey = GXCommon.HexToBytes(parameters.UnicastKey);
                }

                // GBT (General Block Transfer) — propose le flag au compteur
                if (parameters.UseGbt)
                {
                    client.ProposedConformance |= Conformance.GeneralBlockTransfer;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Erreur configuration client DLMS: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
