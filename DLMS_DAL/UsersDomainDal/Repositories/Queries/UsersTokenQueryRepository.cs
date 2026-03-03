using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public class UsersTokenQueryRepository : QueryBaseRepository<TokenUser>, IUsersTokenQueryRepository
    {
        public UsersTokenQueryRepository(DLMSDBContext context)
            : base(context)
        {
        }

        public async Task<User> GetRefreshTokenAsync(ClaimsPrincipal claimsPrincipal, string refreshToken)
        {
            var expiryDateUnix = long.Parse(claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                //return new AuthentificationResponse { Errors = new[] { "This token hasn't expired yet" } };
                return null;
            }
            var jti = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _context.TokenUsers.FirstOrDefault(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                // return new AuthentificationResponse { Errors = new[] { "This refresh token does not exist" } };
                return null;
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryAt)
            {
                //return new AuthentificationResponse { Errors = new[] { "This refresh token has expired" } };

                return null;
            }

            if (storedRefreshToken.IsUsed == true)
            {
                //return new AuthentificationResponse { Errors = new[] { "This refresh token has been used" } };
                return null;
            }

            if (storedRefreshToken.JwtId != jti)
            {
                //return new AuthentificationResponse { Errors = new[] { "This refresh token does not match this JWT" } };
                return null;
            }

            storedRefreshToken.IsUsed = true;
            _context.TokenUsers.Update(storedRefreshToken);

            await _context.SaveChangesAsync();
            string strUserId = claimsPrincipal.Claims.Single(x => x.Type == ClaimsKey.UtilisateurId).Value;
            Guid userId = Guid.Empty;
            Guid.TryParse(strUserId, out userId);
            var user = _context.Users.FirstOrDefault(c => c.Id == userId);
            if (user == null)
            {
                //return new AuthentificationResponse { Errors = new[] { "User Not Found" } };
                return null;
            }

            // return await new AuthentificationResponse {  = }
            return user;
        }

        public async Task<List<TokenUser>> GetTokensByUserIdAsync(Guid userId)
        {
            return await _context.TokenUsers.Where(x => x.UsersId == userId).ToListAsync();

        }
    }

}
