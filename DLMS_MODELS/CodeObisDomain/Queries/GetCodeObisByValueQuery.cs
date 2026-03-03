using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CodeObisDomain.Queries
{
    public class GetCodeObisByValueQuery : IRequest<ResponseBase<CodeObisResponse>>
    {
        public string Value { get; set; }

        public GetCodeObisByValueQuery() { }
        public GetCodeObisByValueQuery(string Value)
        {
            this.Value = Value;
        }
    }
}
