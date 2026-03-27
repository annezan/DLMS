using DLMS_MODELS;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_UTILITIES;
using Microsoft.Extensions.Logging;

namespace DLMS_SERVICE.Services
{
    public interface IDataProcessingService
    {
        Task<bool> ProcessCompteurDataAsync(string data, int compteurId);
    }

    public class DataProcessingService : IDataProcessingService
    {
        private readonly ILogger<DataProcessingService> _logger;
        private readonly ICompteurCommandRepository _compteurCommandRepo;
        private readonly ICompteurQueryRepository _compteurQueryRepo;

        public DataProcessingService(
            ILogger<DataProcessingService> logger,
            ICompteurCommandRepository compteurCommandRepo,
            ICompteurQueryRepository compteurQueryRepo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compteurCommandRepo = compteurCommandRepo ?? throw new ArgumentNullException(nameof(compteurCommandRepo));
            _compteurQueryRepo = compteurQueryRepo ?? throw new ArgumentNullException(nameof(compteurQueryRepo));
        }

        public async Task<bool> ProcessCompteurDataAsync(string data, int compteurId)
        {
            try
            {
                _logger.LogDebug("Traitement des données pour le compteur ID: {CompteurId}", compteurId);

                var compteurUtilities = new CompteurUtilities(_compteurCommandRepo, _compteurQueryRepo);
                var success = compteurUtilities.MAJCompteur(data, compteurId);

                if (!success)
                {
                    _logger.LogWarning("MAJCompteur a retourné false pour compteur {CompteurId} — données non persistées", compteurId);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement des données du compteur {CompteurId}", compteurId);
                return false;
            }
        }
    }
}
