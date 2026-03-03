using DLMS_MODELS.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.CompteurEquipementDomain.Responses
{
    public class CompteurEquipementResponse : AuditableEntity
    {
        public int CompteurId { get; set; }

        public int EquipementId { get; set; }
        
    }
}
