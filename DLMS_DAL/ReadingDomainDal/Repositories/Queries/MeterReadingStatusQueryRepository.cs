using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using DLMS_MODELS.ReadingDomain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public class MeterReadingStatusQueryRepository : QueryBaseRepository<MeterReadingStatus>, IMeterReadingStatusQueryRepository
{
    public MeterReadingStatusQueryRepository(DLMSDBContext context) : base(context)
    {
    }

    /// <summary>
    /// Returns CompteurEquipement IDs that have NOT been successfully read in this cycle.
    /// Critical query for inter-session persistence.
    /// </summary>
    public async Task<List<int>> GetUnreadMeterIdsForCycleAsync(int cycleId)
    {
        var readMeterIds = await _context.MeterReadingStatuses
            .Where(mrs => mrs.ReadingSession.ReadingCycleId == cycleId
                       && mrs.Resultat == MeterReadingResult.Lu)
            .Select(mrs => mrs.CompteurEquipementId)
            .Distinct()
            .ToListAsync();

        var allMeterIds = await _context.CompteurEquipement
            .Where(ce => !ce.IsArchive)
            .Select(ce => ce.Id)
            .ToListAsync();

        return allMeterIds.Except(readMeterIds).ToList();
    }

    public async Task<int> GetReadCountForCycleAsync(int cycleId)
    {
        return await _context.MeterReadingStatuses
            .Where(mrs => mrs.ReadingSession.ReadingCycleId == cycleId
                       && mrs.Resultat == MeterReadingResult.Lu)
            .Select(mrs => mrs.CompteurEquipementId)
            .Distinct()
            .CountAsync();
    }
}
