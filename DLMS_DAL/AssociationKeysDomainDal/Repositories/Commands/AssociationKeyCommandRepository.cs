using DLMS_DAL.AssociationKeyDomainDal.Repositories.Commands;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using DLMS_MODELS.CompteurEquipementDomain.Entities;

namespace DLMS_DAL.AssociationKeyDomainDal.Repositories
{
    public class AssociationKeyCommandRepository : CommandRepository<AssociationKey>, IAssociationKeyCommandRepository
    {
        public AssociationKeyCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
        public async Task<List<AssociationKey>> AddAssociationKey(List<AssociationKey> AssociationKey)
        {

            try
            {
                // Supprime tous les AssociationKeys de la table
                await _context.AssociationKeys.ExecuteDeleteAsync();
                if (AssociationKey.Count > 0)
                {
                    foreach (var item in AssociationKey)
                    {
                        string keyvalueEncrypt = Cryptage.Encrypt(item.Keyvalue, "ASCDLMS");

                        var associationKey = await _context.AssociationKeys.FirstOrDefaultAsync(x => x.Keyvalue == keyvalueEncrypt && x.CompteurId == item.CompteurId);
                        if (associationKey == null)
                        {

                            item.Keyvalue = keyvalueEncrypt;
                            item.CreatedAt = DateTime.Now;
                            _context.AssociationKeys.Add(item);

                            }
                        }
                            await _context.SaveChangesAsync();
                            return AssociationKey;
                        }
                        return null;

            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public async Task<AssociationKey> EditAssociationKey(AssociationKey AssociationKey)
        {
            try
            {
                var AssociationKey_update = _context.AssociationKeys.AsNoTracking().FirstOrDefault(x => x.Id == AssociationKey.Id);
                string keyvalueEncryptold = AssociationKey_update.Keyvalue;
                string keyvalueEncrypt = Cryptage.Encrypt(AssociationKey.Keyvalue, "ASCDLMS");


                //AssociationKey_update.CompteurId = AssociationKey_update.CompteurId == AssociationKey.CompteurId ? AssociationKey_update.CompteurId : AssociationKey.CompteurId;
                //AssociationKey_update.Type = AssociationKey_update.Type == AssociationKey.Type ? AssociationKey_update.Type : AssociationKey.Type;
                AssociationKey_update.Keyvalue = keyvalueEncryptold == keyvalueEncrypt ? AssociationKey_update.Keyvalue : keyvalueEncrypt;
                AssociationKey_update.Keyname = AssociationKey_update.Keyname == AssociationKey.Keyname ? AssociationKey_update.Keyname : AssociationKey.Keyname;
                AssociationKey_update.Pwd = AssociationKey_update.Pwd == AssociationKey.Pwd ? AssociationKey_update.Pwd : AssociationKey.Pwd;

                _context.AssociationKeys.Update(AssociationKey_update);
                await _context.SaveChangesAsync();

                return AssociationKey_update;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

    }
}
