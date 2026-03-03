using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Commands;
using DLMS_MODELS.CompteurCelluleDomain.Queries;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des relations Compteur-Cellule
    /// Permet d'associer des compteurs à des cellules
    /// La logique de vérification d'accès est gérée dans les handlers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompteurCelluleController : AuthorizedApiController
    {
        /// <summary>
        /// Associe un ou plusieurs compteurs à des cellules
        /// </summary>
        [HttpPost("add")]
        [RequirePermission("CREATE_COMPTEUR_CELLULE")]
        public async Task<ActionResult<ResponseBase<List<CompteurCelluleResponse>>>> Add([FromBody] List<CompteurCelluleAddCommand> commands)
        {
            var userId = GetCurrentUserId();
            
            // Assigner le UserId et CreatedBy à chaque commande pour la vérification d'accès dans le handler
            foreach (var command in commands)
            {
                command.UserId = userId;
            }

            var request = new CompteurCelluleAddCommandList(commands);
            var result = await Mediator.Send(request);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime une ou plusieurs associations Compteur-Cellule
        /// </summary>
        [HttpDelete("delete")]
        [RequirePermission("DELETE_COMPTEUR_CELLULE")]
        public async Task<ActionResult<ResponseBase<List<CompteurCelluleResponse>>>> Delete([FromBody] List<CompteurCelluleDeleteCommand> commands)
        {
            var userId = GetCurrentUserId();
            
            // Assigner le UserId et DeletedBy à chaque commande pour la vérification d'accès dans le handler
            foreach (var command in commands)
            {
                command.UserId = userId;
            }

            var request = new CompteurCelluleDeleteCommandList(commands);
            var result = await Mediator.Send(request);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les cellules associées à un compteur
        /// La vérification d'accès est gérée dans le handler
        /// </summary>
        [HttpGet("getCompteurCelluleByIdCompteur")]
        [RequirePermission("VIEW_COMPTEUR_CELLULE")]
        public async Task<ActionResult<ResponseBase<List<CompteurCelluleResponse>>>> GetCompteurCelluleByIdCompteur([FromQuery] GetCompteurCelluleByIdCompteurQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les compteurs associés à une cellule
        /// La vérification d'accès est gérée dans le handler
        /// </summary>
        [HttpGet("getCompteurCelluleByIdCellule")]
        [RequirePermission("VIEW_COMPTEUR_CELLULE")]
        public async Task<ActionResult<ResponseBase<List<CompteurCelluleResponse>>>> GetCompteurCelluleByIdCellule([FromQuery] GetCompteurCelluleByIdCelluleQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result.IsSuccess ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
