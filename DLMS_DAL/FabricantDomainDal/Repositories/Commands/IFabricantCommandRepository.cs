using DLMS_DAL.Bases;
using DLMS_MODELS.FabricantDomain.Entities;

namespace DLMS_DAL.FabricantDomainDal.Repositories.Commands
{
    public interface IFabricantCommandRepository : ICommandRepository<Fabricant>
    {
        Task<Fabricant> AddFabricant(Fabricant Fabricant);

        Task<Fabricant> EditFabricant(Fabricant Fabricant);
        Task<Fabricant> DeleteFabricant(Fabricant Fabricant);

    }
}
