using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.UsersDomainDal.Repositories
{
    public class UsersCommandRepository : CommandRepository<User>, IUsersCommandRepository
    {
        public UsersCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }

        public async Task<User> Login(string loginId, string password)
        {
            try
            {
                var utilisateurs = await _context.Users.Where(x => x.DeletedAt == null).ToListAsync();

                var user = utilisateurs.FirstOrDefault(x => x.Email.ToLower().Contains(loginId.ToLower()));

                if (user == null) return user;

                var isVerified = BCryptHelper.VerifyPassword(password, user.MotDePasse);

                if (!isVerified)
                {
                    user = null; return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> Register(User utilisateur)
        {
            utilisateur.MotDePasse = BCryptHelper.CryptPassword(utilisateur.MotDePasse);
            await _context.Users.AddAsync(utilisateur);
            await _context.SaveChangesAsync();
            utilisateur.CodeValidation = string.Empty;
            utilisateur.MotDePasse = string.Empty;
            return utilisateur;
        }
    }
}
