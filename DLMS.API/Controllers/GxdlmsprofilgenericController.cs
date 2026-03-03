using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Commands;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des profils génériques DLMS
    /// Les profils sont liés aux compteurs via NumeroCompteur (IdCompteur)
    /// Le filtrage par poste est maintenant implémenté dans les handlers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GxdlmsprofilgenericController : AuthorizedApiController
    {
        /// <summary>
        /// Ajoute un détail de profil générique
        /// </summary>
        [HttpPost("Addprofilgenericdetail")]
        [RequirePermission("CREATE_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<GxdlmsprofilgenericResponse>>> AddGxdlmsprofilgenericdetail([FromBody] GxdlmsprofilgenericdetailAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Ajoute un événement de profil générique
        /// </summary>
        [HttpPost("Addprofilgenericdetailsevent")]
        [RequirePermission("CREATE_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<GxdlmsprofilgenericResponse>>> AddGxdlmsprofilgenericdetailsevent([FromBody] GxdlmsprofilgenericdetailseventAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un détail de profil générique
        /// </summary>
        [HttpPost("deleteprofilgenericdetail")]
        [RequirePermission("DELETE_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<string>>> deleteprofilgenericdetail([FromBody] GxdlmsprofilgenericdetailDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un événement de profil générique
        /// </summary>
        [HttpPost("deleteprofilgenericdetailsevent")]
        [RequirePermission("DELETE_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<string>>> deleteprofilgenericdetailsevent([FromBody] GxdlmsprofilgenericdetailDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un profil générique par son Logical Name (LN)
        /// </summary>
        [HttpGet("profilgenericByLN")]
        [RequirePermission("VIEW_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<GxdlmsprofilgenericResponse>>> GetGxdlmsprofilgenericByLN([FromBody] GetGxdlmsprofilgenericByLNQuery query)
        {
            var result = await Mediator.Send(new GetGxdlmsprofilgenericByLNQuery(query.LN));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les profils génériques par statut d'archivage
        /// </summary>
        [HttpGet("profilgenericByStatus")]
        [RequirePermission("VIEW_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<GxdlmsprofilgenericResponse>>> GetGxdlmsprofilgenericByStatus([FromQuery] GetGxdlmsprofilgenericByStatusQuery query)
        {
            var result = await Mediator.Send(new GetGxdlmsprofilgenericByStatusQuery(query.IsArchive));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les détails d'un profil générique par numéro de compteur
        /// Filtre les résultats par poste si l'utilisateur a un PosteId assigné
        /// </summary>
        [HttpGet("profilgenericdetailByStatus")]
        [RequirePermission("VIEW_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<GxdlmsprofilgenericResponse>>> GetGxdlmsprofilgenericdetailByStatus([FromQuery] GetGxdlmsprofilgenericdetailQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les événements d'un profil générique par numéro de compteur
        /// Filtre les résultats par poste si l'utilisateur a un PosteId assigné
        /// </summary>
        [HttpGet("profilgenericdetailseventByStatus")]
        [RequirePermission("VIEW_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<GxdlmsprofilgenericResponse>>> GetGxdlmsprofilgenericdetailseventByStatus([FromQuery] GetGxdlmsprofilgenericdetailseventQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les détails d'un profil générique par critères multiples
        /// Permet de filtrer par liste de numéros de compteurs, liste d'IDs de profils génériques, liste d'IDs de codes OBIS et date
        /// Filtre les résultats par poste si l'utilisateur a un PosteId assigné
        /// </summary>
        [HttpPost("profilgenericdetailByMultipleCriteria")]
        [RequirePermission("VIEW_DLMS_PROFILE")]
        public async Task<ActionResult<ResponseBase<List<GxdlmsprofilgenericdetailResponse>>>> GetGxdlmsprofilgenericdetailByMultipleCriteria([FromBody] GetGxdlmsprofilgenericdetailByMultipleCriteriaQuery query)
        {
            var userId = GetCurrentUserId();
            query.UserId = userId;
            
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
