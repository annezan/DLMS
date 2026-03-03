using DLMS_DAL.AlarmsDomainDal.Repositories.Queries;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.AlarmsDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.AlarmsDomainDal.Repositories.Queries
{
    public class AlarmsQueryRepository : QueryBaseRepository<Alarms>, IAlarmsQueryRepository
    {

        public AlarmsQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<Alarms?> GetAlarmsById(int id)
        {
            return await _context.Alarms.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Alarms?> GetAlarmsByValue(int value)
        {
            return await _context.Alarms.AsNoTracking().FirstOrDefaultAsync(x => x.Value == value);
        }
        public async Task<List<Alarms?>> GetAlarms()
        {
            return await _context.Alarms.ToListAsync();
        }

    }
}
