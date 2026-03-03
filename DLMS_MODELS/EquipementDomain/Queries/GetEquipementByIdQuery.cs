using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.EquipementDomain.Responses;

namespace DLMS_MODELS.EquipementDomain.Queries
{
    public class GetEquipementByIdQuery : IRequest<ResponseBase<EquipementResponse>>
    {
        public int Id { get; set; }
        public GetEquipementByIdQuery() { }
        public GetEquipementByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
