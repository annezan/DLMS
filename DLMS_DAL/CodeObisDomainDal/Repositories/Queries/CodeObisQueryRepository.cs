using DLMS_DAL.CodeObisDomainDal.Repositories.Queries;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CodeObisDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CodeObisDomainDal.Repositories.Queries
{
    public class CodeObisQueryRepository : QueryBaseRepository<CodeObis>, ICodeObisQueryRepository
    {

        public CodeObisQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<CodeObis?> GetCodeObisById(int id)
        {
            return await _context.CodeObis.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<CodeObis?> GetCodeObisByValue(string value)
        {
            return await _context.CodeObis.AsNoTracking().FirstOrDefaultAsync(x => x.Value == value);
        }
        public async Task<List<CodeObis?>> GetCodeObis()
        {
            return await _context.CodeObis.ToListAsync();
        }

    }
}
