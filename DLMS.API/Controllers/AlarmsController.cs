using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.AlarmsDomain.Queries;
using DLMS_MODELS.AlarmsDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des alarmes (table de référence)
    /// Les alarmes sont des codes globaux, pas de filtrage par poste
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlarmsController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère la liste de toutes les alarmes
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_ALARM")]
        public async Task<ActionResult<ResponseBase<List<AlarmsResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetAlarmsQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une alarme par son ID
        /// </summary>
        [HttpGet("getAlarmsById")]
        [RequirePermission("VIEW_ALARM")]
        public async Task<ActionResult<ResponseBase<AlarmsResponse>>> GetAlarmsById([FromQuery] GetAlarmsByIdQuery query)
        {
            var result = await Mediator.Send(new GetAlarmsByIdQuery(query.Id));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une alarme par sa valeur
        /// </summary>
        [HttpGet("getAlarmsByValue")]
        [RequirePermission("VIEW_ALARM")]
        public async Task<ActionResult<ResponseBase<AlarmsResponse>>> GetAlarmsByValue([FromQuery] GetAlarmsByValueQuery query)
        {
            var result = await Mediator.Send(new GetAlarmsByValueQuery(query.Value));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
