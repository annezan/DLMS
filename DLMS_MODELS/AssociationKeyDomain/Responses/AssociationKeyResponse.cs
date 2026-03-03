using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.AssociationKeyDomain.Responses
{
    public class AssociationKeyResponse
    {
        public int Id { get; set; }
        public string CompteurId { get; set; }

        public string Type { get; set; } = null!;

        public string Keyvalue { get; set; } = null!;

        public string Keyname { get; set; } = null!;

        public string? Pwd { get; set; }


    }
}
