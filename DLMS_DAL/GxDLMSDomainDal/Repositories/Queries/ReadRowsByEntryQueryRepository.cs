using DLMS_DAL.Bases;
using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_DAL.Datas;
using Microsoft.EntityFrameworkCore;
using DLMS_COMMUNICATION;
using DLMS_COMMUNICATION.Reader;
using Gurux.DLMS.Objects;
using Newtonsoft.Json;
using DLMS_MODELS.AssociationKeyDomain.Entities;

namespace DLMS_DAL.GxDLMSDomainDal.Repositories.Queries
{
    public class ReadRowsByEntryQueryRepository : QueryBaseRepository<AssociationKey>, IReadRowsByEntryQueryRepository
    {

        public ReadRowsByEntryQueryRepository(DLMSDBContext context)
        :
            base(context)
        { 
        }
        

        public string GetReadRowsByEntry(string? port, string? serialport, string? AddressIp, string? ClientAddress, string? SerialNumber, string? interfaceType, string? Objects)
        {
            try
            {
                var authentication = _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "authentication" && x.CompteurId.Contains(SerialNumber)).FirstOrDefault();
                var unicast = _context.AssociationKeys.Where(x => x.Type == ClientAddress && x.Keyname == "unicast" && x.CompteurId.Contains(SerialNumber)).FirstOrDefault();

                if (authentication == null || unicast == null)
                {
                    throw new Exception("Authentication ou Unicast key non trouvée");
                }

                var AuthenticationKey = Cryptage.Decrypt(authentication.Keyvalue, "ASCDLMS");
                var UnicastKey = Cryptage.Decrypt(unicast.Keyvalue, "ASCDLMS");
                long count = 1;
                var result=ReaderCommunication.ReadRowsByEntry(count,port, serialport, AddressIp, ClientAddress, SerialNumber, interfaceType, authentication.Pwd, AuthenticationKey, UnicastKey, Objects);

                // Désérialisation en liste d'objets
                var entries = JsonConvert.DeserializeObject<List<KeyValuePair<object[], object[]>>>(result);

                if (entries == null || entries.Count == 0)
                {
                    Console.WriteLine("Aucune donnée à traiter.");
                    return "";
                }

                foreach (var entry in entries)
                {
                    Console.WriteLine("📌 Nouvelle entrée :");

                    // Parcourir les lignes de données
                    foreach (var row in entry.Key)
                    {
                        Console.WriteLine($"  - Donnée : {row}");
                    }

                    // Parcourir les objets associés
                    foreach (var obj in entry.Value)
                    {
                        Console.WriteLine($"  - Objet DLMS : {obj}");
                    }

                    Console.WriteLine("----------");
                }
                return result.ToString();
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

    }
}
