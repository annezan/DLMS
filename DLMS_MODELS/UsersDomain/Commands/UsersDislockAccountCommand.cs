using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersDislockAccountCommand : IRequest<ResponseBase<string>>
    {
        public Guid Id { get; private set; }

        public UsersDislockAccountCommand(Guid Id)
        {
            this.Id = Id;
        }
    }
}
