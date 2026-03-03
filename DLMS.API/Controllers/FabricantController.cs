using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Commands;
using DLMS_MODELS.FabricantDomain.Queries;
using DLMS_MODELS.FabricantDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FabricantController : ApiController
    {
        /// <summary>
        /// Récupère la liste des fabricants
        /// </summary>
        [RequirePermission("VIEW_FABRICANT")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<FabricantResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetFabricantQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un fabricant par son ID
        /// </summary>
        [RequirePermission("VIEW_FABRICANT")]
        [HttpGet("getFabricantById")]
        public async Task<ActionResult<ResponseBase<FabricantResponse>>> GetFabricantById([FromQuery] GetFabricantByIdQuery query)
        {
            var result = await Mediator.Send(new GetFabricantByIdQuery(query.Id));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouveau fabricant
        /// </summary>
        [RequirePermission("CREATE_FABRICANT")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<FabricantResponse>>> Add([FromBody] FabricantAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un fabricant existant
        /// </summary>
        [RequirePermission("EDIT_FABRICANT")]
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<FabricantResponse>>> Edit([FromBody] FabricantEditCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un fabricant
        /// </summary>
        [RequirePermission("DELETE_FABRICANT")]
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] FabricantDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
