using DLMS_MODELS.ReadingDomain.Entities;

namespace DLMS_DAL.ReadingDomainDal.Repositories.Queries;

public interface IMeterProfileReadHistoryQueryRepository
{
    Task<MeterProfileReadHistory?> GetLastReadAsync(string compteurSerial, string profileObis);
    Task<List<MeterProfileReadHistory>> GetAllForMeterAsync(string compteurSerial);
}
