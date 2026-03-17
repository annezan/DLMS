using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public class MeterProfileReadHistoryQueryRepository : QueryBaseRepository<MeterProfileReadHistory>, IMeterProfileReadHistoryQueryRepository
{
    public MeterProfileReadHistoryQueryRepository(DLMSDBContext context) : base(context)
    {
    }

    public async Task<MeterProfileReadHistory?> GetLastReadAsync(string compteurSerial, string profileObis)
    {
        return await _context.MeterProfileReadHistories
            .AsNoTracking()
            .Where(h => h.CompteurSerial == compteurSerial && h.ProfileObis == profileObis)
            .FirstOrDefaultAsync();
    }

    public async Task<List<MeterProfileReadHistory>> GetAllForMeterAsync(string compteurSerial)
    {
        return await _context.MeterProfileReadHistories
            .AsNoTracking()
            .Where(h => h.CompteurSerial == compteurSerial)
            .ToListAsync();
    }
}
