using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;
using DLMS_MODELS.CommandeCompteurDomain.Responses;

namespace DLMS_MODELS.CommandeCompteurDomain.Commands
{
    public class CommandeCompteurDeleteCommand : IRequest<ResponseBase<string>>
    {

        public int Id { get; set; }
        public Guid UserId { get; set; } // Added for access checks
        public string DeletedBy { get; set; }

        public CommandeCompteurDeleteCommand()
        {
        }
    }
}
