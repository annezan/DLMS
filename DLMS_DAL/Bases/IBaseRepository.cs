using System.Linq.Expressions;

namespace DLMS_DAL.Bases
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        TEntity GetSingle(Expression<Func<TEntity, bool>> entityKey);

        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> entityKey);

        TEntity GetFirst(Expression<Func<TEntity, bool>> entityKey);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> entityKey);

        void Add(TEntity entity);

        void Add(IEnumerable<TEntity> entity);

        void Delete(TEntity entity);

        void Delete(Expression<Func<TEntity, bool>> query);

        void Delete(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Update(IEnumerable<TEntity> entities);

        void Update(TEntity entity, Expression<Func<TEntity, bool>> predicate);

        int Count(Expression<Func<TEntity, bool>> query);

        int Count();

        bool Exist(Expression<Func<TEntity, bool>> query);

        bool Exist();

        void Save();
    }

}
