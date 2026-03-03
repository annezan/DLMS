
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_MODELS.CompteurDomain.Entities;

namespace DLMS_UTILITIES
{
    public class CompteurUtilities
    {
        private readonly ICompteurCommandRepository _CompteurCommandRepository;

        private readonly ICompteurQueryRepository _CompteurQueryRepository;

        public CompteurUtilities(ICompteurCommandRepository compteurCommandRepository, ICompteurQueryRepository compteurQueryRepository)
        {
            _CompteurCommandRepository= compteurCommandRepository;
            _CompteurQueryRepository = compteurQueryRepository;
        }
        public async Task<List<Compteur>> GetCompteur()
        {
            try
            {
                return await _CompteurQueryRepository.GetCompteur();

            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }

        public async Task<Compteur> GetCompteurById(int Id)
        {
            try
            {
                return await _CompteurQueryRepository.GetCompteurById(Id);

            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }

        public bool MAJCompteur(string result, int Id)
        {
            try
            {
                return _CompteurCommandRepository.MAJCompteur(result,Id);

            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }

    }
}
