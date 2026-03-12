using DLMS_DAL.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public interface IMeterReadingStatusQueryRepository : IQueryBaseRepository<MeterReadingStatus>
{
    Task<List<int>> GetUnreadMeterIdsForCycleAsync(int cycleId);
    Task<int> GetReadCountForCycleAsync(int cycleId);
}
