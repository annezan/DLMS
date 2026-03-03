using DLMS_MODELS.EventsDomain.Entities;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.EventDomainDal.Repositories.Queries
{
    public interface IEventQueryRepository
    {
        Task<Events?> GetByValueAndCategoryAsync(long value, string category);
    }

    public class EventQueryRepository : IEventQueryRepository
    {
        private readonly DLMSDBContext _context;

        public EventQueryRepository(DLMSDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Events?> GetByValueAndCategoryAsync(long value, string category)
        {
            return await _context.Events
                .Where(x => x.Value == value && x.Category == category)
                .FirstOrDefaultAsync();
        }
    }
}
