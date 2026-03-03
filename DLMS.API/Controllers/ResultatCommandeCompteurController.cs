using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Queries;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des résultats des commandes compteur
    /// Permet de gérer les résultats d'exécution des commandes sur les compteurs
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultatCommandeCompteurController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère tous les résultats
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_RESULTAT_COMMANDE_COMPTEUR")]
        public async Task<ActionResult<ResponseBase<List<ResultatCommandeCompteurResponse>>>> Get()
        {
            var userId = GetCurrentUserId();
            var query = new GetResultatCommandeCompteursQuery(userId);
            var result = await Mediator.Send(query);

            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un résultat par son ID
        /// </summary>
        [HttpGet("getResultatById")]
        [RequirePermission("VIEW_RESULTAT_COMMANDE_COMPTEUR")]
        public async Task<ActionResult<ResponseBase<ResultatCommandeCompteurResponse>>> GetResultatById([FromQuery] int id)
        {
            var userId = GetCurrentUserId();
            var query = new GetResultatCommandeCompteurByIdQuery(id);
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère tous les résultats pour une commande compteur spécifique
        /// </summary>
        [HttpGet("getResultatsByCommandeCompteurId")]
        //[RequirePermission("VIEW_RESULTAT_COMMANDE_COMPTEUR")]
        public async Task<ActionResult<ResponseBase<List<ResultatCommandeCompteurResponse>>>> GetResultatsByCommandeCompteurId([FromQuery] int commandeCompteurId)
        {
            var userId = GetCurrentUserId();
            var query = new GetResultatCommandeCompteursByCommandeCompteurIdQuery(commandeCompteurId);
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

    }
}
