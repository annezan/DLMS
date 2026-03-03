using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Commands;
using DLMS_MODELS.CompteurEquipementDomain.Queries;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des relations Compteur-Equipement
    /// Permet d'associer des compteurs à des équipements
    /// La logique de vérification d'accès est gérée dans les handlers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompteurEquipementController : AuthorizedApiController
    {
        /// <summary>
        /// Associe un ou plusieurs compteurs à des équipements
        /// </summary>
        [HttpPost("add")]
        [RequirePermission("CREATE_COMPTEUR_EQUIPEMENT")]
        public async Task<ActionResult<ResponseBase<CompteurEquipementResponse>>> Add([FromBody] List<CompteurEquipementAddCommand> commands)
        {
            var userId = GetCurrentUserId();
            
            // Assigner le UserId à chaque commande pour la vérification d'accès dans le handler
            foreach (var command in commands)
            {
                command.UserId = userId;
            }

            var request = new CompteurEquipementAddCommandList(commands);
            var result = await Mediator.Send(request);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime une ou plusieurs associations Compteur-Equipement
        /// </summary>
        [HttpDelete("delete")]
        [RequirePermission("DELETE_COMPTEUR_EQUIPEMENT")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] List<CompteurEquipementDeleteCommand> commands)
        {
            var userId = GetCurrentUserId();
            
            // Assigner le UserId à chaque commande pour la vérification d'accès dans le handler
            foreach (var command in commands)
            {
                command.UserId = userId;
            }

            var request = new CompteurEquipementDeleteCommandList(commands);
            var result = await Mediator.Send(request);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les équipements associés à un compteur
        /// La vérification d'accès est gérée dans le handler
        /// </summary>
        [HttpGet("getCompteurEquipementByIdCompteur")]
        [RequirePermission("VIEW_COMPTEUR_EQUIPEMENT")]
        public async Task<ActionResult<ResponseBase<CompteurEquipementResponse>>> GetCompteurEquipementByIdCompteur([FromQuery] GetCompteurEquipementByIdCompteurQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les compteurs associés à un équipement
        /// La vérification d'accès est gérée dans le handler
        /// </summary>
        [HttpGet("getCompteurEquipementByIdEquipement")]
        [RequirePermission("VIEW_COMPTEUR_EQUIPEMENT")]
        public async Task<ActionResult<ResponseBase<CompteurEquipementResponse>>> GetCompteurEquipementByIdEquipement([FromQuery] GetCompteurEquipementByIdEquipementQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
