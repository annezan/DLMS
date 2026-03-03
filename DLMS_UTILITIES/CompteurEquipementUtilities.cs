
using DLMS.Infrastructure;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Queries;
using DLMS_MODELS.CompteurEquipementDomain.Entities;

namespace DLMS_UTILITIES
{
    public class CompteurEquipementUtilities
    {
        private readonly ICompteurEquipementQueryRepository _CompteurEquipementQueryRepository;

        public CompteurEquipementUtilities(ICompteurEquipementQueryRepository CompteurEquipementQueryRepository)
        {
            _CompteurEquipementQueryRepository = CompteurEquipementQueryRepository;
        }
        public async Task<List<CompteurEquipement>> GetCompteurEquipement()
        {
            try
            {
                return await _CompteurEquipementQueryRepository.GetCompteurEquipement();

            }
            catch (Exception ex) 
            {
                throw new Exception();
            }
        }



    }
}
