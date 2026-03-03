using DLMS_DAL.ErrorDomainDal.Repositories.Queries;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ErrorDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ErrorDomainDal.Repositories.Queries
{
    public class ErrorQueryRepository : QueryBaseRepository<Error>, IErrorQueryRepository
    {

        public ErrorQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<Error?> GetErrorById(int id)
        {
            return await _context.Error.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Error?> GetErrorByValue(int value)
        {
            return await _context.Error.AsNoTracking().FirstOrDefaultAsync(x => x.Value == value);
        }
        public async Task<List<Error?>> GetError()
        {
            return await _context.Error.ToListAsync();
        }

    }
}
