using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    /// <summary>
    /// Commande pour ajouter une ou plusieurs permissions à un rôle
    /// </summary>
    public class RolePermissionAddCommand : IRequest<ResponseBase<string>>
    {
        public Guid RoleId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}

