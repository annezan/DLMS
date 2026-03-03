using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class UsersAuthenticateCommand : IRequest<AuthentificationResponse>
    {
        public User Users { get; private set; }

        public UsersAuthenticateCommand(User user)
        {
            Users = user;
        }
    }
}
