using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.PosteDomain.Responses;

namespace DLMS_MODELS.PosteDomain.Queries
{
    public class GetPosteQuery : IRequest<ResponseBase<List<PosteResponse>>>
    {
        public GetPosteQuery()
        {
        }
    }
}
