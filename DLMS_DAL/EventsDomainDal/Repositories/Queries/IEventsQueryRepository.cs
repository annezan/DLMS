using DLMS_DAL.Bases;
using DLMS_MODELS.EventsDomain.Entities;
using DLMS_MODELS.EventsDomain.Entities;

namespace DLMS_DAL.EventsDomainDal.Repositories.Queries
{
    public interface IEventsQueryRepository : IQueryBaseRepository<Events>
    {
        Task<Events?> GetEventsById(int id);
        Task<Events?> GetEventsByValue(int value);
        Task<List<Events?>> GetEvents();

    }
}
