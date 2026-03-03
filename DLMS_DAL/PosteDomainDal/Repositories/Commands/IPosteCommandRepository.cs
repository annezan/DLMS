using DLMS_DAL.Bases;
using DLMS_MODELS.PosteDomain.Entities;

namespace DLMS_DAL.PosteDomainDal.Repositories.Commands
{
    public interface IPosteCommandRepository : ICommandRepository<Poste>
    {
         Task<Poste> AddPoste(Poste Poste);
         Task<Poste> EditPoste(Poste Poste);
         Task<Poste> DeletePoste(Poste Poste);

    }
}
