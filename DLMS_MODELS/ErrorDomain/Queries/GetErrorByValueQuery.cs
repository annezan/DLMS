using DLMS_MODELS.Bases;
using DLMS_MODELS.ErrorDomain.Responses;
using MediatR;

namespace DLMS_MODELS.ErrorDomain.Queries
{
    public class GetErrorByValueQuery : IRequest<ResponseBase<ErrorResponse>>
    {
        public int Value { get; set; }

        public GetErrorByValueQuery() { }
        public GetErrorByValueQuery(int Value)
        {
            this.Value = Value;
        }
    }
}
