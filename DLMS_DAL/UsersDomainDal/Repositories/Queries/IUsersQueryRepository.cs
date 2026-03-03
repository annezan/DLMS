using DLMS_DAL.Bases;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public interface IUsersQueryRepository : IQueryBaseRepository<User>
    {
        Task<IReadOnlyList<User>> GetAllUsersAsync();

        Task<User> GetUsersById(Guid id);

        Task<User> GetUsersByEmail(string email);

        Task<User> GetUsersByMobile(string mobile);
    }
}
