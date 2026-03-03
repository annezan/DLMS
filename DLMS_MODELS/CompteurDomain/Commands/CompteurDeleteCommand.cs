using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurDomain.Commands
{
    public class CompteurDeleteCommand : IRequest<ResponseBase<CompteurResponse>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string DeletedBy { get; set; }

        public CompteurDeleteCommand()
        {
        }
    }
}
