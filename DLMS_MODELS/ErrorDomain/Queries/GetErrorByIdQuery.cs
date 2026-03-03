using DLMS_MODELS.Bases;
using DLMS_MODELS.ErrorDomain.Responses;
using MediatR;

namespace DLMS_MODELS.ErrorDomain.Queries
{
    public class GetErrorByIdQuery : IRequest<ResponseBase<ErrorResponse>>
    {
        public int Id { get; set; }

        public GetErrorByIdQuery() { }
        public GetErrorByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
