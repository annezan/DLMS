using DLMS_DAL.Bases;
using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Commands;

public interface IReadingSessionCommandRepository : ICommandRepository<ReadingSession>
{
    Task<ReadingCycle> AddCycleAsync(ReadingCycle cycle);
    Task UpdateCycleAsync(ReadingCycle cycle);
    Task<ReadingSession> AddSessionAsync(ReadingSession session);
    Task UpdateSessionAsync(ReadingSession session);
    Task AddPassResultAsync(SessionPassResult passResult);
    Task BulkInsertMeterReadingStatusesAsync(List<MeterReadingStatus> statuses);
    Task BulkInsertIpSessionStatsAsync(List<IpSessionStats> stats);
    Task AddConfigurationAsync(ReadingConfiguration config);
}
