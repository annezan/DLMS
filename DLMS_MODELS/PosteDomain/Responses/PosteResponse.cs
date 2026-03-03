using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Responses;
using System.Collections.Generic;

namespace DLMS_MODELS.PosteDomain.Responses
{
    public class PosteResponse : AuditableEntity
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Libelle { get; set; }
        public string Adresse { get; set; }
        
        // Collection de cellules associées
        public ICollection<CelluleResponse> Cellules { get; set; } = new List<CelluleResponse>();
    }
}
