using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Responses;
using DLMS_MODELS.CompteurDomain.Responses;

namespace DLMS_MODELS.CelluleDomain.Responses
{
    public class CelluleResponse : AuditableEntity
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? ValeurTension { get; set; }
        public string? Libelle { get; set; }
        public string Adresse { get; set; }
        
        // Clé étrangère vers Poste
        public int PosteId { get; set; }
        
        // Optionnel: Information du poste associé
        public PosteResponse? Poste { get; set; }
        
        // Optionnel: Liste des compteurs associés
        public List<CompteurResponse> Compteurs { get; set; } = new List<CompteurResponse>();
    }
}

