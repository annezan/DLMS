using System;
using System.Threading.Tasks;

namespace DLMS_MODELS.ServiceContracts
{
    public enum DlmsCommandType { ReadByRange, ReadByEntry, GetClock }

    public class CommandRequest
    {
        public DlmsCommandType Type { get; set; }
        public string AddressIp { get; set; } = "";
        public string? Port { get; set; }
        public string SerialNumber { get; set; } = "";
        public string Password { get; set; } = "";
        public string AuthenticationKey { get; set; } = "";
        public string UnicastKey { get; set; } = "";
        public string ProfileObis { get; set; } = "1.0.99.1.0.255";
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int NombreEntree { get; set; }
        public int? CommandeCompteurId { get; set; }
    }

    public class CommandResult
    {
        public bool Success { get; set; }
        public string Data { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public int RowCount { get; set; }
        public TimeSpan Duration { get; set; }
    }

    /// <summary>
    /// Interface pour l'exécution de commandes DLMS à la demande.
    /// Implémentée dans DLMS_SERVICE, consommée par le DAL.
    /// </summary>
    public interface ICommandExecutor
    {
        Task<CommandResult> ExecuteCommandAsync(CommandRequest request);
    }
}
