using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EventsDomain.Queries;
using DLMS_MODELS.EventsDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des événements (table de référence)
    /// Les événements sont des codes globaux, pas de filtrage par poste
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère la liste de tous les événements
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_EVENT")]
        public async Task<ActionResult<ResponseBase<List<EventsResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetEventsQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un événement par son ID
        /// </summary>
        [HttpGet("getEventsById")]
        [RequirePermission("VIEW_EVENT")]
        public async Task<ActionResult<ResponseBase<EventsResponse>>> GetEventsById([FromQuery] GetEventsByIdQuery query)
        {
            var result = await Mediator.Send(new GetEventsByIdQuery(query.Id));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un événement par sa valeur
        /// </summary>
        [HttpGet("getEventsByValue")]
        [RequirePermission("VIEW_EVENT")]
        public async Task<ActionResult<ResponseBase<EventsResponse>>> GetEventsByValue([FromQuery] GetEventsByValueQuery query)
        {
            var result = await Mediator.Send(new GetEventsByValueQuery(query.Value));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
