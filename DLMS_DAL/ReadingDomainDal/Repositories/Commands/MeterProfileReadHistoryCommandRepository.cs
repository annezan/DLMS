using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.ReadingDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Commands;

public class MeterProfileReadHistoryCommandRepository : CommandRepository<MeterProfileReadHistory>, IMeterProfileReadHistoryCommandRepository
{
    public MeterProfileReadHistoryCommandRepository(DLMSDBContext context) : base(context)
    {
    }

    public async Task UpsertAsync(string compteurSerial, string profileObis, DateTime lastReadUpTo, int rowsRead, long durationMs)
    {
        var existing = await _context.MeterProfileReadHistories
            .Where(h => h.CompteurSerial == compteurSerial && h.ProfileObis == profileObis)
            .FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.LastReadUpTo = lastReadUpTo;
            existing.LastReadAt = DateTime.Now;
            existing.RowsRead = rowsRead;
            existing.ReadDurationMs = durationMs;
            existing.UpdatedAt = DateTime.Now;
        }
        else
        {
            var newHistory = new MeterProfileReadHistory
            {
                CompteurSerial = compteurSerial,
                ProfileObis = profileObis,
                LastReadUpTo = lastReadUpTo,
                LastReadAt = DateTime.Now,
                RowsRead = rowsRead,
                ReadDurationMs = durationMs,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _context.MeterProfileReadHistories.AddAsync(newHistory);
        }

        await _context.SaveChangesAsync();
    }
}
