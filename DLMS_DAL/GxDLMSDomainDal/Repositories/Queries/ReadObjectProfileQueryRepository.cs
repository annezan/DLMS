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

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class ReadObjectProfileQueryRepository : QueryBaseRepository<AssociationKey>, IReadObjectProfileQueryRepository
    {
        private readonly IGxdlmsprofilgenericdetailCommandRepository _IGxdlmsprofilgenericdetailCommandRepository;
        private readonly IGxdlmsprofilgenericdetailseventCommandRepository _IGxdlmsprofilgenericdetailseventCommandRepository;

        private readonly IGxdlmsprofilgenericQueryRepository _IGxdlmsprofilgenericQueryRepository;
        public ReadObjectProfileQueryRepository(DLMSDBContext context, IGxdlmsprofilgenericdetailCommandRepository IGxdlmsprofilgenericdetailCommandRepository, IGxdlmsprofilgenericQueryRepository IGxdlmsprofilgenericQueryRepository , IGxdlmsprofilgenericdetailseventCommandRepository IGxdlmsprofilgenericdetailseventCommandRepository)
        :
            base(context)
        {
            _IGxdlmsprofilgenericdetailCommandRepository = IGxdlmsprofilgenericdetailCommandRepository;
            _IGxdlmsprofilgenericdetailseventCommandRepository = IGxdlmsprofilgenericdetailseventCommandRepository;
            _IGxdlmsprofilgenericQueryRepository = IGxdlmsprofilgenericQueryRepository;
        }
        

        public async Task<string> GetReadObjectProfile(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            try
            {
                Gxdlmsprofilgenericdetail detailprofil = new Gxdlmsprofilgenericdetail();
                Gxdlmsprofilgenericdetailsevent detailprofilevent = new Gxdlmsprofilgenericdetailsevent();
                string[] profil = new string[]
                {
                "1.0.99.1.0.255",
                "1.0.99.2.0.255",
                "1.0.99.3.0.255",
                "0.0.98.1.0.255",
                "0.0.99.98.0.255",
                "0.0.99.98.1.255",
                "0.0.99.98.2.255",
                "0.0.99.98.3.255",
                "0.0.99.98.4.255",
                "0.0.99.98.5.255",
                "0.0.99.98.6.255",
                "0.0.99.98.7.255"
            };
                var authentication = await _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "authentication" && x.CompteurId.Contains(SerialNumber)).FirstOrDefaultAsync();
                var unicast = await _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "unicast" && x.CompteurId.Contains(SerialNumber)).FirstOrDefaultAsync();

                if (authentication == null || unicast == null)
                {
                    return null;
                }

                var AuthenticationKey = Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS");
                var UnicastKey = Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS");
                
                var profilgeneticdetail = await _context.Gxdlmsprofilgenericdetails.Where(x => x.NumeroCompteur == SerialNumber).FirstOrDefaultAsync();
                var profilgeneticdetailevent = await _context.Gxdlmsprofilgenericdetailsevents.Where(x => x.NumeroCompteur == SerialNumber).FirstOrDefaultAsync();

                var result = "";
                long count= 0;
                
                if (profilgeneticdetail == null && profilgeneticdetailevent== null)
                {
                    count = 0;
                    result = ReaderCommunication.ReadRowsByEntry(count, port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, authentication.Pwd, AuthenticationKey, UnicastKey, "1.0.99.1.0.255:2;1.0.99.2.0.255:2; 1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2");

                }
                else
                {
                    DateTime maintenant = DateTime.Now;
                    DateTime ilYA48Heures = maintenant.AddHours(-48);
                    var datestart = ilYA48Heures.Date.ToString();
                    var dateend= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                    var essai = datestart;
                    result = ReaderCommunication.ReadRowsByRange(datestart,dateend.ToString(), port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, authentication.Pwd, AuthenticationKey, UnicastKey, "1.0.99.1.0.255:2;1.0.99.2.0.255:2; 1.0.99.3.0.255:2;0.0.98.1.0.255:2;0.0.99.98.0.255:2;0.0.99.98.1.255:2;0.0.99.98.2.255:2;0.0.99.98.3.255:2;0.0.99.98.4.255:2;0.0.99.98.5.255:2;0.0.99.98.6.255:2;0.0.99.98.7.255:2");

                }

                // Désérialisation en liste d'objets
                var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(result);

                if (entries == null || entries.Count == 0)
                {
                    Console.WriteLine("Aucune donnée à traiter.");
                    return null;
                }
                var k = 0;
                foreach (var entry in entries)
                {
                    Console.WriteLine("📌 Nouvelle entrée :");
                    
                    var profilgeneric = await _IGxdlmsprofilgenericQueryRepository.GetProfilgenericByLN(profil[k]);

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
                                if (k < 4)
                                {
                                    if (i == 0)
                                    {
                                        var codeobis = await _context.CodeObis.Where(x => x.Value == obj).FirstOrDefaultAsync();
                                        // Si la première valeur est convertible en DateTime
                                        long unixTimestamp = Convert.ToUInt32(array[i]);

                                        // Convertir le timestamp en date UTC
                                        dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                        detailprofil.DateEnr = dateUtc;
                                        detailprofil.GxdlmsprofilgenericId = profilgeneric.Id;
                                        detailprofil.CodeObisId = codeobis.Id;

                                        detailprofil.NumeroCompteur = SerialNumber;
                                        detailprofil.IsArchive = false;
                                        detailprofil.Value = array[i].ToString();
                                        var addprofil = await _IGxdlmsprofilgenericdetailCommandRepository.AddGxdlmsprofilgenericdetail(detailprofil);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var essai = entry.Value[i];
                                            // Créer une instance de connexion pour accéder aux objets DLMS
                                            var connexion = new ConnexionCommunication();
                                            
                                            GXDLMSObjectCollection objs = connexion.client.Objects.GetObjects(new ObjectType[] { ObjectType.Register, ObjectType.ExtendedRegister, ObjectType.DemandRegister, ObjectType.None, ObjectType.Data });
                                            foreach (GXDLMSObject it in objs)
                                            {
                                                if (it.Name.ToString() == entry.Value[i].ToString())
                                                {
                                                    if (it is GXDLMSRegister || it is GXDLMSDemandRegister)
                                                    {
                                                        var realValue = ((JValue)array[i]).Value;
                                                        var datatype = realValue?.GetType();
                                                        //var datatype = array[i].GetType();
                                                        if (datatype == typeof(GXDateTime))
                                                        {
                                                            var valconvert1 = array[i].ToString();
                                                            detailprofil.Value = valconvert1;
                                                        }
                                                        else if (datatype == typeof(decimal) || datatype == typeof(Int64) || datatype == typeof(Int32))
                                                        {
                                                            var valconvert = Convert.ToDecimal(array[i]);
                                                            detailprofil.Value = valconvert.ToString();
                                                        }
                                                        else
                                                        {
                                                            detailprofil.Value = array[i].ToString();
                                                        }


                                                        var codeobis = await _context.CodeObis.Where(x => x.Value == obj).FirstOrDefaultAsync();

                                                        detailprofil.DateEnr = dateUtc;
                                                        detailprofil.GxdlmsprofilgenericId = profilgeneric.Id;
                                                        detailprofil.NumeroCompteur = SerialNumber;
                                                        detailprofil.IsArchive = false;
                                                        detailprofil.CodeObisId = codeobis.Id;
                                                        var addprofil = await _IGxdlmsprofilgenericdetailCommandRepository.AddGxdlmsprofilgenericdetail(detailprofil);

                                                    }
                                                    else
                                                    {
                                                        var codeobis = await _context.CodeObis.Where(x => x.Value == obj).FirstOrDefaultAsync();
                                                        detailprofil.Value = array[i].ToString();
                                                        detailprofil.DateEnr = dateUtc;
                                                        detailprofil.GxdlmsprofilgenericId = profilgeneric.Id;
                                                        detailprofil.NumeroCompteur = SerialNumber;
                                                        detailprofil.IsArchive = false;
                                                        detailprofil.CodeObisId = codeobis.Id;
                                                        var addprofil = await _IGxdlmsprofilgenericdetailCommandRepository.AddGxdlmsprofilgenericdetail(detailprofil);
                                                    }
                                                }
                                            }


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.ToString());
                                            // Actaric SL7000 peut retourner une erreur ici. Continuer la lecture.
                                        }



                                    }
                                }
                                else
                                {

                                    long valconvert = 0;

                                    if (entry.Value[i].ToString() == "0.0.96.11.0.255" || entry.Value[i].ToString() == "0.0.96.11.1.255" || entry.Value[i].ToString() == "0.0.96.11.2.255" || entry.Value[i].ToString() == "0.0.96.11.3.255" || entry.Value[i].ToString() == "0.0.96.11.4.255" || entry.Value[i].ToString() == "0.0.96.11.5.255" || entry.Value[i].ToString() == "0.0.96.11.6.255" || entry.Value[i].ToString() == "0.0.96.11.7.255")
                                    {
                                        valconvert = Convert.ToInt64(array[i]);
                                        var eventresult = await _context.Events.Where(x => x.Value == valconvert && x.Category == "EVENTS_GROUP_ALL_REGISTERS").FirstOrDefaultAsync();
                                        detailprofilevent.Value = array[i].ToString();

                                        detailprofilevent.EventId = eventresult.Id;

                                    }
                                    else
                                    {

                                        detailprofilevent.Value = array[i].ToString();
                                        detailprofilevent.EventId = null;
                                        detailprofilevent.Event = null;

                                    }

                                    if (i == 0)
                                    {
                                        long unixTimestamp = Convert.ToUInt32(array[i]);
                                        dateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
                                    }
                                    // Convertir le timestamp en date UTC
                                    var codeobis = await _context.CodeObis.Where(x => x.Value == obj).FirstOrDefaultAsync();

                                    detailprofilevent.DateEnr = dateUtc;
                                    detailprofilevent.CodeObisId = codeobis.Id;
                                    detailprofilevent.GxdlmsprofilgenericId = profilgeneric.Id;
                                    detailprofilevent.NumeroCompteur = SerialNumber;
                                    //detailprofilevent.Compteurid = "APAESX30" + SerialNumber;
                                    detailprofilevent.IsArchive = false;

                                    var addprofilevent = await _IGxdlmsprofilgenericdetailseventCommandRepository.AddGxdlmsprofilgenericdetailsevent(detailprofilevent);


                                }


                            }
                        }
                    }
                    
                    k++;
                }
                return result.ToString();
                
            }
            catch (Exception ex)
            {
                return null ;
            }

        }

    }
}
