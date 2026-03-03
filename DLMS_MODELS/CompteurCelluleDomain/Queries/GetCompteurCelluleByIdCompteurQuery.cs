using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurCelluleDomain.Queries
{
    public class GetCompteurCelluleByIdCompteurQuery : IRequest<ResponseBase<List<CompteurCelluleResponse>>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } // Added for access checks
        public GetCompteurCelluleByIdCompteurQuery() { }
    }
}
