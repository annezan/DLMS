using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeCompteurDomain.Queries
{
    public class GetCommandeCompteurByIdQuery : IRequest<ResponseBase<CommandeCompteurResponse>>
    {
        public int Id { get; }
        public Guid UserId { get; set; } // Added for access checks

        public GetCommandeCompteurByIdQuery(int id)
        {
            Id = id;
        }
    }
} 