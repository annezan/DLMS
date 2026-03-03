using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using DLMS_DAL.PosteDomainDal.Repositories.Commands;
using DLMS_MODELS.PosteDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.PosteDomainDal.Repositories
{
    public class PosteCommandRepository : CommandRepository<Poste>, IPosteCommandRepository
    {
        public PosteCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
        public async Task<Poste> AddPoste(Poste Poste)
        {
            try
            {
                var poste = _context.Poste.AsNoTracking().FirstOrDefault(x => x.Id == Poste.Id);
                if (poste == null)
                {
                    Poste.CreatedAt= DateTime.Now;
                    Poste.IsArchive = false;
                    _context.Poste.Add(Poste);
                    await _context.SaveChangesAsync();
                    return Poste;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Poste> EditPoste(Poste Poste)
        {
            try
            {
                var Poste_update = _context.Poste.AsNoTracking().FirstOrDefault(x => x.Id == Poste.Id);
                if (Poste_update != null)
                {
                    Poste_update.Numero = Poste_update.Numero == Poste.Numero ? Poste_update.Numero : Poste.Numero;
                    Poste_update.Libelle = Poste_update.Libelle == Poste.Libelle ? Poste_update.Libelle : Poste.Libelle;
                    Poste_update.Adresse = Poste_update.Adresse == Poste.Adresse ? Poste_update.Adresse : Poste.Adresse;
                    Poste_update.UpdatedAt = DateTime.Now;
                    Poste_update.UpdatedBy = Poste_update.UpdatedBy == Poste.UpdatedBy ? Poste_update.UpdatedBy : Poste.UpdatedBy;
                    Poste_update.IsArchive = Poste_update.IsArchive == Poste.IsArchive ? Poste_update.IsArchive : Poste.IsArchive;
                    _context.Poste.Update(Poste_update);
                    await _context.SaveChangesAsync();

                    return Poste_update;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Poste> DeletePoste(Poste Poste)
        {
            try
            {
                var Poste_delete = _context.Poste.AsNoTracking().FirstOrDefault(x => x.Id == Poste.Id);
                if(Poste_delete != null)
                {
                    Poste_delete.DeletedAt = DateTime.Now;
                    Poste_delete.DeletedBy = Poste_delete.DeletedBy == Poste.DeletedBy ? Poste_delete.DeletedBy : Poste.DeletedBy;

                    Poste_delete.IsArchive = true;

                    _context.Poste.Update(Poste_delete);
                    await _context.SaveChangesAsync();

                    return Poste_delete;
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
