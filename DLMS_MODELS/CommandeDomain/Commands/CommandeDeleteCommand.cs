using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeDomain.Commands
{
    public class CommandeDeleteCommand : IRequest<ResponseBase<string>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string DeletedBy { get; set; }

        public CommandeDeleteCommand()
        {
        }
    }
}
