using DLMS_MODELS;
using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DLMS_SERVICE.Services
{
    public interface IDLMSCommandProcessorService
    {
        Task<List<ActiveCommandInfo>> GetActiveCommandsAsync();
        Task ProcessCommandResultAsync(int commandId, bool success, string result);
        Task UpdateCommandStatusAsync(int commandId, string status, int retryCount = 0);
        Task<List<ActiveCommandInfo>> GetFailedCommandsForRetryAsync();
    }

    public class DLMSCommandProcessorService : IDLMSCommandProcessorService
    {
        private readonly ILogger<DLMSCommandProcessorService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly int _maxRetryCount = 3;

        public DLMSCommandProcessorService(
            ILogger<DLMSCommandProcessorService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<List<ActiveCommandInfo>> GetActiveCommandsAsync()
        {
            try
            {
                _logger.LogDebug("Recherche des commandes actives");

                using var scope = _serviceProvider.CreateScope();
                var commandeQueryRepo = scope.ServiceProvider.GetRequiredService<ICommandeQueryRepository>();

                // Récupération des commandes avec statut "Active"
                var activeCommands = await commandeQueryRepo.GetActiveCommandsAsync();
                _logger.LogInformation("📊 Nombre de commandes trouvées dans la query: {Count}", activeCommands.Count);
                
                var commandInfos = new List<ActiveCommandInfo>();

                foreach (var command in activeCommands)
                {
                    _logger.LogInformation("🔍 Commande {CommandId} - Statut: {Statut} - Nombre de CommandeCompteur: {Count}", 
                        command.Id, command.Statut, command.CommandeCompteur?.Count ?? 0);
                    
                    // Une commande peut avoir plusieurs CommandeCompteur
                    foreach (var commandeCompteur in command.CommandeCompteur)
                    {
                        _logger.LogInformation("📋 Traitement CommandeCompteur {Id} pour Compteur {CompteurId}", 
                            commandeCompteur.Id, commandeCompteur.CompteurId);
                        
                        var compteurEquipement = commandeCompteur.Compteur?.CompteurEquipement?.FirstOrDefault();
                        
                        // Récupérer les heures dans la plage si Datedebut et Datefin sont définis
                        List<DateTime> heures = new List<DateTime>();
                        if (command.Datedebut != null && command.Datefin != null)
                        {
                            heures = GetHeuresDansPlage(command.Datedebut.Value, command.Datefin.Value);
                        }
                        
                        commandInfos.Add(new ActiveCommandInfo
                        {
                            CommandId = command.Id,
                            CompteurId = commandeCompteur.CompteurId,
                            CommandeCompteurId = commandeCompteur.Id,
                            NumeroCompteur = commandeCompteur.Compteur?.NumeroCompteur ?? string.Empty,
                            AdresseIp = compteurEquipement?.Equipement?.AdresseIp ?? string.Empty,
                            Port= compteurEquipement?.Equipement?.Port ?? string.Empty,
                            SerialPort= compteurEquipement?.Equipement?.SerialPort ?? string.Empty,
                            CommandType = command.Typecommande?.Libelletype ?? string.Empty,
                            Numeroprofile = command.Numeroprofile ?? 0,
                            Nombreentree= command.Nombreentree ?? 0,
                            Heures = heures,
                            CreatedAt = command.CreatedAt ?? DateTime.Now,
                            RetryCount = commandeCompteur.NumeroTentative
                        });
                    }
                }

                if (commandInfos.Count > 0)
                {
                    _logger.LogInformation("⚡ {Count} commandes actives trouvées", commandInfos.Count);
                }

                return commandInfos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche des commandes actives");
                return new List<ActiveCommandInfo>();
            }
        }

        public async Task ProcessCommandResultAsync(int commandId, bool success, string result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var commandeCommandRepo = scope.ServiceProvider.GetRequiredService<ICommandeCommandRepository>();

                if (success)
                {
                    // Marquer la commande comme complétée avec succès
                    await UpdateCommandStatusAsync(commandId, "Completed");
                    
                    _logger.LogInformation("✅ Commande {CommandId} complétée avec succès", commandId);
                }
                else
                {
                    // Récupérer tous les CommandeCompteur pour cette commande
                    var commandeQueryRepo = scope.ServiceProvider.GetRequiredService<ICommandeQueryRepository>();
                    var commandeCompteurs = await commandeQueryRepo.GetCommandeCompteursByCommandIdAsync(commandId);
                    
                    foreach (var commandeCompteur in commandeCompteurs)
                    {
                        var newRetryCount = commandeCompteur.NumeroTentative + 1;
                        
                        if (newRetryCount >= _maxRetryCount)
                        {
                            await UpdateCommandeCompteurStatusAsync(commandeCompteur.Id, "Failed", newRetryCount);
                            _logger.LogWarning("❌ CommandeCompteur {CommandeCompteurId} marqué comme Failed après {RetryCount} tentatives", 
                                commandeCompteur.Id, newRetryCount);
                        }
                        else
                        {
                            await UpdateCommandeCompteurStatusAsync(commandeCompteur.Id, "Active", newRetryCount);
                            _logger.LogInformation("🔄 CommandeCompteur {CommandeCompteurId} réactivé pour retry ({RetryCount}/{MaxRetry})", 
                                commandeCompteur.Id, newRetryCount, _maxRetryCount);
                        }
                    }
                }

                // Sauvegarder le résultat de la commande
                await SaveCommandResultAsync(commandId, success, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement du résultat de la commande {CommandId}", commandId);
                throw;
            }
        }

        public async Task UpdateCommandStatusAsync(int commandId, string status, int retryCount = 0)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var commandeCommandRepo = scope.ServiceProvider.GetRequiredService<ICommandeCommandRepository>();

                await commandeCommandRepo.UpdateCommandStatusAsync(commandId, status, retryCount);
                
                _logger.LogDebug("Statut de la commande {CommandId} mis à jour: {Status}, Retry: {Retry}", 
                    commandId, status, retryCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du statut de la commande {CommandId}", commandId);
                throw;
            }
        }

        public async Task<List<ActiveCommandInfo>> GetFailedCommandsForRetryAsync()
        {
            try
            {
                _logger.LogDebug("Recherche des commandes failed pour retry");

                using var scope = _serviceProvider.CreateScope();
                var commandeQueryRepo = scope.ServiceProvider.GetRequiredService<ICommandeQueryRepository>();

                // Récupération des commandes avec statut "Failed" et retry count < max
                var failedCommands = await commandeQueryRepo.GetFailedCommandsForRetryAsync(_maxRetryCount);
                var commandInfos = new List<ActiveCommandInfo>();

                foreach (var command in failedCommands)
                {
                    // Une commande peut avoir plusieurs CommandeCompteur
                    foreach (var commandeCompteur in command.CommandeCompteur)
                    {
                        var compteurEquipement = commandeCompteur.Compteur?.CompteurEquipement?.FirstOrDefault();
                        commandInfos.Add(new ActiveCommandInfo
                        {
                            CommandId = command.Id,
                            CompteurId = commandeCompteur.CompteurId,
                            CommandeCompteurId = commandeCompteur.Id,
                            NumeroCompteur = commandeCompteur.Compteur?.NumeroCompteur ?? string.Empty,
                            AdresseIp = compteurEquipement?.Equipement?.AdresseIp ?? string.Empty,
                            CommandType = command.Typecommande?.Libelletype ?? string.Empty,
                            CreatedAt = command.CreatedAt ?? DateTime.Now,
                            RetryCount = commandeCompteur.NumeroTentative
                        });
                    }
                }

                if (commandInfos.Count > 0)
                {
                    _logger.LogInformation("🔄 {Count} commandes failed éligibles au retry", commandInfos.Count);
                }

                return commandInfos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche des commandes failed pour retry");
                return new List<ActiveCommandInfo>();
            }
        }

        private async Task UpdateCommandeCompteurStatusAsync(int commandeCompteurId, string status, int retryCount)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var commandeCommandRepo = scope.ServiceProvider.GetRequiredService<ICommandeCommandRepository>();

                await commandeCommandRepo.UpdateCommandeCompteurStatusAsync(commandeCompteurId, status, retryCount);
                
                _logger.LogDebug("Statut du CommandeCompteur {CommandeCompteurId} mis à jour: {Status}, Retry: {Retry}", 
                    commandeCompteurId, status, retryCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du statut du CommandeCompteur {CommandeCompteurId}", commandeCompteurId);
                throw;
            }
        }

        private async Task SaveCommandResultAsync(int commandId, bool success, string result)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var commandeCommandRepo = scope.ServiceProvider.GetRequiredService<ICommandeCommandRepository>();

                // Sauvegarde du résultat dans la table CommandResult (ou équivalent)
                await commandeCommandRepo.SaveCommandResultAsync(commandId, success, result);
                
                _logger.LogDebug("Résultat de la commande {CommandId} sauvegardé: Success={Success}", 
                    commandId, success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde du résultat de la commande {CommandId}", commandId);
                // Ne pas throw pour ne pas casser le flux principal
            }
        }

        private List<DateTime> GetHeuresDansPlage(DateTime datedebut, DateTime datefin)
        {
            var heures = new List<DateTime>();
            var current = datedebut;
            
            while (current <= datefin)
            {
                heures.Add(current);
                current = current.AddHours(1);
            }
            
            return heures;
        }
    }
}
