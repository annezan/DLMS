using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.FabricantDomain.Responses;

namespace DLMS_MODELS.FabricantDomain.Queries
{
    public class GetFabricantQuery : IRequest<ResponseBase<List<FabricantResponse>>>
    {
        public GetFabricantQuery()
        {
        }
    }
}
