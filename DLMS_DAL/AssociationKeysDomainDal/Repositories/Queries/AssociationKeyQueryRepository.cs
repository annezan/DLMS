using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries
{
    public class AssociationKeyQueryRepository : QueryBaseRepository<AssociationKey>, IAssociationKeyQueryRepository
    {

        public AssociationKeyQueryRepository(DLMSDBContext context) :
            base(context)
        {

        }
        public async Task<List<AssociationKey?>> GetAssociationKeyByType(string type, string compteurid)
        {
            try
            {

                return await _context.AssociationKeys.Where(x => x.Type == type && x.CompteurId == compteurid).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            
        }
        public async Task<AssociationKey?> GetAssociationKeyExisting(string Keyvalue,string compteurid)
        {
            string keyvalueEncrypt = Cryptage.Encrypt(Keyvalue, "ASCDLMS");

            return await _context.AssociationKeys.AsNoTracking().FirstOrDefaultAsync(x => x.Keyvalue == keyvalueEncrypt && x.CompteurId == compteurid);
        }
        public async Task<List<AssociationKey?>> GetAssociationKey()
        {
            return await _context.AssociationKeys.ToListAsync();
        }

        // Ma méthode personnalisée pour le service DLMS
        public async Task<AssociationKey?> GetByKeyAsync(string keyName, string serialNumber, string keyType)
        {
            return await _context.AssociationKeys
                .Where(x => x.Type == keyType && 
                           x.Keyname == keyName && 
                           x.CompteurId.Contains(serialNumber))
                .FirstOrDefaultAsync();
        }

    }
}
