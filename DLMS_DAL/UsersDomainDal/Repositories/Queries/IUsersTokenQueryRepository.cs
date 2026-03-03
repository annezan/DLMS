using DLMS_DAL.Bases;
using DLMS_MODELS.UsersDomain.Entities;
using System.Security.Claims;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public interface IUsersTokenQueryRepository : IQueryBaseRepository<TokenUser>
    {
        Task<User> GetRefreshTokenAsync(ClaimsPrincipal claimsPrincipal, string refreshToken);

        Task<List<TokenUser>> GetTokensByUserIdAsync(Guid userId);
    }
}
