using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CodeObisDomain.Queries
{
    public class GetCodeObisQuery : IRequest<ResponseBase<List<CodeObisResponse>>>
    {
        public GetCodeObisQuery()
        {
        }
    }
}
