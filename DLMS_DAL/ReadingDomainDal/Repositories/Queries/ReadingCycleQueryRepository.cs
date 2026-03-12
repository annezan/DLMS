using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public class ReadingCycleQueryRepository : QueryBaseRepository<ReadingCycle>, IReadingCycleQueryRepository
{
    public ReadingCycleQueryRepository(DLMSDBContext context) : base(context)
    {
    }

    public async Task<ReadingCycle?> GetActiveCycleAsync()
    {
        return await _context.ReadingCycles
            .Where(c => c.Statut == ReadingCycleStatus.EnCours && !c.IsArchive)
            .OrderByDescending(c => c.DateDebut)
            .FirstOrDefaultAsync();
    }

    public async Task<ReadingCycle?> GetCycleWithSessionsAsync(int cycleId)
    {
        return await _context.ReadingCycles
            .Include(c => c.Sessions)
            .FirstOrDefaultAsync(c => c.Id == cycleId);
    }
}
