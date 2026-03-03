using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ApiController
    {
        /// <summary>
        /// Récupère la liste des rôles
        /// </summary>
        [RequirePermission("VIEW_ROLE")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<RoleResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetRolesQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un rôle par son ID
        /// </summary>
        [RequirePermission("VIEW_ROLE")]
        [HttpGet("getRoleById")]
        public async Task<ActionResult<ResponseBase<RoleResponse>>> GetRoleById([FromQuery] Guid id)
        {
            var result = await Mediator.Send(new GetRoleByIdQuery(id));
            return result != null ? Ok(result) : BadRequest("Rôle introuvable.");
        }

        /// <summary>
        /// Récupère un rôle par son code
        /// </summary>
        [RequirePermission("VIEW_ROLE")]
        [HttpGet("getRoleByCode")]
        public async Task<ActionResult<ResponseBase<RoleResponse>>> GetRoleByCode([FromQuery] string code)
        {
            var query = new GetRoleByCodeQuery(code);
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouveau rôle
        /// </summary>
        [RequirePermission("CREATE_ROLE")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<RoleResponse>>> Add([FromBody] RoleAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un rôle existant
        /// </summary>
        [RequirePermission("EDIT_ROLE")]
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<RoleResponse>>> Edit([FromBody] RoleEditCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un rôle
        /// </summary>
        [RequirePermission("DELETE_ROLE")]
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] RoleDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
