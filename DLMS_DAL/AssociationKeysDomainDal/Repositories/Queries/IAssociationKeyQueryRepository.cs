using DLMS_DAL.Bases;
using DLMS_MODELS.AssociationKeyDomain.Entities;

namespace DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries
{
    public interface IAssociationKeyQueryRepository : IQueryBaseRepository<AssociationKey>
    {
        Task<List<AssociationKey>> GetAssociationKeyByType(string type,string id);
        Task<AssociationKey> GetAssociationKeyExisting(string Keyvalue, string compteurid);
        Task<List<AssociationKey>> GetAssociationKey();
        Task<AssociationKey?> GetByKeyAsync(string keyName, string serialNumber, string keyType);
    }
}
