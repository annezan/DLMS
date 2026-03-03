using DLMS_MODELS.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.FabricantDomain.Responses
{
    public class FabricantResponse : AuditableEntity
    {
        public int Id { get; set; }

        public string Libelle { get; set; }
        

    }
}
