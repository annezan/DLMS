using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.ErrorDomain.Queries;
using DLMS_MODELS.ErrorDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des erreurs (table de référence)
    /// Les erreurs sont des codes globaux, pas de filtrage par poste
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ErrorController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère la liste de toutes les erreurs
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_ERROR")]
        public async Task<ActionResult<ResponseBase<List<ErrorResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetErrorQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une erreur par son ID
        /// </summary>
        [HttpGet("getErrorById")]
        [RequirePermission("VIEW_ERROR")]
        public async Task<ActionResult<ResponseBase<ErrorResponse>>> GetErrorById([FromQuery] GetErrorByIdQuery query)
        {
            var result = await Mediator.Send(new GetErrorByIdQuery(query.Id));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une erreur par sa valeur
        /// </summary>
        [HttpGet("getErrorByValue")]
        [RequirePermission("VIEW_ERROR")]
        public async Task<ActionResult<ResponseBase<ErrorResponse>>> GetErrorByValue([FromQuery] GetErrorByValueQuery query)
        {
            var result = await Mediator.Send(new GetErrorByValueQuery(query.Value));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
