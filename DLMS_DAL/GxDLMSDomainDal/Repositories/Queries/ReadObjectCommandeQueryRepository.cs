using DLMS_DAL.Bases;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using DLMS_COMMUNICATION;
using Newtonsoft.Json;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_MODELS.ServiceContracts;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class ReadObjectCommandeQueryRepository : QueryBaseRepository<AssociationKey>, IReadObjectCommandeQueryRepository
    {
        private readonly ICommandeQueryRepository _commandeQueryRepo;
        private readonly ICommandeCompteurCommandRepository _commandeCompteurCommandRepo;
        private readonly ICommandeCommandRepository _commandeCommandRepo;
        private readonly ICommandExecutor _commandExecutor;

        public ReadObjectCommandeQueryRepository(
            DLMSDBContext context,
            ICommandeQueryRepository commandeQueryRepo,
            ICommandeCompteurCommandRepository commandeCompteurCommandRepo,
            ICommandeCommandRepository commandeCommandRepo,
            ICommandExecutor commandExecutor)
        : base(context)
        {
            _commandeQueryRepo = commandeQueryRepo;
            _commandeCompteurCommandRepo = commandeCompteurCommandRepo;
            _commandeCommandRepo = commandeCommandRepo;
            _commandExecutor = commandExecutor;
        }

        public async Task<string> GetReadObjectCommande(int commandeId)
        {
            try
            {
                var commande = await _commandeQueryRepo.GetCommandeById(commandeId);
                if (commande == null)
                    return "Commande non trouvée";

                var commandeCompteurs = commande.CommandeCompteur
                    .Where(x => x.IsArchive == false)
                    .ToList();

                if (!commandeCompteurs.Any())
                    return "Aucun CommandeCompteur trouvé";

                var lastResult = "";

                foreach (var commandeCompteur in commandeCompteurs)
                {
                    try
                    {
                        var compteur = commandeCompteur.Compteur;
                        var serialNumber = compteur.NumeroCompteur;

                        // Récupérer les clés DLMS
                        var authentication = await _context.AssociationKeys
                            .FirstOrDefaultAsync(x => x.Type == "read" && x.Keyname == "authentication" && x.CompteurId.Contains(serialNumber));
                        var unicast = await _context.AssociationKeys
                            .FirstOrDefaultAsync(x => x.Type == "read" && x.Keyname == "unicast" && x.CompteurId.Contains(serialNumber));

                        if (authentication == null || unicast == null)
                            continue;

                        // Récupérer les paramètres réseau
                        var compteurEquipement = await _context.CompteurEquipement
                            .Include(ce => ce.Equipement)
                            .FirstOrDefaultAsync(x => x.CompteurId == compteur.Id && x.IsArchive == false);

                        if (compteurEquipement == null)
                            continue;

                        var equipement = compteurEquipement.Equipement;

                        // Déterminer le profil
                        var profil = commande.Numeroprofile switch
                        {
                            2 => "1.0.99.2.0.255",
                            3 => "1.0.99.3.0.255",
                            _ => "1.0.99.1.0.255"
                        };

                        // Déterminer le type de commande
                        DlmsCommandType cmdType;
                        if (commande.Nombreentree != null && commande.Nombreentree != 0)
                            cmdType = DlmsCommandType.ReadByEntry;
                        else
                            cmdType = DlmsCommandType.ReadByRange;

                        // Construire la requête
                        var request = new CommandRequest
                        {
                            Type = cmdType,
                            AddressIp = equipement.AdresseIp,
                            Port = (equipement.Port == "0" || equipement.Port == "null") ? null : equipement.Port,
                            SerialNumber = serialNumber,
                            Password = authentication.Pwd,
                            AuthenticationKey = Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS"),
                            UnicastKey = Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS"),
                            ProfileObis = profil,
                            DateStart = commande.Datedebut,
                            DateEnd = commande.Datefin,
                            NombreEntree = commande.Nombreentree ?? 0,
                            CommandeCompteurId = commandeCompteur.Id
                        };

                        // Exécuter via le service (avec verrou IP et conversion scaler)
                        var result = await _commandExecutor.ExecuteCommandAsync(request);

                        if (result.Success)
                        {
                            // Archiver le CommandeCompteur traité
                            await _commandeCompteurCommandRepo.DeleteCommandeCompteur(new CommandeCompteur
                            {
                                Id = commandeCompteur.Id,
                                DeletedBy = "System"
                            });
                            lastResult = result.Data;
                        }
                        else
                        {
                            // Incrémenter le numéro de tentative
                            await _commandeCompteurCommandRepo.EditCommandeCompteur(new CommandeCompteur
                            {
                                Id = commandeCompteur.Id,
                                NumeroTentative = commandeCompteur.NumeroTentative + 1
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erreur CommandeCompteur {commandeCompteur.Id}: {ex.Message}");
                        continue;
                    }
                }

                // Archiver la commande
                await _commandeCommandRepo.DeleteCommande(new Commande
                {
                    Id = commandeId,
                    DeletedBy = "System"
                });

                return lastResult ?? "Aucune donnée retournée";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur commande {commandeId}: {ex.Message}");
                return null;
            }
        }
    }
}
