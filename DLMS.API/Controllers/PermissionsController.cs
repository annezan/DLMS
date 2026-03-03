using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionsController : ApiController
    {
        /// <summary>
        /// Récupère la liste des permissions
        /// </summary>
        [RequirePermission("VIEW_PERMISSION")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<PermissionResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetPermissionsQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une permission par son ID
        /// </summary>
        [RequirePermission("VIEW_PERMISSION")]
        [HttpGet("getPermissionsById")]
        public async Task<ActionResult<ResponseBase<PermissionResponse>>> GetPermissionsById([FromQuery] Guid id)
        {
            var query = new GetPermissionByIdQuery(id);
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
