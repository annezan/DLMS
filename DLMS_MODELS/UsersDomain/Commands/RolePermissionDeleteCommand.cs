using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    /// <summary>
    /// Commande pour retirer une ou plusieurs permissions d'un rôle
    /// </summary>
    public class RolePermissionDeleteCommand : IRequest<ResponseBase<string>>
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}

