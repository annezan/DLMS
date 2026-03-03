using DLMS_DAL.Bases;
using DLMS_MODELS.CodeObisDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Entities;

namespace DLMS_DAL.CodeObisDomainDal.Repositories.Queries
{
    public interface ICodeObisQueryRepository : IQueryBaseRepository<CodeObis>
    {
        Task<CodeObis?> GetCodeObisById(int id);
        Task<CodeObis?> GetCodeObisByValue(string value);
        Task<List<CodeObis?>> GetCodeObis();

    }
}
