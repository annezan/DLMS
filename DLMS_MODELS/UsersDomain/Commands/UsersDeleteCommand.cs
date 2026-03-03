using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersDeleteCommand : IRequest<ResponseBase<string>>
    {
        public Guid Id { get; private set; }
        private DateTime? DeletedAt { get; set; } = null;

        public UsersDeleteCommand(Guid Id)
        {
            this.Id = Id;
            this.DeletedAt = DateTime.Now;
        }
    }
}
