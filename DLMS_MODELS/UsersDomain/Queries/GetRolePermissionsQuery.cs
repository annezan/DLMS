using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Queries
{
    /// <summary>
    /// Query pour récupérer toutes les permissions d'un rôle spécifique
    /// </summary>
    public class GetRolePermissionsQuery : IRequest<ResponseBase<List<PermissionResponse>>>
    {
        public Guid RoleId { get; set; }

        public GetRolePermissionsQuery(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}

