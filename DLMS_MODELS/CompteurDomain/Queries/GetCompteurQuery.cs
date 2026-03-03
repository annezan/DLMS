using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.CompteurDomain.Responses;

namespace DLMS_MODELS.CompteurDomain.Queries
{
    public class GetCompteurQuery : IRequest<ResponseBase<List<CompteurResponse>>>
    {
        public Guid UserId { get; set; }

        public GetCompteurQuery()
        {
        }

        public GetCompteurQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
