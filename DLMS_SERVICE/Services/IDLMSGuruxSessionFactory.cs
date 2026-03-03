using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Secure;
using Gurux.Net;
using Gurux.Serial;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using DLMS_COMMUNICATION.Reader;
using DLMS_COMMUNICATION;
using DLMS_MODELS;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSGuruxSessionFactory : IDisposable
    {
        DLMSGuruxSession CreateSession(DLMSConnectionParameters parameters);
    }

    public class DLMSGuruxSession : IDLMSCommunicationSession, IDisposable
    {
        public IGXMedia Media { get; private set; }
        public GXDLMSSecureClient? Client { get; set; }
        public Gurux.DLMS.Reader.GXDLMSReader? Reader { get; private set; }
        public DLMSConnectionParameters Parameters { get; private set; }
        public List<KeyValuePair<string, int>> ReadObjects { get; private set; }
        public List<KeyValuePair<object[], object[]>> Entries { get; private set; }
        public string? OutputFile { get; private set; }
        public bool IsConnected { get; set; } = false;
        public TraceLevel Trace { get; set; }
        public string? InvocationCounter { get; set; }
        public bool AssociationLoaded { get; set; } = false;

        private readonly ILogger<DLMSGuruxSession> _logger;
        private bool _disposed = false;

        // Constructeur modifié pour accepter media déjà configuré
        public DLMSGuruxSession(
            IGXMedia media,
            DLMSConnectionParameters parameters,
            ILogger<DLMSGuruxSession> logger)
        {
            Media = media ?? throw new ArgumentNullException(nameof(media));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            ReadObjects = new List<KeyValuePair<string, int>>();
            Entries = new List<KeyValuePair<object[], object[]>>();
            
            // Définir OutputFile par défaut si non spécifié
            OutputFile = !string.IsNullOrEmpty(parameters.OutputFile) 
                ? parameters.OutputFile 
                : (parameters.ClientAddress?.ToLower() switch
                {
                    "public" => "public.xml",
                    "read" => "Read.xml", 
                    "managed" => "Managed.xml",
                    "fwu" => "fwu.xml",
                    _ => "Read.xml"
                });
                
            Trace = parameters.Trace;
            InvocationCounter = parameters.InvocationCounter;
        }

        // Constructeur legacy pour compatibilité
        public DLMSGuruxSession(
            IGXMedia media,
            GXDLMSSecureClient client,
            Gurux.DLMS.Reader.GXDLMSReader? reader,
            DLMSConnectionParameters parameters,
            ILogger<DLMSGuruxSession> logger) : this(media, parameters, logger)
        {
            Client = client;
            Reader = reader;
        }

        // 🔥 Ouvre UNIQUEMENT le transport TCP vers UMAD
        public async Task<bool> OpenTransportAsync(CancellationToken ct)
        {
            try
            {
                if (Media == null)
                {
                    _logger.LogError("❌ Media null pour OpenTransportAsync");
                    return false;
                }

                // Configurer le media avec les paramètres de transport
                ParameterCommunication.ConfigureMedia(Media, Parameters);

                // 🔴 GESTION DES ERREURS DE CONNEXION SANS BLOCAGE
                var connectionSuccess = false;
                try
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            if (!Media.IsOpen)
                                Media.Open();
                            connectionSuccess = true;
                        }
                        catch (System.Net.Sockets.SocketException ex)
                        {
                            _logger.LogError(ex, "❌ Erreur de connexion réseau à {Ip}:{Port} - Équipement inaccessible ou timeout", 
                                Parameters.AddressIp, Parameters.Port);
                            // Ne pas bloquer - retourner directement
                            return;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "❌ Erreur inattendue lors de l'ouverture du transport vers {Ip}:{Port}", 
                                Parameters.AddressIp, Parameters.Port);
                            // Ne pas bloquer - retourner directement
                            return;
                        }
                    }, ct);

                    if (connectionSuccess)
                    {
                        _logger.LogInformation("✅ Transport ouvert vers {Ip}:{Port}", 
                            Parameters.AddressIp, Parameters.Port);
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Impossible d'ouvrir le transport vers {Ip}:{Port} - passage à l'IP suivante", 
                            Parameters.AddressIp, Parameters.Port);
                        return false;
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    _logger.LogError(ex, "❌ Erreur de connexion réseau à {Ip}:{Port} - Équipement inaccessible", 
                        Parameters.AddressIp, Parameters.Port);
                    // Ne pas bloquer - retourner false pour continuer avec les autres IPs
                    return false;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("⏰ Connexion annulée pour {Ip}:{Port}", 
                        Parameters.AddressIp, Parameters.Port);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Erreur inattendue lors de la connexion à {Ip}:{Port}", 
                        Parameters.AddressIp, Parameters.Port);
                    // Ne pas bloquer - retourner false pour continuer
                    return false;
                }

                IsConnected = true;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Échec ouverture transport UMAD");
                return false;
            }
        }

        // 🔥 Association DLMS PAR COMPTEUR (HDLC)
        public void InitializeMeterClient(DLMSConnectionParameters meterParams)
        {
            try
            {
                // Créer un nouveau client pour ce compteur
                Client = new GXDLMSSecureClient(true);

                // Configurer le client avec les paramètres du compteur
                ParameterCommunication.ConfigureClient(Client, meterParams);

                // Créer le reader pour ce compteur
                Reader = new Gurux.DLMS.Reader.GXDLMSReader(
                    Client, 
                    Media, 
                    Trace, 
                    InvocationCounter);

                AssociationLoaded = false;

                _logger.LogDebug("✅ Client DLMS initialisé pour {Serial}", meterParams.SerialNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erreur initialisation client DLMS");
                throw;
            }
        }


        public async Task DisconnectAsync()
        {
            try
            {
                if (Reader != null)
                {
                    Reader.Close();
                    _logger.LogDebug("Reader DLMS fermé pour IP: {IP}", Parameters.AddressIp);
                }

                if (Media != null)
                {
                    Media.Close();
                    _logger.LogDebug("Media DLMS fermé pour IP: {IP}", Parameters.AddressIp);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la fermeture de session DLMS pour IP: {IP}", Parameters.AddressIp);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                DisconnectAsync().GetAwaiter().GetResult();
                _disposed = true;
            }
        }
    }

    public class DLMSGuruxSessionFactory : IDLMSGuruxSessionFactory
    {
        private readonly ILogger<DLMSGuruxSessionFactory> _logger;
        private readonly ILogger<DLMSGuruxSession> _sessionLogger;
        private bool _disposed = false;

        public DLMSGuruxSessionFactory(
            ILogger<DLMSGuruxSessionFactory> logger,
            ILogger<DLMSGuruxSession> sessionLogger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionLogger = sessionLogger ?? throw new ArgumentNullException(nameof(sessionLogger));
        }

        public DLMSGuruxSession CreateSession(DLMSConnectionParameters parameters)
        {
            try
            {
                _logger.LogDebug("Création d'une nouvelle session DLMS pour IP: {IP}", parameters.AddressIp);

                // Création du média (sera configuré dans ConnectAsync)
                IGXMedia media = CreateMedia(parameters);
                
                // Création du client sécurisé (sera configuré dans ConnectAsync)
                var client = new GXDLMSSecureClient(true);
                
                // Le Reader sera créé dans ConnectAsync après configuration des objets
                // Pas de création ici pour éviter les objets non configurés

                var session = new DLMSGuruxSession(media, client, null, parameters, _sessionLogger);
                
                _logger.LogDebug("Session DLMS créée avec succès pour IP: {IP}", parameters.AddressIp);
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de session DLMS pour IP: {IP}", parameters.AddressIp);
                throw;
            }
        }

        private IGXMedia CreateMedia(DLMSConnectionParameters parameters)
        {
            // Priorité à l'adresse IP pour les connexions réseau
            if (!string.IsNullOrEmpty(parameters.AddressIp))
            {
                _logger.LogDebug("Création d'un média réseau (GXNet) pour l'IP: {IP}", parameters.AddressIp);
                return new GXNet();
            }
            
            // Sinon, utilisation du port série pour les connexions série
            if (!string.IsNullOrEmpty(parameters.SerialPort))
            {
                _logger.LogDebug("Création d'un média série (GXSerial) pour le port: {Port}", parameters.SerialPort);
                return new GXSerial();
            }
            
            // Si aucun des deux n'est spécifié, erreur
            throw new ArgumentException($"Aucun média de connexion spécifié. AddressIp et SerialPort sont tous les deux vides.");
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _logger.LogDebug("Factory DLMSGuruxSessionFactory disposée");
                _disposed = true;
            }
        }
    }
}
