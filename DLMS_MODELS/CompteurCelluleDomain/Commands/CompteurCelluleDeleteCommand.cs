using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurCelluleDomain.Commands
{   
    public class CompteurCelluleDeleteCommand : IRequest<ResponseBase<CompteurCelluleResponse>>
    {
        public int CompteurId { get; set; }
        public int CelluleId { get; set; }
        public Guid UserId { get; set; } // Added for access checks
        public string DeletedBy { get; set; }
    }
}
