using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Responses;

namespace DLMS_MODELS.CompteurCelluleDomain.Responses
{
    public class CompteurCelluleResponse : AuditableEntity
    {
        public int Id { get; set; }
        public int CompteurId { get; set; }
        public int CelluleId { get; set; }
        
        public CompteurResponse Compteur { get; set; }
        public CelluleResponse Cellule { get; set; }
    }
}
