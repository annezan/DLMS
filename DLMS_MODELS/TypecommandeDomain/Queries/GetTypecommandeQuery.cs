using DLMS_MODELS.Bases;
using DLMS_MODELS.TypecommandeDomain.Responses;
using MediatR;

namespace DLMS_MODELS.TypecommandeDomain.Queries
{
    public class GetTypecommandeQuery : IRequest<ResponseBase<List<TypecommandeResponse>>>
    {
        public GetTypecommandeQuery()
        {
        }
    }
}
