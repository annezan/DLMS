using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.CompteurDomain.Responses;

namespace DLMS_MODELS.CompteurDomain.Queries
{
    public class GetCompteurByIdQuery : IRequest<ResponseBase<CompteurResponse>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public GetCompteurByIdQuery() { }

        public GetCompteurByIdQuery(int Id)
        {
            this.Id = Id;
        }

        public GetCompteurByIdQuery(int Id, Guid userId)
        {
            this.Id = Id;
            this.UserId = userId;
        }
    }
}
