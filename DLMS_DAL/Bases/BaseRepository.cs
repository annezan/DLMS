using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLMS_DAL.Bases
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbEntitySet;
        //private bool _disposed;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbEntitySet = _context.Set<TEntity>();
        }

        public void Add(IEnumerable<TEntity> entity)
        {
            _dbEntitySet.AddRange(entity);
        }

        public void Add(TEntity entity)
        {
            _dbEntitySet.Add(entity);
        }

        public int Count()
        {
            return Count(null);
        }

        public int Count(Expression<Func<TEntity, bool>> query)
        {
            if (null == query)
                return _dbEntitySet.Count();
            else return _dbEntitySet.Where(query).Count();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            _dbEntitySet.RemoveRange(entities);
        }

        public void Delete(Expression<Func<TEntity, bool>> query)
        {
            TEntity entity = _dbEntitySet.FirstOrDefault(query);
            if (null != entity) _dbEntitySet.Remove(entity);
        }

        public async void DeleteAsync(Expression<Func<TEntity, bool>> query)
        {
            TEntity entity = await _dbEntitySet.FirstOrDefaultAsync(query);
            if (null != entity) _dbEntitySet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbEntitySet.Attach(entity);
            _dbEntitySet.Remove(entity);
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();

            GC.SuppressFinalize(this);
        }

        public bool Exist()
        {
            return Exist(null);
        }

        public bool Exist(Expression<Func<TEntity, bool>> query)
        {
            try
            {
                var isExist = default(IList<TEntity>);
                if (null != query)
                    isExist = _dbEntitySet.Where(query).ToList();
                else isExist = _dbEntitySet.ToList();
                if (null != isExist && isExist.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbEntitySet.AsQueryable();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbEntitySet.Where(predicate);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> entityKey)
        {
            return _dbEntitySet.FirstOrDefault(entityKey);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> entityKey)
        {
            return await _dbEntitySet.FirstOrDefaultAsync(entityKey);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> entityKey)
        {
            return _dbEntitySet.Find(entityKey);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> entityKey)
        {
            return await _dbEntitySet.FindAsync(entityKey);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities) Update(entity);
        }

        public void Update(TEntity entity)
        {
            if (null != entity)
            {
                if (_context.Entry<TEntity>(entity).State == EntityState.Detached)
                    _dbEntitySet.Attach(entity);
                else
                {
                    _context.Entry<TEntity>(entity).State = EntityState.Detached;
                    _dbEntitySet.Attach(entity);
                }
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
        }

        public void Update(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            TEntity _entity = GetSingle(predicate);
            if (null != entity)
            {
                // _entity = HSF.Core.Object.ConvertEntity<TEntity, TEntity>(entity);
                Update(entity);
            }
        }
    }


}
