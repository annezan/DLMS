using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Commands;
using DLMS_MODELS.CompteurDomain.Queries;
using DLMS_MODELS.CompteurDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompteurController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère la liste des compteurs
        /// Le filtrage par poste est géré automatiquement dans le handler
        /// </summary>
        [RequirePermission("VIEW_COMPTEUR")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<CompteurResponse>>>> Get()
        {
            var userId = GetCurrentUserId();
            var query = new GetCompteurQuery(userId);
            var result = await Mediator.Send(query);
            
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un compteur par son ID
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("VIEW_COMPTEUR")]
        [HttpGet("getCompteurById")]
        public async Task<ActionResult<ResponseBase<CompteurResponse>>> GetCompteurById([FromQuery] GetCompteurByIdQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouveau compteur
        /// </summary>
        [RequirePermission("CREATE_COMPTEUR")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<CompteurResponse>>> Add([FromBody] CompteurAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un compteur existant
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("EDIT_COMPTEUR")]
        [HttpPut("edit")]
        public async Task<ActionResult<ResponseBase<CompteurResponse>>> Edit([FromBody] CompteurEditCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un compteur
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("DELETE_COMPTEUR")]
        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseBase<CompteurResponse>>> Delete([FromBody] CompteurDeleteCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Met à jour les données d'un compteur (synchronisation)
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("SYNC_COMPTEUR")]
        [HttpPost("MAJCompteur")]
        public async Task<ActionResult<ResponseBase<bool>>> MAJCompteur(CompteurMAJCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
