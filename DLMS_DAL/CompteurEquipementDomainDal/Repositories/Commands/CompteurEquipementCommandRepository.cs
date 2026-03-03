using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_DAL.CompteurEquipementDomainDal.Repositories.Commands;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.CompteurDomain.Entities;

namespace DLMS_DAL.CompteurDomainDal.Repositories
{
    public class CompteurEquipementCommandRepository : CommandRepository<CompteurEquipement>, ICompteurEquipementCommandRepository
    {
        public CompteurEquipementCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
        public async Task<List<CompteurEquipement>> AddCompteurEquipement(List<CompteurEquipement> CompteurEquipement)
        {
            try
            {
                if (CompteurEquipement.Count>0)
                {
                    foreach (var item in CompteurEquipement)
                    {
                        var compteurequipement = _context.CompteurEquipement.AsNoTracking().FirstOrDefault(x => x.CompteurId == item.CompteurId && x.EquipementId == item.EquipementId);

                        if (compteurequipement == null)
                        {
                            item.CreatedAt = DateTime.Now;
                            _context.CompteurEquipement.Add(item);
                            
                        }
                        else if(compteurequipement != null && compteurequipement.IsArchive == true)
                        {
                            compteurequipement.CreatedAt = DateTime.Now;
                            compteurequipement.IsArchive = false;
                            compteurequipement.DeletedAt = null;
                            compteurequipement.DeletedBy = "";
                            _context.CompteurEquipement.Update(compteurequipement);

                        }
                    }
                    await _context.SaveChangesAsync();
                    return CompteurEquipement;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<CompteurEquipement>> DeleteCompteurEquipement(List<CompteurEquipement> CompteurEquipement)
        {
            try
            {
                if (CompteurEquipement.Count > 0)
                {
                    foreach (var item in CompteurEquipement)
                    {
                        var compteurequipement_delete = _context.CompteurEquipement.AsNoTracking().FirstOrDefault(x => x.CompteurId == item.CompteurId && x.EquipementId == item.EquipementId && x.IsArchive == false);

                        if (compteurequipement_delete != null)
                        {
                            compteurequipement_delete.DeletedAt = DateTime.Now;
                            compteurequipement_delete.DeletedBy = item.DeletedBy;
                            compteurequipement_delete.IsArchive = true;
                            _context.CompteurEquipement.Update(compteurequipement_delete);

                        }
                    }
                    await _context.SaveChangesAsync();
                    return CompteurEquipement;
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
