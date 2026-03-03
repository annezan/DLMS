using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_DAL.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CommandeCompteurDomainDal.Repositories
{
    public class CommandeCompteurCommandRepository : CommandRepository<CommandeCompteur>, ICommandeCompteurCommandRepository
    {
        public CommandeCompteurCommandRepository(DLMSDBContext context)
            : base(context)
        {
        }

        public async Task<CommandeCompteur> AddCommandeCompteur(CommandeCompteur CommandeCompteur)
        {
            try
            {
                var commandecompteur = _context.CommandeCompteur.AsNoTracking().FirstOrDefault(x => x.CommandeId == CommandeCompteur.CommandeId && x.CompteurId== CommandeCompteur.CompteurId && x.Libellegroupe== CommandeCompteur.Libellegroupe && x.IsArchive == false);
                if (commandecompteur == null)
                {
                    CommandeCompteur.CreatedAt = DateTime.Now;
                    _context.CommandeCompteur.Add(CommandeCompteur);
                    await _context.SaveChangesAsync();
                    return CommandeCompteur;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CommandeCompteur> EditCommandeCompteur(CommandeCompteur CommandeCompteur)
        {
            try
            {
                var CommandeCompteur_update = _context.CommandeCompteur.AsNoTracking().FirstOrDefault(x => x.CommandeId == CommandeCompteur.CommandeId && x.CompteurId == CommandeCompteur.CompteurId && x.Libellegroupe == CommandeCompteur.Libellegroupe && x.IsArchive == false);
                if (CommandeCompteur_update != null)
                {
                    // Mise à jour des champs généraux (les résultats sont maintenant dans ResultatCommandeCompteur)
                    CommandeCompteur_update.Libellegroupe = CommandeCompteur.Libellegroupe;
                    CommandeCompteur_update.UpdatedBy = "Admin";
                    CommandeCompteur_update.UpdatedAt = DateTime.Now;
                    _context.CommandeCompteur.Update(CommandeCompteur_update);
                    await _context.SaveChangesAsync();
                }
                return CommandeCompteur_update;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCommandeCompteur(CommandeCompteur CommandeCompteur)
        {
            try
            {
                if (CommandeCompteur.CommandeId != 0 && CommandeCompteur.Id == 0)
                {
                    var CommandeCompteur_update_list = _context.CommandeCompteur.AsNoTracking().Where(x => x.CommandeId == CommandeCompteur.CommandeId && x.IsArchive == false).ToList();
                    if (CommandeCompteur_update_list != null)
                    {
                        foreach (var CommandeCompteur_update in CommandeCompteur_update_list)
                        {
                            CommandeCompteur_update.DeletedBy = CommandeCompteur.DeletedBy;
                            CommandeCompteur_update.DeletedAt = DateTime.Now;
                            CommandeCompteur_update.IsArchive = true;
                            _context.CommandeCompteur.Update(CommandeCompteur_update);
                        }
                        await _context.SaveChangesAsync();
                    }
                    return true;
                }
                else if (CommandeCompteur.CommandeId == 0 && CommandeCompteur.Id != 0)
                {
                    var CommandeCompteur_update = _context.CommandeCompteur.AsNoTracking().FirstOrDefault(x => x.Id == CommandeCompteur.Id && x.IsArchive == false);
                    if (CommandeCompteur_update != null)
                    {
                        CommandeCompteur_update.NumeroTentative =CommandeCompteur.NumeroTentative== CommandeCompteur_update.NumeroTentative ? CommandeCompteur_update.NumeroTentative: CommandeCompteur.NumeroTentative;
                        CommandeCompteur_update.DeletedBy = CommandeCompteur.DeletedBy;
                        CommandeCompteur_update.DeletedAt = DateTime.Now;
                        CommandeCompteur_update.IsArchive = true;
                        _context.CommandeCompteur.Update(CommandeCompteur_update);               
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
