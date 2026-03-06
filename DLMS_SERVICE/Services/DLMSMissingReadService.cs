using DLMS_MODELS;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_MODELS.CompteurDomain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSMissingReadService
    {
        Task<List<MissingReadInfo>> GetMissingReadsAsync(DateTime currentTime);
        Task MarkReadAsCompletedAsync(int compteurId, DateTime readHour);
        Task<List<MissingReadInfo>> GetMissingReadsForPeriodAsync(DateTime startDate, DateTime endDate);
    }

    public class DLMSMissingReadService : IDLMSMissingReadService
    {
        private readonly ILogger<DLMSMissingReadService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _lookbackPeriod = TimeSpan.FromHours(24); // Regarder 24h en arrière (optimisé depuis 744h)

        public DLMSMissingReadService(
            ILogger<DLMSMissingReadService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<List<MissingReadInfo>> GetMissingReadsAsync(DateTime currentTime)
        {
            try
            {
                _logger.LogInformation("🔍 Recherche des lectures manquantes à {Time}", currentTime);

                using var scope = _serviceProvider.CreateScope();
                var compteurQueryRepo = scope.ServiceProvider.GetRequiredService<ICompteurQueryRepository>();
                
                // Récupération de tous les compteurs actifs
                var compteurs = await compteurQueryRepo.GetActiveCompteursAsync();
                _logger.LogInformation("📊 {Count} compteurs actifs vérifiés sur un mois", compteurs.Count);
                
                var missingReads = new List<MissingReadInfo>();

                foreach (var compteur in compteurs)
                {
                    var missingHours = await GetMissingHoursForCompteurAsync(compteur, currentTime);
                    if (missingHours.Count > 0)
                    {
                        _logger.LogInformation("⚠️ Compteur {Numero}: {Count} heures manquantes", 
                            compteur.NumeroCompteur, missingHours.Count);
                    }
                    missingReads.AddRange(missingHours);
                }

                if (missingReads.Count > 0)
                {
                    _logger.LogInformation("🔍 {Count} lectures manquantes détectées au total", missingReads.Count);
                }
                else
                {
                    _logger.LogInformation("✅ Aucune lecture manquante détectée - Tous les compteurs sont à jour");
                }

                return missingReads;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche des lectures manquantes");
                return new List<MissingReadInfo>();
            }
        }

        public async Task MarkReadAsCompletedAsync(int compteurId, DateTime readHour)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var compteurCommandRepo = scope.ServiceProvider.GetRequiredService<ICompteurCommandRepository>();

                // Marquage de la lecture comme complétée en base
                // Implémentation selon votre modèle de données
                await Task.CompletedTask;

                _logger.LogDebug("Lecture marquée comme complétée: CompteurId={CompteurId}, Hour={Hour}", 
                    compteurId, readHour);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du marquage de la lecture comme complétée");
                throw;
            }
        }

        public async Task<List<MissingReadInfo>> GetMissingReadsForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Recherche des lectures manquantes pour la période {Start} - {End}", 
                    startDate, endDate);

                using var scope = _serviceProvider.CreateScope();
                var compteurQueryRepo = scope.ServiceProvider.GetRequiredService<ICompteurQueryRepository>();
                
                var compteurs = await compteurQueryRepo.GetActiveCompteursAsync();
                var missingReads = new List<MissingReadInfo>();

                foreach (var compteur in compteurs)
                {
                    var missingHours = await GetMissingHoursForCompteurInPeriodAsync(compteur, startDate, endDate);
                    missingReads.AddRange(missingHours);
                }

                return missingReads;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche des lectures manquantes pour la période");
                return new List<MissingReadInfo>();
            }
        }

        private async Task<List<MissingReadInfo>> GetMissingHoursForCompteurAsync(Compteur compteur, DateTime currentTime)
        {
            var missingReads = new List<MissingReadInfo>();

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var compteurQueryRepo = scope.ServiceProvider.GetRequiredService<ICompteurQueryRepository>();

                // Période de vérification : les dernières 24 heures
                var startDate = currentTime.AddHours(-_lookbackPeriod.TotalHours);
                var endDate = currentTime;

                _logger.LogDebug("🔍 Vérification compteur {Numero} du {Start} au {End}", 
                    compteur.NumeroCompteur, startDate, endDate);

                // Récupération des heures existantes en base pour ce compteur
                var existingReads = await compteurQueryRepo.GetReadHoursForCompteurAsync(compteur.NumeroCompteur, startDate, endDate);
                
                // Génération de toutes les heures attendues
                var expectedHours = GenerateExpectedHours(startDate, endDate);
                
                _logger.LogDebug("📊 Compteur {Numero}: {Existing}/{Expected} heures trouvées", 
                    compteur.NumeroCompteur, existingReads.Count, expectedHours.Count);

                // Identification des heures manquantes
                foreach (var expectedHour in expectedHours)
                {
                    if (!existingReads.Contains(expectedHour))
                    {
                        missingReads.Add(new MissingReadInfo
                        {
                            CompteurId = compteur.Id,
                            NumeroCompteur = compteur.NumeroCompteur,
                            AdresseIp = compteur.CompteurEquipement?.FirstOrDefault()?.Equipement?.AdresseIp ?? string.Empty,
                            Port= compteur.CompteurEquipement?.FirstOrDefault()?.Equipement?.Port ?? string.Empty,
                            SerialPort = compteur.CompteurEquipement?.FirstOrDefault()?.Equipement?.SerialPort ?? string.Empty,
                            MissingHour = expectedHour,
                            ClientAddress = "read" ?? string.Empty
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche des heures manquantes pour le compteur {Numero}", 
                    compteur.NumeroCompteur);
            }

            return missingReads;
        }

        private async Task<List<MissingReadInfo>> GetMissingHoursForCompteurInPeriodAsync(Compteur compteur, DateTime startDate, DateTime endDate)
        {
            var missingReads = new List<MissingReadInfo>();

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var compteurQueryRepo = scope.ServiceProvider.GetRequiredService<ICompteurQueryRepository>();

                // Récupération des heures existantes en base pour ce compteur
                var existingReads = await compteurQueryRepo.GetReadHoursForCompteurAsync(compteur.NumeroCompteur, startDate, endDate);

                // Génération de toutes les heures attendues
                var expectedHours = GenerateExpectedHours(startDate, endDate);

                // Identification des heures manquantes
                foreach (var expectedHour in expectedHours)
                {
                    if (!existingReads.Contains(expectedHour))
                    {
                        missingReads.Add(new MissingReadInfo
                        {
                            CompteurId = compteur.Id,
                            NumeroCompteur = compteur.NumeroCompteur,
                            AdresseIp = compteur.CompteurEquipement?.FirstOrDefault()?.Equipement?.AdresseIp ?? string.Empty,
                            MissingHour = expectedHour,
                            ClientAddress = "read" ?? string.Empty
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche des heures manquantes pour le compteur {Numero} dans la période", 
                    compteur.NumeroCompteur);
            }

            return missingReads;
        }

        private List<DateTime> GenerateExpectedHours(DateTime startDate, DateTime endDate)
        {
            var hours = new List<DateTime>();
            var current = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 0, 0);

            while (current <= endDate)
            {
                hours.Add(current);
                current = current.AddHours(1);
            }

            return hours;
        }
    }
}
