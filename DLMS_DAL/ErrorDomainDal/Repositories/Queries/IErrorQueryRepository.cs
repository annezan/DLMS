using DLMS_DAL.Bases;
using DLMS_MODELS.ErrorDomain.Entities;
using DLMS_MODELS.ErrorDomain.Entities;

namespace DLMS_DAL.ErrorDomainDal.Repositories.Queries
{
    public interface IErrorQueryRepository : IQueryBaseRepository<Error>
    {
        Task<Error?> GetErrorById(int id);
        Task<Error?> GetErrorByValue(int value);
        Task<List<Error?>> GetError();

    }
}
