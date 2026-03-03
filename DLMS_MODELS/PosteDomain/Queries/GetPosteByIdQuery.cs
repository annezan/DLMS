using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.PosteDomain.Responses;

namespace DLMS_MODELS.PosteDomain.Queries
{
    public class GetPosteByIdQuery : IRequest<ResponseBase<PosteResponse>>
    {
        public int IdPoste { get; set; }
        public GetPosteByIdQuery() { }
        public GetPosteByIdQuery(int idPoste)
        {
            this.IdPoste = idPoste;
        }
    }
}
