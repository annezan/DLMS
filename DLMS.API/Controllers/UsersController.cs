using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Queries;
using DLMS_MODELS.UsersDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    /// <summary>
    /// Contrôleur pour la gestion des utilisateurs
    /// Note: Les utilisateurs sont gérés globalement, pas de filtrage par poste
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : AuthorizedApiController
    {
        /// <summary>
        /// Récupère la liste de tous les utilisateurs
        /// </summary>
        [HttpGet]
        [RequirePermission("VIEW_USER")]
        public async Task<ActionResult<ResponseBase<List<UsersResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetUsersQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouvel utilisateur
        /// </summary>
        [HttpPost("add")]
        [RequirePermission("CREATE_USER")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> Add([FromBody] UsersAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un utilisateur par son ID
        /// </summary>
        [HttpGet("getuserbyId")]
        [RequirePermission("VIEW_USER")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> GetUserById([FromQuery] Guid id)
        {
            var query = new GetUsersByIdQuery(id);
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Récupère un utilisateur par son email
        /// </summary>
        [HttpGet("getuserbyemail")]
        [RequirePermission("VIEW_USER")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> GetUserByEmail([FromQuery] string email)
        {
            var query = new GetUsersByEmailQuery(email);
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Débloque le compte d'un utilisateur
        /// </summary>
        [HttpPost("dislockuseraccount")]
        [RequirePermission("UNLOCK_USER")]
        public async Task<ActionResult<ResponseBase<string>>> DislockUserAccount([FromBody] UsersDislockAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un utilisateur existant
        /// </summary>
        [HttpPost("edit")]
        [RequirePermission("EDIT_USER")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> Edit([FromBody] UsersEditCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un utilisateur
        /// </summary>
        [HttpPost("delete")]
        [RequirePermission("DELETE_USER")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] UsersDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Assigne ou retire un poste à un utilisateur
        /// </summary>
        [HttpPost("assign-poste")]
        [RequirePermission("ASSIGN_POSTE")]
        public async Task<ActionResult<ResponseBase<UsersResponse>>> AssignPoste([FromBody] UsersAssignPosteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Gère la demande de mot de passe oublié
        /// </summary>
        [HttpPost("forgot-password")]
        [AllowAnonymous] // Permet aux utilisateurs non authentifiés de demander un nouveau mot de passe
        public async Task<ActionResult<ResponseBase<UsersResponse>>> ForgotPassword([FromBody] UsersForgotPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
