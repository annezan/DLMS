using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CompteurCelluleDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.CompteurCelluleDomainDal.Repositories.Commands
{
    public class CompteurCelluleCommandRepository : CommandRepository<CompteurCellule>, ICompteurCelluleCommandRepository
    {
        public CompteurCelluleCommandRepository(DLMSDBContext context)
            : base(context)
        {
        }

        public async Task<CompteurCellule> GetByIdsAsync(int compteurId, int celluleId)
        {
            return await _context.CompteurCellule
                .FirstOrDefaultAsync(cc => cc.CompteurId == compteurId && cc.CelluleId == celluleId && cc.IsArchive==false);
        }

        public async Task<List<CompteurCellule>> AddCompteurCelluleList(List<CompteurCellule> compteurCellules)
        {
            try
            {
                if (compteurCellules.Count > 0)
                {
                    foreach (var item in compteurCellules)
                    {
                        var compteurCellule = _context.CompteurCellule.AsNoTracking()
                            .FirstOrDefault(x => x.CompteurId == item.CompteurId && x.CelluleId == item.CelluleId);

                        if (compteurCellule == null)
                        {
                            item.CreatedAt = DateTime.UtcNow;
                            _context.CompteurCellule.Add(item);
                        }
                        else if (compteurCellule != null && compteurCellule.IsArchive == true)
                        {
                            compteurCellule.CreatedAt = DateTime.UtcNow;
                            compteurCellule.IsArchive = false;
                            compteurCellule.DeletedAt = null;
                            compteurCellule.DeletedBy = "";
                            _context.CompteurCellule.Update(compteurCellule);
                        }
                    }
                    await _context.SaveChangesAsync();
                    return compteurCellules;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CompteurCellule>> DeleteCompteurCelluleList(List<CompteurCellule> compteurCellules)
        {
            try
            {
                if (compteurCellules.Count > 0)
                {
                    foreach (var item in compteurCellules)
                    {
                        var compteurCellule_delete = _context.CompteurCellule.AsNoTracking()
                            .FirstOrDefault(x => x.CompteurId == item.CompteurId && x.CelluleId == item.CelluleId && x.IsArchive == false);

                        if (compteurCellule_delete != null)
                        {
                            compteurCellule_delete.DeletedAt = DateTime.UtcNow;
                            compteurCellule_delete.DeletedBy = item.DeletedBy;
                            compteurCellule_delete.IsArchive = true;
                            _context.CompteurCellule.Update(compteurCellule_delete);
                        }
                    }
                    await _context.SaveChangesAsync();
                    return compteurCellules;
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
