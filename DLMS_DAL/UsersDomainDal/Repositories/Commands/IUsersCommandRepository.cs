using DLMS_DAL.Bases;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Commands
{
    public interface IUsersCommandRepository : ICommandRepository<User>
    {
        Task<User> Register(User utilisateur);

        Task<User> Login(string loginId, string password);
    }
}
