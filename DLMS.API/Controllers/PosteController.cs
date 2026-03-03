using DLMS.API.Helpers;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Commands;
using DLMS_MODELS.PosteDomain.Queries;
using DLMS_MODELS.PosteDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomAuthService = DLMS_DAL.Services.IAuthorizationService;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PosteController : AuthorizedApiController
    {
        public PosteController(CustomAuthService authService) : base(authService)
        {
        }

        /// <summary>
        /// Récupère la liste des postes
        /// - Utilisateur avec PosteId : voit uniquement son poste
        /// - Utilisateur sans PosteId : voit tous les postes
        /// </summary>
        [RequirePermission("VIEW_POSTE")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<PosteResponse>>>> Get()
        {
            var userId = GetCurrentUserId();
            var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);

            if (hasPosteAssigne)
            {
                // Filtrer pour montrer uniquement son poste
                var posteId = await AuthService.GetUserPosteIdAsync(userId);
                if (!posteId.HasValue)
                {
                    return BadRequest(new ResponseBase<List<PosteResponse>>
                    {
                        IsSuccess = false,
                        Message = "Aucun poste assigné"
                    });
                }

                var singlePoste = await Mediator.Send(new GetPosteByIdQuery(posteId.Value));
                if (singlePoste == null || !singlePoste.IsSuccess)
                {
                    return BadRequest(singlePoste);
                }

                return Ok(new ResponseBase<List<PosteResponse>>
                {
                    Data = new List<PosteResponse> { singlePoste.Data },
                    IsSuccess = true,
                    Message = "Votre poste récupéré avec succès"
                });
            }

            // Sinon, retourner tous les postes
            var result = await Mediator.Send(new GetPosteQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un poste par son ID
        /// Vérifie l'accès au poste (utilisateur avec PosteId ne peut voir que son poste)
        /// </summary>
        [RequirePermission("VIEW_POSTE")]
        [RequirePosteAccess("IdPoste")]
        [HttpGet("getPosteById")]
        public async Task<ActionResult<ResponseBase<PosteResponse>>> GetPosteById([FromQuery] GetPosteByIdQuery query)
        {
            var result = await Mediator.Send(new GetPosteByIdQuery(query.IdPoste));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouveau poste
        /// </summary>
        [RequirePermission("CREATE_POSTE")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<PosteResponse>>> Add([FromBody] PosteAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un poste existant
        /// Vérifie que l'utilisateur a accès à ce poste
        /// </summary>
        [RequirePermission("EDIT_POSTE")]
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<PosteResponse>>> Edit([FromBody] PosteEditCommand command)
        {
            // Vérifier l'accès au poste
            var userId = GetCurrentUserId();
            var hasAccess = await AuthService.UserHasAccessToPosteAsync(userId, command.Id);

            if (!hasAccess)
            {
                return StatusCode(403, new ResponseBase<PosteResponse>
                {
                    IsSuccess = false,
                    Message = "Accès refusé : Vous ne pouvez pas modifier ce poste"
                });
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un poste
        /// Vérifie que l'utilisateur a accès à ce poste
        /// </summary>
        [RequirePermission("DELETE_POSTE")]
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] PosteDeleteCommand command)
        {
            // Vérifier l'accès au poste
            var userId = GetCurrentUserId();
            var hasAccess = await AuthService.UserHasAccessToPosteAsync(userId, command.Id);

            if (!hasAccess)
            {
                return StatusCode(403, new ResponseBase<string>
                {
                    IsSuccess = false,
                    Message = "Accès refusé : Vous ne pouvez pas supprimer ce poste"
                });
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
