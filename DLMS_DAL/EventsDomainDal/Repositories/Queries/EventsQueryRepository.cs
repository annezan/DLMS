using DLMS_DAL.EventsDomainDal.Repositories.Queries;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.EventsDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.EventsDomainDal.Repositories.Queries
{
    public class EventsQueryRepository : QueryBaseRepository<Events>, IEventsQueryRepository
    {

        public EventsQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<Events?> GetEventsById(int id)
        {
            return await _context.Events.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Events?> GetEventsByValue(int value)
        {
            return await _context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Value == value);
        }
        public async Task<List<Events?>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

    }
}
