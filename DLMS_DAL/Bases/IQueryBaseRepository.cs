using System.Linq.Expressions;

namespace DLMS_DAL.Bases
{
    public interface IQueryBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);

        Task<T> GetByIdAsync(params object[] keyValues);
    }
}
