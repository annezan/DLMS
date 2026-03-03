using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class RoleDeleteCommand : IRequest<ResponseBase<string>>
    {
        public Guid Id { get; private set; }
        private DateTime? DeletedAt { get; set; } = null;
        public RoleDeleteCommand(Guid Id)
        {
            this.Id = Id;
            this.DeletedAt = DateTime.Now;
        }
    }
}
