using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.CompteurEquipementDomain.Responses;

namespace DLMS_MODELS.CompteurEquipementDomain.Queries
{
    public class GetCompteurEquipementByIdCompteurQuery : IRequest<ResponseBase<List<CompteurEquipementResponse>>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } // Added for access checks
        
        public GetCompteurEquipementByIdCompteurQuery() { }
        public GetCompteurEquipementByIdCompteurQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
