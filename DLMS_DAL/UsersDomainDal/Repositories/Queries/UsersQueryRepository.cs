using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public class UsersQueryRepository : QueryBaseRepository<User>, IUsersQueryRepository
    {

        public UsersQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }

        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Poste)
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<User> GetUsersByEmail(string email)
        {
            return await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Poste)
                .SingleOrDefaultAsync(x => x.DeletedAt == null && x.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetUsersById(Guid id)
        {
            return await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Poste)
                .SingleOrDefaultAsync(x => x.DeletedAt == null && x.Id == id);
        }

        public async Task<User> GetUsersByMobile(string mobile)
        {
            return await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Poste)
                .SingleOrDefaultAsync(x => x.DeletedAt == null && x.Mobile == mobile);
        }

    }
}
