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
using DLMS_COMMUNICATION.Reader;

namespace DLMS_COMMUNICATION
{
    public class ConnexionCommunication
    {
        public IGXMedia media = null;
        public TraceLevel trace = TraceLevel.Verbose;
        public GXDLMSSecureClient client = new GXDLMSSecureClient(true);
        public string invocationCounter = null;

        // Collections d'instance (plus de static)
        public List<KeyValuePair<string, int>> readObjects = new List<KeyValuePair<string, int>>();
        public List<KeyValuePair<string, object>> resultObjects = new List<KeyValuePair<string, object>>();
        public List<KeyValuePair<object[], GXDLMSObject[]>> entries = new List<KeyValuePair<object[], GXDLMSObject[]>>();
        public List<KeyValuePair<object[], object[]>> entries2 = new List<KeyValuePair<object[], object[]>>();

        public List<KeyValuePair<string, object>> Unitscalers = new List<KeyValuePair<string, object>>();
        public List<Dictionary<string, string?>> queryParametersList = new List<Dictionary<string, string?>>();

        //Cache file.
        public string outputFile = null;

        // Méthode d'instance pour la connexion (plus de static)
        public async Task<string> ConnectAsync(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            GXDLMSObjectCollection Result = new GXDLMSObjectCollection();
            object ResultObis = null;
            int it_value = 0;
            string jsonText = "";

            try
            {
                ////////////////////////////////////////
                //Handle command line parameters.
                ParameterCommunication.GetParameters(this, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);

                ////////////////////////////////////////
                //Initialize connection settings.
                if (this.media is GXSerial)
                {
                }
                else if (this.media is GXNet)
                {
                }
                else
                {
                    throw new Exception("Unknown media type.");
                }

                ////////////////////////////////////////
                Gurux.DLMS.Reader.GXDLMSReader reader = null;
                try
                {
                    reader = new Gurux.DLMS.Reader.GXDLMSReader(this.client, this.media, this.trace, this.invocationCounter);
                    reader.OnNotification += (data) =>
                    {
                        var essai1 = data;
                        Console.WriteLine(data);
                    };

                    //Create manufacturer spesific custom COSEM object.
                    this.client.OnCustomObject += (type, version) =>
                    {
                        /*
                        if (type == 6001 && version == 0)
                        {
                            return new ManufacturerSpesificObject();
                        }
                        */
                        return null;
                    };

                    this.media.Open();
                    reader.InitializeConnection();

                    // La connexion a réussi, on continue
                    return "Connexion réussie";
                }
                catch (System.IO.IOException ex)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    return ex.ToString();
                }
                catch (GXDLMSException ex)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    return ex.ToString();
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    return ex.ToString();
                }
                catch (GXDLMSConfirmedServiceError ex)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    return ex.ToString();
                }
                catch (Exception ex)
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    return ex.ToString();
                }
            }
            catch (Exception ex)
            {
                // Gestion des exceptions du premier try (paramétrage)
                return ex.ToString();
            }
        }

        // Méthode statique conservée pour compatibilité (à remplacer progressivement)
        public static string Connexion(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? password, string? AuthenticationKey, string? UnicastKey, string? Objects)
        {
            // CRÉATION D'UNE INSTANCE POUR MAINTENIR LA COMPATIBILITÉ
            var instance = new ConnexionCommunication();

            // Utilisation de la version asynchrone en synchrone pour compatibilité
            var task = instance.ConnectAsync(port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, password, AuthenticationKey, UnicastKey, Objects);
            task.Wait(); // Attendre la completion (à éviter dans le nouveau code)

            return task.Result;
        }
    }
}
