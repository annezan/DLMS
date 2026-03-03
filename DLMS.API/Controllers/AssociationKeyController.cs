using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Commands;
using DLMS_MODELS.AssociationKeyDomain.Queries;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DLMS_MODELS.AssocationKeyDomain.Queries;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des clés d'association DLMS
    /// ⚠️ ENDPOINTS DÉSACTIVÉS - Les clés d'association ne sont plus accessibles via l'API
    /// Les clés sont liées aux compteurs, filtrage par poste appliqué
    /// La logique de vérification d'accès est gérée dans les handlers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssociationKeyController : AuthorizedApiController
    {
        // ⚠️ TOUS LES ENDPOINTS DE CE CONTRÔLEUR SONT DÉSACTIVÉS

        /*
        /// <summary>
        /// Récupère toutes les clés d'association
        /// Le filtrage par poste est géré automatiquement dans le handler
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_ASSOCIATION_KEY")]
        public async Task<ActionResult<ResponseBase<List<AssociationKeyResponse>>>> Get()
        {
            var userId = GetCurrentUserId();
            var query = new GetAssociationKeyQuery(userId);
            var result = await Mediator.Send(query);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Ajoute une ou plusieurs clés d'association
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [HttpPost("add")]
        [RequirePermission("CREATE_ASSOCIATION_KEY")]
        public async Task<ActionResult<ResponseBase<AssociationKeyResponse>>> Add([FromBody] List<AssociationKeyAddCommand> commands)
        {
            var userId = GetCurrentUserId();
            
            // Assigner le UserId à chaque commande pour la vérification d'accès dans le handler
            foreach (var command in commands)
            {
                command.UserId = userId;
            }

            var request = new AssociationKeyAddCommandList(commands);
            var result = await Mediator.Send(request);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les clés d'association par type et compteur
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [HttpGet("getAssociationKeyByType")]
        [RequirePermission("VIEW_ASSOCIATION_KEY")]
        public async Task<ActionResult<ResponseBase<AssociationKeyResponse>>> GetAssociationKeyByType([FromQuery] GetAssociationKeyByTypeQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(new GetAssociationKeyByTypeQuery(query.Type, query.CompteurId) { UserId = userId });
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Vérifie l'existence d'une clé d'association
        /// La vérification d'accès est gérée automatiquement dans le handler
        /// </summary>
        [HttpGet("getAssociationKeyExisting")]
        [RequirePermission("VIEW_ASSOCIATION_KEY")]
        public async Task<ActionResult<ResponseBase<AssociationKeyResponse>>> GetAssociationKeyExisting([FromBody] GetAssociationKeyExistingQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(new GetAssociationKeyExistingQuery(query.Keyvalue, query.Compteurid) { UserId = userId });
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
        */
    }
}
