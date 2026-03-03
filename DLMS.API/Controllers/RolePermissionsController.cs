using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des associations entre rôles et permissions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolePermissionsController : ApiController
    {
        /// <summary>
        /// Récupère toutes les permissions associées à un rôle spécifique
        /// </summary>
        /// <param name="roleId">ID du rôle</param>
        //[RequirePermission("VIEW_ROLE")]
        [HttpGet("{roleId}")]
        public async Task<ActionResult<ResponseBase<List<PermissionResponse>>>> GetRolePermissions(Guid roleId)
        {
            var query = new GetRolePermissionsQuery(roleId);
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Ajoute une ou plusieurs permissions à un rôle
        /// </summary>
        [RequirePermission("EDIT_ROLE")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<string>>> AddPermissionsToRole([FromBody] RolePermissionAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Retire une ou plusieurs permissions d'un rôle
        /// </summary>
        [RequirePermission("EDIT_ROLE")]
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> RemovePermissionsFromRole([FromBody] RolePermissionDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : BadRequest(result);
        }
    }
}

