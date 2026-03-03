using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.UsersDomainDal.Repositories.Commands;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories
{
    public class RolesCommandRepository : CommandRepository<Role>, IRolesCommandRepository
    {
        public RolesCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
    }
}
