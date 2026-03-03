using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Queries;
using DLMS_MODELS.CodeObisDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CodeObisController : ApiController
    {
        /// <summary>
        /// Récupère la liste des codes OBIS
        /// </summary>
        [RequirePermission("VIEW_CODEOBIS")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<CodeObisResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetCodeObisQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un code OBIS par son ID
        /// </summary>
        [RequirePermission("VIEW_CODEOBIS")]
        [HttpGet("getCodeObisById")]
        public async Task<ActionResult<ResponseBase<CodeObisResponse>>> GetCodeObisById([FromQuery] GetCodeObisByIdQuery query)
        {
            var result = await Mediator.Send(new GetCodeObisByIdQuery(query.Id));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un code OBIS par sa valeur
        /// </summary>
        [RequirePermission("VIEW_CODEOBIS")]
        [HttpGet("getCodeObisByValue")]
        public async Task<ActionResult<ResponseBase<CodeObisResponse>>> GetCodeObisByValue([FromQuery] GetCodeObisByValueQuery query)
        {
            var result = await Mediator.Send(new GetCodeObisByValueQuery(query.Value));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
