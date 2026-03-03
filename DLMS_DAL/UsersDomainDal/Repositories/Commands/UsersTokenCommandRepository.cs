using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Commands
{
    public class UsersTokenCommandRepository : CommandRepository<TokenUser>, IUsersTokenCommandRepository
    {
        public UsersTokenCommandRepository(DLMSDBContext context)
            : base(context)
        {
        }
    }
}
