using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace DLMS_DAL.Bases
{
    public class QueryBaseRepository<T> : IQueryBaseRepository<T> where T : class
    {
        protected readonly DLMSDBContext _context;

        public QueryBaseRepository(DLMSDBContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(params object[] keyValues)
        {
            return await _context.Set<T>().FindAsync(keyValues);
        }
    }
}
