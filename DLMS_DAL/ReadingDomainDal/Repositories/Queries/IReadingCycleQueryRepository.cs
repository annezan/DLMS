using DLMS_DAL.Bases;
using DLMS_MODELS.ReadingDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public interface IReadingCycleQueryRepository : IQueryBaseRepository<ReadingCycle>
{
    Task<ReadingCycle?> GetActiveCycleAsync();
    Task<ReadingCycle?> GetCycleWithSessionsAsync(int cycleId);
}
