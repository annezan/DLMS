using DLMS_DAL.Bases;
using DLMS_MODELS.AssociationKeyDomain.Entities;

namespace DLMS_DAL.AssociationKeyDomainDal.Repositories.Commands
{
    public interface IAssociationKeyCommandRepository : ICommandRepository<AssociationKey>
    {
        Task<AssociationKey> EditAssociationKey(AssociationKey AssociationKey);
        Task<List<AssociationKey>> AddAssociationKey(List<AssociationKey> AssociationKey);

    }

}
