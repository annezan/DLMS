using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using DLMS_DAL.CelluleDomainDal.Repositories.Commands;
using DLMS_MODELS.CelluleDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CelluleDomainDal.Repositories
{
    public class CelluleCommandRepository : CommandRepository<Cellule>, ICelluleCommandRepository
    {
        public CelluleCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
        public async Task<Cellule> AddCellule(Cellule cellule)
        {
            try
            {
               // var existingCellule = _context.Cellule.AsNoTracking().FirstOrDefault(x => x.IsArchive == false && x.Libelle == cellule.Libelle);
                //if (existingCellule == null)
                // {
                    cellule.CreatedAt = DateTime.Now;
                    cellule.IsArchive = false;
                    _context.Cellule.Add(cellule);
                    await _context.SaveChangesAsync();
                    return cellule;
                //}
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Cellule> EditCellule(Cellule cellule)
        {
            try
            {
                var cellule_update = _context.Cellule.AsNoTracking().FirstOrDefault(x => x.Id == cellule.Id);
                if (cellule_update != null)
                {
                    cellule_update.Type = cellule_update.Type == cellule.Type ? cellule_update.Type : cellule.Type;
                    cellule_update.ValeurTension = cellule_update.ValeurTension == cellule.ValeurTension ? cellule_update.ValeurTension : cellule.ValeurTension;
                    cellule_update.Libelle = cellule_update.Libelle == cellule.Libelle ? cellule_update.Libelle : cellule.Libelle;
                    cellule_update.Adresse = cellule_update.Adresse == cellule.Adresse ? cellule_update.Adresse : cellule.Adresse;
                    cellule_update.PosteId = cellule_update.PosteId == cellule.PosteId ? cellule_update.PosteId : cellule.PosteId;
                    cellule_update.UpdatedAt = DateTime.Now;
                    cellule_update.UpdatedBy = cellule_update.UpdatedBy == cellule.UpdatedBy ? cellule_update.UpdatedBy : cellule.UpdatedBy;
                    cellule_update.IsArchive = cellule_update.IsArchive == cellule.IsArchive ? cellule_update.IsArchive : cellule.IsArchive;
                    _context.Cellule.Update(cellule_update);
                    await _context.SaveChangesAsync();

                    return cellule_update;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Cellule> DeleteCellule(Cellule cellule)
        {
            try
            {
                var cellule_delete = _context.Cellule.AsNoTracking().FirstOrDefault(x => x.Id == cellule.Id);
                if(cellule_delete != null)
                {
                    cellule_delete.DeletedAt = DateTime.Now;
                    cellule_delete.DeletedBy = cellule_delete.DeletedBy == cellule.DeletedBy ? cellule_delete.DeletedBy : cellule.DeletedBy;
                    cellule_delete.IsArchive = true;
                    _context.Cellule.Update(cellule_delete);
                    
                    // Archiver les CompteurCellule associés
                    var compteursCellule = await _context.CompteurCellule
                        .Where(cc => cc.CelluleId == cellule.Id && !cc.IsArchive)
                        .ToListAsync();
                    
                    foreach (var cc in compteursCellule)
                    {
                        cc.IsArchive = true;
                        cc.DeletedAt = DateTime.Now;
                        cc.DeletedBy = cellule.DeletedBy;
                        _context.CompteurCellule.Update(cc);
                    }
                    
                    await _context.SaveChangesAsync();

                    return cellule_delete;
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

