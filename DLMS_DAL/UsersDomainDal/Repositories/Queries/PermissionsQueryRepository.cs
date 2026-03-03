using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.UsersDomainDal.Repositories.Queries
{
    public class PermissionsQueryRepository : QueryBaseRepository<Permission>, IPermissionsQueryRepository
    {
        public PermissionsQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
    }
}
