using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CodeObisDomain.Queries
{
    public class GetCodeObisByIdQuery : IRequest<ResponseBase<CodeObisResponse>>
    {
        public int Id { get; set; }
        public GetCodeObisByIdQuery() { }
        public GetCodeObisByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
