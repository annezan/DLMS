using DLMS_DAL.Bases;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using DLMS_COMMUNICATION;
using DLMS_COMMUNICATION.Reader;
using Gurux.DLMS.Objects;
using Newtonsoft.Json;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.CodeObisDomain.Entities;
using Gurux.DLMS;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using Gurux.DLMS.Enums;
using Newtonsoft.Json.Linq;
using DLMS_DAL.GxdlmsprofilgenericdetailDomainDal.Repositories;
using DLMS_DAL.CommandeDomainDal.Repositories.Queries;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CommandeDomainDal.Repositories.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Entities;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class ReadObjectCommandeQueryRepository : QueryBaseRepository<AssociationKey>, IReadObjectCommandeQueryRepository
    {
        private readonly ICommandeQueryRepository _ICommandeQueryRepository;
        private readonly IResultatCommandeCompteurCommandRepository _IResultatCommandeCompteurCommandRepository;
        private readonly IGxdlmsprofilgenericQueryRepository _IGxdlmsprofilgenericQueryRepository;
        private readonly ICommandeCompteurCommandRepository _ICommandeCompteurCommandRepository;
        private readonly ICommandeCommandRepository _ICommandeCommandRepository;
        public ReadObjectCommandeQueryRepository(
            DLMSDBContext context,
            ICommandeQueryRepository ICommandeQueryRepository,
            IResultatCommandeCompteurCommandRepository IResultatCommandeCompteurCommandRepository,
            IGxdlmsprofilgenericQueryRepository IGxdlmsprofilgenericQueryRepository,
            ICommandeCompteurCommandRepository ICommandeCompteurCommandRepository,
            ICommandeCommandRepository ICommandeCommandRepository)
        :
            base(context)
        {
            _ICommandeQueryRepository = ICommandeQueryRepository;
            _IResultatCommandeCompteurCommandRepository = IResultatCommandeCompteurCommandRepository;
            _IGxdlmsprofilgenericQueryRepository = IGxdlmsprofilgenericQueryRepository;
            _ICommandeCompteurCommandRepository = ICommandeCompteurCommandRepository;
            _ICommandeCommandRepository = ICommandeCommandRepository;
        }


        public async Task<string> GetReadObjectCommande(int commandeId)
        {
            try
            {
                // Récupérer la commande spécifique
                var commande = await _ICommandeQueryRepository.GetCommandeById(commandeId);
                if (commande == null)
                {
                    Console.WriteLine($"Commande {commandeId} non trouvée");
                    return "Commande non trouvée";
                }

                // Récupérer tous les CommandeCompteur non archivés
                var commandeCompteurs = commande.CommandeCompteur
                    .Where(x => x.IsArchive == false)
                    .ToList();
                
                if (!commandeCompteurs.Any())
                {
                    Console.WriteLine($"Aucun CommandeCompteur trouvé pour la commande {commandeId}");
                    return "Aucun CommandeCompteur trouvé";
                }

                var result = "";
                
                // Traiter chaque CommandeCompteur non archivé
                foreach (var commandeCompteur in commandeCompteurs)
                {
                    try
                    {
                        // Récupérer les informations du compteur
                        var compteur = commandeCompteur.Compteur;
                        var serialNumber = compteur.NumeroCompteur;

                        // Récupérer les clés DLMS depuis la base de données
                        var authentication = await _context.AssociationKeys
                            .Where(x => x.Type == "read" && x.Keyname == "authentication" && x.CompteurId.Contains(serialNumber))
                            .FirstOrDefaultAsync();
                        var unicast = await _context.AssociationKeys
                            .Where(x => x.Type == "read" && x.Keyname == "unicast" && x.CompteurId.Contains(serialNumber))
                            .FirstOrDefaultAsync();

                        if (authentication == null || unicast == null)
                        {
                            Console.WriteLine($"Clés DLMS non trouvées pour le compteur {serialNumber}");
                            continue;
                        }

                        var AuthenticationKey = Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS");
                        var UnicastKey = Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS");
                        
                        // Déterminer le profil en fonction du numéro de profil
                        var profil = commande.Numeroprofile switch
                        {
                            2 => "1.0.99.2.0.255",
                            3 => "1.0.99.3.0.255",
                            _ => "1.0.99.1.0.255"
                        };

                        // 1) Si la commande est déjà expirée, on archive ce CommandeCompteur sans le traiter
                        if (commande.Dateexp != null && commande.Dateexp <= DateTime.Now)
                        {
                            // Calcul du numéro de tentative pour ce CommandeCompteur
                            var currentTentative = (commandeCompteur.NumeroTentative) + 1;

                            string currentResult;
                            
                            // Récupérer les paramètres de communication depuis les équipements du compteur
                            var compteurEquipement = await _context.CompteurEquipement
                                .Include(ce => ce.Equipement)
                                .Where(x => x.CompteurId == compteur.Id && x.IsArchive==false)
                                .FirstOrDefaultAsync();
                            
                            if (compteurEquipement == null)
                            {
                                Console.WriteLine($"Aucun équipement trouvé pour le compteur {serialNumber}");
                                continue;
                            }
                            
                            var equipement = compteurEquipement.Equipement;
                            var port = equipement.Port == "0" || equipement.Port == "null" ? null : equipement.Port;
                            var serialport = equipement.SerialPort=="0" || equipement.SerialPort == "null" ? null: equipement.SerialPort;
                            var addressIp = equipement.AdresseIp;
                            var clientAddress = "read"; // Utiliser la même adresse IP
                            var interfaceType = "HDLC";

                            if (commande.Nombreentree != null && commande.Nombreentree != 0)
                            {
                                long count = 0;
                                continue;
                            }
                            else
                            {
                                DateTime datestart = new DateTime(commande.Datedebut.Value.Year, commande.Datedebut.Value.Month, commande.Datedebut.Value.Day, commande.Datedebut.Value.Hour, 0, 0);
                                DateTime dateend = new DateTime(commande.Datefin.Value.Year, commande.Datefin.Value.Month, commande.Datefin.Value.Day, commande.Datefin.Value.Hour, 0, 0);
                                currentResult = ReaderCommunication.ReadRowsByRange(datestart.ToString(), dateend.ToString(), port, serialport, addressIp, clientAddress, serialNumber, interfaceType, authentication.Pwd, AuthenticationKey, UnicastKey, profil + ":2");
                            }

                            // Désérialisation en liste d'objets
                            var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(currentResult);

                            // Si la commande n'a retourné aucune donnée, on ne l'archive pas, on met juste à jour le NumeroTentative
                            if (entries == null || entries.Count == 0)
                            {
                                // Mettre à jour le NumeroTentative même si aucune donnée retournée
                                var commandeCompteurToUpdate = new CommandeCompteur
                                {
                                    Id = commandeCompteur.Id,
                                    NumeroTentative = currentTentative
                                };
                                await _ICommandeCompteurCommandRepository.EditCommandeCompteur(commandeCompteurToUpdate);
                                Console.WriteLine($"CommandeCompteur {commandeCompteur.Id} mis à jour avec tentative {currentTentative} (aucune donnée retournée)");
                                Console.WriteLine($"Aucune donnée à traiter pour le compteur {serialNumber}.");
                                continue;
                            }

                            var profilgeneric = await _IGxdlmsprofilgenericQueryRepository.GetProfilgenericByLN(profil);
                            if (profilgeneric == null)
                            {
                                continue;
                            }

                            foreach (var entry in entries)
                            {
                                Console.WriteLine("📌 Nouvelle entrée :");

                                // Parcourir les lignes de données
                                foreach (var row in entry.Key)
                                {
                                    Console.WriteLine($"  - Donnée : {row}");
                                    DateTime dateUtc = DateTime.Now;

                                    if (row is IEnumerable<object> values)
                                    {
                                        for (int i = 0; i < entry.Value.Length; i++)
                                        {
                                            var array = values.ToArray();
                                            var obj = entry.Value[i];
                                            
                                            ResultatCommandeCompteur resultatCommande = new ResultatCommandeCompteur();
                                            
                                            try
                                            {
                                                if (i == 0)
                                                {
                                                    long unixTimestamp = Convert.ToUInt32(array[i]);
                                                    dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                                }
                                                
                                                var realValue = array[i];
                                                bool isRegisterValue = realValue is decimal || realValue is int || realValue is long;
                                                
                                                resultatCommande.Value = isRegisterValue ? Convert.ToDecimal(realValue).ToString() : realValue?.ToString();

                                                var codeobis = await _context.CodeObis.Where(x => x.Value == obj).FirstOrDefaultAsync();
                                                if (codeobis != null)
                                                {
                                                    resultatCommande.DateEnr = dateUtc;
                                                    resultatCommande.GxdlmsprofilgenericId = profilgeneric.Id;
                                                    resultatCommande.CommandeCompteurId = commandeCompteur.Id;
                                                    resultatCommande.NumeroCompteur = serialNumber;
                                                    resultatCommande.IsArchive = false;
                                                    resultatCommande.CodeObisId = codeobis.Id;
                                                    var addresultat = await _IResultatCommandeCompteurCommandRepository.AddResultatCommandeCompteur(resultatCommande);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.ToString());
                                                // Actaric SL7000 peut retourner une erreur ici. Continuer la lecture.
                                            }
                                        }
                                    }
                                }
                            }

                            // La commande a été traitée et a retourné des entrées : on archive ce CommandeCompteur
                            var commandeCompteurToArchiveAfterEntries = new CommandeCompteur
                            {
                                Id = commandeCompteur.Id,
                                DeletedBy = "System"
                            };
                            await _ICommandeCompteurCommandRepository.DeleteCommandeCompteur(commandeCompteurToArchiveAfterEntries);
                            
                            result = currentResult; // Garder le dernier résultat
                            Console.WriteLine($"CommandeCompteur {commandeCompteur.Id} traité avec succès pour {serialNumber}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors du traitement du CommandeCompteur {commandeCompteur.Id}: {ex.Message}");
                        continue;
                    }
                }
                
                // Après avoir traité tous les CommandeCompteur, on archive la commande elle-même
                var commandeToArchive = new Commande
                {
                    Id = commandeId,
                    DeletedBy = "System"
                };
                await _ICommandeCommandRepository.DeleteCommande(commandeToArchive);
                
                Console.WriteLine($"Tous les CommandeCompteur traités pour la commande {commandeId}");
                return result ?? "Aucune donnée retournée";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du traitement de la commande {commandeId}: {ex.Message}");
                return null;
            }
        }

    }
}
