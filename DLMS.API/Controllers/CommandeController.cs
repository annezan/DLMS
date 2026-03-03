using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Commands;
using DLMS_MODELS.CommandeDomain.Queries;
using DLMS_MODELS.CommandeDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommandeController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère la liste des commandes
        /// Le filtrage par poste est géré automatiquement dans le handler
        /// </summary>
        [RequirePermission("VIEW_COMMANDE")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<CommandeResponse>>>> Get()
        {
            var userId = GetCurrentUserId();
            var query = new GetCommandesQuery(userId);
            var result = await Mediator.Send(query);
            
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une commande par son ID
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("VIEW_COMMANDE")]
        [HttpGet("getCommandeById")]
        public async Task<ActionResult<ResponseBase<CommandeResponse>>> GetCommandeById([FromQuery] GetCommandeByIdQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée une nouvelle commande
        /// </summary>
        [RequirePermission("CREATE_COMMANDE")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<CommandeResponse>>> Add([FromBody] CommandeAddCommand command)
        {
            // Note: La validation des compteurs pourrait être ajoutée ici ou dans le handler
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie une commande existante
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("EDIT_COMMANDE")]
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<CommandeResponse>>> Edit([FromBody] CommandeEditCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime une commande
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [RequirePermission("DELETE_COMMANDE")]
        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] CommandeDeleteCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
