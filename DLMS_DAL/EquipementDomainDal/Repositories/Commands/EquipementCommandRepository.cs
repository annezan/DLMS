using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using DLMS_DAL.EquipementDomainDal.Repositories.Commands;
using DLMS_MODELS.EquipementDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.EquipementDomainDal.Repositories
{
    public class EquipementCommandRepository : CommandRepository<Equipement>, IEquipementCommandRepository
    {
        public EquipementCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
        public async Task<Equipement> AddEquipement(Equipement Equipement)
        {
            try
            {
                var equipement = _context.Equipement.AsNoTracking().FirstOrDefault(x => x.NumeroSerie == Equipement.NumeroSerie);
                if (equipement == null)
                {
                    Equipement.CreatedAt=DateTime.Now;
                    Equipement.IsArchive = false;
                    _context.Equipement.Add(Equipement);
                    await _context.SaveChangesAsync();
                    return Equipement;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Equipement> EditEquipement(Equipement Equipement)
        {
            try
            {
                var Equipement_update = _context.Equipement.AsNoTracking().FirstOrDefault(x => x.Id == Equipement.Id);
                if(Equipement_update != null)
                {
                    Equipement_update.NumeroSerie = Equipement_update.NumeroSerie == Equipement.NumeroSerie ? Equipement_update.NumeroSerie : Equipement.NumeroSerie;
                    Equipement_update.Libelle = Equipement_update.Libelle == Equipement.Libelle ? Equipement_update.Libelle : Equipement.Libelle;
                    Equipement_update.AdresseIp = Equipement_update.AdresseIp == Equipement.AdresseIp ? Equipement_update.AdresseIp : Equipement.AdresseIp;
                    Equipement_update.Type = Equipement_update.Type == Equipement.Type ? Equipement_update.Type : Equipement.Type;
                    Equipement_update.Marque = Equipement_update.Marque == Equipement.Marque ? Equipement_update.Marque : Equipement.Marque;
                    Equipement_update.Port = Equipement_update.Port == Equipement.Port ? Equipement_update.Port : Equipement.Port;
                    Equipement_update.DatePremierePose = Equipement_update.DatePremierePose == Equipement.DatePremierePose ? Equipement_update.DatePremierePose : Equipement.DatePremierePose;
                    Equipement_update.DatePoseActuelle = Equipement_update.DatePoseActuelle == Equipement.DatePoseActuelle ? Equipement_update.DatePoseActuelle : Equipement.DatePoseActuelle;
                    Equipement_update.UpdatedAt = DateTime.Now;
                    Equipement_update.UpdatedBy = Equipement_update.UpdatedBy == Equipement.UpdatedBy ? Equipement_update.UpdatedBy : Equipement.UpdatedBy;
                    Equipement_update.IsArchive = Equipement_update.IsArchive == Equipement.IsArchive ? Equipement_update.IsArchive : Equipement.IsArchive;
                    _context.Equipement.Update(Equipement_update);
                    await _context.SaveChangesAsync();

                    return Equipement_update;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Equipement> DeleteEquipement(Equipement Equipement)
        {
            try
            {
                var Equipement_delete = _context.Equipement.AsNoTracking().FirstOrDefault(x => x.Id == Equipement.Id);
                if(Equipement_delete != null)
                {
                    Equipement_delete.DeletedAt = DateTime.Now;
                    Equipement_delete.DeletedBy = Equipement_delete.DeletedBy == Equipement.DeletedBy ? Equipement_delete.DeletedBy : Equipement.DeletedBy;
                    Equipement_delete.IsArchive = true;
                    _context.Equipement.Update(Equipement_delete);
                    
                    // Archiver les CompteurEquipement associés
                    var compteursEquipement = await _context.CompteurEquipement
                        .Where(ce => ce.EquipementId == Equipement.Id && !ce.IsArchive)
                        .ToListAsync();
                    
                    foreach (var ce in compteursEquipement)
                    {
                        ce.IsArchive = true;
                        ce.DeletedAt = DateTime.Now;
                        ce.DeletedBy = Equipement.DeletedBy;
                        _context.CompteurEquipement.Update(ce);
                    }
                    
                    await _context.SaveChangesAsync();

                    return Equipement_delete;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}

