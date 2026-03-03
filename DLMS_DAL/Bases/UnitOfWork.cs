using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace DLMS_DAL.Bases
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private IDbContextTransaction _transaction = null;
        private bool _disposed;
        private Hashtable _repositories;

        public DLMSDBContext LeContext
        {
            get { return _context as DLMSDBContext; }
        }

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class, new()
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IBaseRepository<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(BaseRepository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));

            return (IBaseRepository<TEntity>)_repositories[type];
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public int Saves()
        {
            try
            {
                if (null == _context)
                {
                    return -1;
                }
                _context.SaveChanges();
                return 0;
            }
            catch { return -1; }
        }

        public int Commit()
        {
            try
            {
                if (null != _transaction)
                {
                    _context.SaveChanges();
                    _transaction.Commit();
                    return 0;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
        }

        public void Rollback()
        {
            if (null != _transaction) _transaction.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                foreach (IDisposable repository in _repositories.Values)
                {
                    repository.Dispose();
                }
            }
            _disposed = true;
        }
    }


}
