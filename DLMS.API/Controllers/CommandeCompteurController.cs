using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Queries;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des relations Commande-Compteur
    /// Permet d'associer des compteurs à des commandes
    /// La logique de vérification d'accès est gérée dans les handlers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommandeCompteurController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère toutes les relations Commande-Compteur
        /// Le filtrage par poste est géré automatiquement dans le handler
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_COMMANDE_COMPTEUR")]
        public async Task<ActionResult<ResponseBase<List<CommandeCompteurResponse>>>> Get()
        {
            var userId = GetCurrentUserId();
            var query = new GetCommandeCompteursQuery(userId);
            var result = await Mediator.Send(query);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère une relation Commande-Compteur par son ID
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [HttpGet("getCommandeCompteurById")]
        [RequirePermission("VIEW_COMMANDE_COMPTEUR")]
        public async Task<ActionResult<ResponseBase<CommandeCompteurResponse>>> GetCommandeCompteurById([FromQuery] int id)
        {
            var userId = GetCurrentUserId();
            var query = new GetCommandeCompteurByIdQuery(id);
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime une relation Commande-Compteur
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [HttpDelete("delete")]
        [RequirePermission("DELETE_COMMANDE_COMPTEUR")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] CommandeCompteurDeleteCommand command)
        {
            var userId = GetCurrentUserId();
            command.UserId = userId;
            
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
