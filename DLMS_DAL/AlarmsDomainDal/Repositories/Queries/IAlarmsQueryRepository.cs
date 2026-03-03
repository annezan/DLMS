using DLMS_DAL.Bases;
using DLMS_MODELS.AlarmsDomain.Entities;
using DLMS_MODELS.AlarmsDomain.Entities;

namespace DLMS_DAL.AlarmsDomainDal.Repositories.Queries
{
    public interface IAlarmsQueryRepository : IQueryBaseRepository<Alarms>
    {
        Task<Alarms?> GetAlarmsById(int id);
        Task<Alarms?> GetAlarmsByValue(int value);
        Task<List<Alarms?>> GetAlarms();

    }
}
