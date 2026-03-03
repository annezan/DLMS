using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using DLMS_DAL.FabricantDomainDal.Repositories.Commands;
using DLMS_MODELS.FabricantDomain.Entities;
using Microsoft.EntityFrameworkCore;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_MODELS.PosteDomain.Entities;

namespace DLMS_DAL.FabricantDomainDal.Repositories
{
    public class FabricantCommandRepository : CommandRepository<Fabricant>, IFabricantCommandRepository
    {
        public FabricantCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }

        public async Task<Fabricant> AddFabricant(Fabricant Fabricant)
        {
            try
            {
                var fabricant = _context.Fabricants.AsNoTracking().FirstOrDefault(x => x.Libelle == Fabricant.Libelle);
                if (fabricant == null)
                {
                    Fabricant.CreatedAt = DateTime.Now;
                    Fabricant.IsArchive = false;
                    _context.Fabricants.Add(Fabricant);
                    await _context.SaveChangesAsync();
                    return Fabricant;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Fabricant> EditFabricant(Fabricant Fabricant)
        {
            try
            {
                var Fabricant_update = _context.Fabricants.AsNoTracking().FirstOrDefault(x => x.Id == Fabricant.Id);
                if (Fabricant_update != null)
                {
                    Fabricant_update.Libelle = Fabricant_update.Libelle == Fabricant.Libelle ? Fabricant_update.Libelle : Fabricant.Libelle;
                    Fabricant_update.UpdatedAt = DateTime.Now;
                    Fabricant_update.UpdatedBy = Fabricant_update.UpdatedBy == Fabricant.UpdatedBy ? Fabricant_update.UpdatedBy : Fabricant.UpdatedBy;

                    _context.Fabricants.Update(Fabricant_update);
                    await _context.SaveChangesAsync();

                    return Fabricant_update;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Fabricant> DeleteFabricant(Fabricant Fabricant)
        {
            try
            {
                var Fabricant_delete = _context.Fabricants.AsNoTracking().FirstOrDefault(x => x.Id == Fabricant.Id);
                if(Fabricant_delete != null)
                {
                    Fabricant_delete.IsArchive = true;
                    Fabricant_delete.DeletedAt = DateTime.Now;
                    Fabricant_delete.DeletedBy = Fabricant_delete.DeletedBy == Fabricant.DeletedBy ? Fabricant_delete.DeletedBy : Fabricant.DeletedBy;

                    _context.Fabricants.Update(Fabricant_delete);
                    await _context.SaveChangesAsync();

                    return Fabricant_delete;
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
