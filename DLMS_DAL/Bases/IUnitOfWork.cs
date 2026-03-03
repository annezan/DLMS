namespace DLMS_DAL.Bases
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class, new();

        void BeginTransaction();

        int Commit();

        void Rollback();

        void Dispose(bool disposing);
    }
}
