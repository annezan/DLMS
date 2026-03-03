using System;

namespace DLMS_MODELS
{
    /// <summary>
    /// Informations sur une lecture manquante à rattraper
    /// </summary>
    public class MissingReadInfo
    {
        public int CompteurId { get; set; }
        public string NumeroCompteur { get; set; } = string.Empty;
        public string AdresseIp { get; set; } = string.Empty;
        public string? Port { get; set; } = string.Empty;
        public string? SerialPort { get; set; } = string.Empty;
        public DateTime MissingHour { get; set; }
        public string ClientAddress { get; set; } = string.Empty;
    }

    /// <summary>
    /// Informations sur une commande active à exécuter
    /// </summary>
    public class ActiveCommandInfo
    {
        public int CommandId { get; set; }
        public int CompteurId { get; set; }
        public int CommandeCompteurId { get; set; }
        public string NumeroCompteur { get; set; } = string.Empty;
        public string AdresseIp { get; set; } = string.Empty;
        public string? Port { get; set; } = string.Empty;
        public string? SerialPort { get; set; } = string.Empty;
        public string CommandType { get; set; } = string.Empty;
        public int? Numeroprofile { get; set; }
        public int? Nombreentree { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<DateTime> Heures { get; set; }
        public int RetryCount { get; set; }
    }

    /// <summary>
    /// Résultat du traitement d'un compteur
    /// </summary>
    public class CompteurProcessingResult
    {
        public bool Success { get; set; }
        public int CompteurId { get; set; }
        public string NumeroCompteur { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public System.Collections.Generic.Dictionary<string, bool> OperationResults { get; set; } = new();
    }
}
