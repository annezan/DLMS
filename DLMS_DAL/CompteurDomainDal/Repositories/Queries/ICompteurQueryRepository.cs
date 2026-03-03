using DLMS_DAL.Bases;
using DLMS_MODELS.CompteurDomain.Entities;

namespace DLMS_DAL.CompteurDomainDal.Repositories.Queries
{
    public interface ICompteurQueryRepository : IQueryBaseRepository<Compteur>
    {
        Task<List<Compteur>> GetCompteur();
        Task<Compteur> GetCompteurById(int Id);
        Task<List<Compteur>> GetActiveCompteursAsync();
        Task<List<DateTime>> GetReadHoursForCompteurAsync(string numeroCompteur, DateTime startDate, DateTime endDate);
    }
}
