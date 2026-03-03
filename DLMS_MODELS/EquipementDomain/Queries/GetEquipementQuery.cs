using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.EquipementDomain.Responses;

namespace DLMS_MODELS.EquipementDomain.Queries
{
    public class GetEquipementQuery : IRequest<ResponseBase<List<EquipementResponse>>>
    {
        public GetEquipementQuery()
        {
        }
    }
}
