using DLMS_DAL.Bases;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public interface IReadObjectCommandeQueryRepository 
    {
        Task<string> GetReadObjectCommande(int commandeId);
    }
}
