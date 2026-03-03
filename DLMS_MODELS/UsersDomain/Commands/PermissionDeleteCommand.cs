using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class PermissionDeleteCommand : IRequest<ResponseBase<string>>
    {
        public Guid Id { get; private set; }
        private DateTime? DeletedAt { get; set; } = null;

        public PermissionDeleteCommand(Guid Id)
        {
            this.Id = Id;
            this.DeletedAt = DateTime.Now;
        }
    }
}
