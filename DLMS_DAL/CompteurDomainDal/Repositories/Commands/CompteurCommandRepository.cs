using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_DAL.Helpers;
using DLMS_DAL.Bases;
using DLMS_DAL.Datas;
using DLMS_MODELS.CompteurDomain.Entities;
using Microsoft.EntityFrameworkCore;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_COMMUNICATION.Reader;
using Gurux.DLMS.Enums;
using System.IO.Ports;
using Gurux.DLMS.ManufacturerSettings;
using Newtonsoft.Json;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;

namespace DLMS_DAL.CompteurDomainDal.Repositories
{
    public class CompteurCommandRepository : CommandRepository<Compteur>, ICompteurCommandRepository
    {
        public CompteurCommandRepository(DLMSDBContext context)
            : base(context)
        {

        }
        public async Task<Compteur> AddCompteur(Compteur Compteur)
        {
            try
            {
                var compteur = _context.Compteur.AsNoTracking().FirstOrDefault(x => x.IdCompteur == Compteur.IdCompteur);
                if (compteur == null)
                {
                    Compteur.CreatedAt= DateTime.Now;
                    _context.Compteur.Add(Compteur);
                    await _context.SaveChangesAsync();
                    return Compteur;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Compteur> EditCompteur(Compteur compteur)
        {
            try
            {
                var Compteur_update = _context.Compteur.AsNoTracking().FirstOrDefault(x => x.Id == compteur.Id && x.IsArchive==false);
                if (Compteur_update != null)
                {
                    Compteur_update.IdCompteur = Compteur_update.IdCompteur == compteur.IdCompteur ? Compteur_update.IdCompteur : compteur.IdCompteur;
                    Compteur_update.NumeroCompteur = Compteur_update.NumeroCompteur == compteur.NumeroCompteur ? Compteur_update.NumeroCompteur : compteur.NumeroCompteur;
                    Compteur_update.MarqueCompteur = Compteur_update.MarqueCompteur == compteur.MarqueCompteur ? Compteur_update.MarqueCompteur : compteur.MarqueCompteur;
                    Compteur_update.DatePremierePose = Compteur_update.DatePremierePose == compteur.DatePremierePose ? Compteur_update.DatePremierePose : compteur.DatePremierePose;
                    Compteur_update.DatePoseActuelle = Compteur_update.DatePoseActuelle == compteur.DatePoseActuelle ? Compteur_update.DatePoseActuelle : compteur.DatePoseActuelle;
                    Compteur_update.Typecompteur = Compteur_update.Typecompteur == compteur.Typecompteur ? Compteur_update.Typecompteur : compteur.Typecompteur;
                    Compteur_update.FabriquantId = Compteur_update.FabriquantId == compteur.FabriquantId ? Compteur_update.FabriquantId : compteur.FabriquantId;
                    Compteur_update.Etatcontacteur = Compteur_update.Etatcontacteur == compteur.Etatcontacteur ? Compteur_update.Etatcontacteur : compteur.Etatcontacteur;
                    Compteur_update.UpdatedBy = Compteur_update.UpdatedBy == compteur.UpdatedBy ? Compteur_update.UpdatedBy : compteur.UpdatedBy;
                    Compteur_update.UpdatedAt = DateTime.Now;

                    _context.Compteur.Update(Compteur_update);
                    await _context.SaveChangesAsync();

                    return Compteur_update;
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool MAJCompteur(string result, int Id)
        {
            try
            {
                var Deserializeresult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                if (Deserializeresult == null || Deserializeresult.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"MAJCompteur: données vides pour compteur Id={Id}");
                    return false;
                }

                var Compteur_update = _context.Compteur.FirstOrDefault(x => x.Id == Id);
                if (Compteur_update == null)
                {
                    System.Diagnostics.Debug.WriteLine($"MAJCompteur: compteur Id={Id} introuvable en base");
                    return false;
                }

                foreach (var item1 in Deserializeresult)
                {
                    if (item1.Key == "0.0.42.0.0.255" && item1.Value?.Length >= 7)
                    {
                        Compteur_update.Typecompteur = item1.Value.ToString().Substring(3, 4);
                    }
                    else if (item1.Key == "1.0.99.1.0.255")
                    {
                        if (int.TryParse(item1.Value, out int val))
                            Compteur_update.EnergyProfilePeriod = (val / 60).ToString();
                    }
                    else if (item1.Key == "1.0.99.2.0.255")
                    {
                        if (int.TryParse(item1.Value, out int val))
                            Compteur_update.TechnicalProfilePeriod = (val / 60).ToString();
                    }
                    else if (item1.Key == "0.0.0.2.8.255")
                    {
                        Compteur_update.CrcFirmware = item1.Value?.ToString();
                    }
                    else if (item1.Key == "0.0.0.2.0.255")
                    {
                        Compteur_update.VersionFirmware = item1.Value?.ToString();
                    }
                    else if (item1.Key == "1.0.0.2.2.255")
                    {
                        Compteur_update.Tarif = item1.Value?.ToString();
                    }
                }

                _context.Compteur.Update(Compteur_update);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MAJCompteur ERREUR compteur Id={Id}: {ex.Message}");
                throw; // remonter l'exception pour que l'appelant sache que ça a échoué
            }
        }

        public async Task<Compteur> DeleteCompteur(Compteur compteur)
        {
            try
            {
                var Compteur_delete = _context.Compteur.AsNoTracking().FirstOrDefault(x => x.Id == compteur.Id);
                Compteur_delete.IsArchive = true;
                Compteur_delete.DeletedAt= DateTime.Now;
                Compteur_delete.DeletedBy = Compteur_delete.DeletedBy == compteur.DeletedBy ? Compteur_delete.DeletedBy : compteur.DeletedBy;
                _context.Compteur.Update(Compteur_delete);
                
                // Archiver les CompteurEquipement associés
                var compteursEquipement = await _context.CompteurEquipement
                    .Where(ce => ce.CompteurId == compteur.Id && !ce.IsArchive)
                    .ToListAsync();
                
                foreach (var ce in compteursEquipement)
                {
                    ce.IsArchive = true;
                    ce.DeletedAt = DateTime.Now;
                    ce.DeletedBy = compteur.DeletedBy;
                    _context.CompteurEquipement.Update(ce);
                }
                
                // Archiver les CompteurCellule associés
                var compteursCellule = await _context.CompteurCellule
                    .Where(cc => cc.CompteurId == compteur.Id && !cc.IsArchive)
                    .ToListAsync();
                
                foreach (var cc in compteursCellule)
                {
                    cc.IsArchive = true;
                    cc.DeletedAt = DateTime.Now;
                    cc.DeletedBy = compteur.DeletedBy;
                    _context.CompteurCellule.Update(cc);
                }
                
                await _context.SaveChangesAsync();

                return Compteur_delete;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}

