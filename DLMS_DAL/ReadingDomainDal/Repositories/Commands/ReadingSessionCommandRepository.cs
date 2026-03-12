using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Commands;

public class ReadingSessionCommandRepository : CommandRepository<ReadingSession>, IReadingSessionCommandRepository
{
    public ReadingSessionCommandRepository(DLMSDBContext context) : base(context)
    {
    }

    public async Task<ReadingCycle> AddCycleAsync(ReadingCycle cycle)
    {
        await _context.ReadingCycles.AddAsync(cycle);
        await _context.SaveChangesAsync();
        return cycle;
    }

    public async Task UpdateCycleAsync(ReadingCycle cycle)
    {
        _context.Entry(cycle).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<ReadingSession> AddSessionAsync(ReadingSession session)
    {
        await _context.ReadingSessions.AddAsync(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task UpdateSessionAsync(ReadingSession session)
    {
        _context.Entry(session).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task AddPassResultAsync(SessionPassResult passResult)
    {
        await _context.SessionPassResults.AddAsync(passResult);
        await _context.SaveChangesAsync();
    }

    public async Task BulkInsertMeterReadingStatusesAsync(List<MeterReadingStatus> statuses)
    {
        if (statuses.Count == 0) return;

        // Batch insert in chunks of 500 to avoid parameter limits
        const int batchSize = 500;
        for (int i = 0; i < statuses.Count; i += batchSize)
        {
            var batch = statuses.Skip(i).Take(batchSize).ToList();
            await _context.MeterReadingStatuses.AddRangeAsync(batch);
            await _context.SaveChangesAsync();
        }
    }

    public async Task BulkInsertIpSessionStatsAsync(List<IpSessionStats> stats)
    {
        if (stats.Count == 0) return;
        await _context.IpSessionStats.AddRangeAsync(stats);
        await _context.SaveChangesAsync();
    }

    public async Task AddConfigurationAsync(ReadingConfiguration config)
    {
        await _context.ReadingConfigurations.AddAsync(config);
        await _context.SaveChangesAsync();
    }
}
