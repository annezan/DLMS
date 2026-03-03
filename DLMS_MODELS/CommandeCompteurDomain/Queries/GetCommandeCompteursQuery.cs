using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeCompteurDomain.Queries
{
    public class GetCommandeCompteursQuery : IRequest<ResponseBase<List<CommandeCompteurResponse>>>
    {
        public Guid UserId { get; set; } // Added for access checks
        
        public GetCommandeCompteursQuery(Guid userId)
        {
            UserId = userId;
        }
    }
} 