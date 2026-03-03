using DLMS_MODELS.Bases;
using DLMS_MODELS.ErrorDomain.Responses;
using MediatR;

namespace DLMS_MODELS.ErrorDomain.Queries
{
    public class GetErrorQuery : IRequest<ResponseBase<List<ErrorResponse>>>
    {
        public GetErrorQuery()
        {
        }
    }
}
