namespace DLMS_DAL.Bases
{
    public interface ICommandRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);
    }
}
