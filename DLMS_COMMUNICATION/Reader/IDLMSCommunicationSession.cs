using DLMS_MODELS;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using Gurux.DLMS.Reader;
using System.Diagnostics;
using Task = System.Threading.Tasks.Task;

namespace DLMS_COMMUNICATION.Reader
{
    /// <summary>
    /// Interface pour une session DLMS thread-safe respectant les exigences du prompt :
    /// - Factory pour sessions Gurux thread-safe
    /// - Une session par meter IP
    /// - Chaque read a ses propres client/media/reader instances
    /// - Proper session closing
    /// - No static Gurux objects
    /// </summary>
    public interface IDLMSCommunicationSession : IDisposable
    {
        // Instances Gurux propres à chaque session (exigence prompt)
        IGXMedia Media { get; }
        GXDLMSSecureClient Client { get; }
        GXDLMSReader? Reader { get; }

        // Configuration de la session
        DLMSConnectionParameters Parameters { get; }
        TraceLevel Trace { get; }
        string? InvocationCounter { get; }
        string? OutputFile { get; }
        bool IsConnected { get; }
        bool AssociationLoaded { get; set; }
        bool ScalersLoaded { get; set; }

        // Collections pour les lectures (évite les static)
        List<KeyValuePair<string, int>> ReadObjects { get; }
        List<KeyValuePair<object[], object[]>> Entries { get; }

        // Méthodes de cycle de vie (proper session closing)
        Task<bool> OpenTransportAsync(CancellationToken ct);
        void InitializeMeterClient(DLMSConnectionParameters meterParams, int? waitTime = null, int? retryCount = null);
        Task DisconnectAsync();
    }
}
