using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.TypecommandeDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.TypecommandeDomainDal.Repositories.Queries
{
    public class TypecommandeQueryRepository : QueryBaseRepository<Typecommande>, ITypecommandeQueryRepository
    {

        public TypecommandeQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<List<Typecommande?>> GetTypecommande()
        {
            return await _context.Typecommandes.ToListAsync();
        }

    }
}
