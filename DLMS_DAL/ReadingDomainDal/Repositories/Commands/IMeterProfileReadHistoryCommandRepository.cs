using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Commands;

public interface IMeterProfileReadHistoryCommandRepository
{
    Task UpsertAsync(string compteurSerial, string profileObis, DateTime lastReadUpTo, int rowsRead, long durationMs);
}
