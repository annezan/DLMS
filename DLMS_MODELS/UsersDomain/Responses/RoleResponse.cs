using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLMS_MODELS.UsersDomain.Responses
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string Libelle { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        
        /// <summary>
        /// Liste des permissions associées à ce rôle
        /// </summary>
        public List<PermissionResponse> Permissions { get; set; } = new List<PermissionResponse>();
    }
}
