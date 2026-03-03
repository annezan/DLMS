using DLMS.API.Helpers;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Commands;
using DLMS_MODELS.EquipementDomain.Queries;
using DLMS_MODELS.EquipementDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomAuthService = DLMS_DAL.Services.IAuthorizationService;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipementController : AuthorizedApiController
    {
        public EquipementController(CustomAuthService authService) : base(authService)
        {
        }

        /// <summary>
        /// Récupère la liste des équipements
        /// - Utilisateur avec PosteId : voit uniquement les équipements de son poste
        /// - Utilisateur sans PosteId : voit tous les équipements
        /// </summary>
        [RequirePermission("VIEW_EQUIPEMENT")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<EquipementResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetEquipementQuery());
            
            if (result == null || !result.IsSuccess)
            {
                return BadRequest(result);
            }

            var userId = GetCurrentUserId();
            var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);

            if (hasPosteAssigne)
            {
                // Filtrer uniquement les équipements de son poste
                var posteId = await AuthService.GetUserPosteIdAsync(userId);
                
                if (posteId.HasValue && result.Data != null)
                {
                    // Filtrer les équipements par poste (via cellule)
                    result.Data = result.Data
                        .Where(e =>
                        {
                            // Vérifier si cet équipement appartient au poste de l'utilisateur
                            return AuthService.UserHasAccessToEquipementAsync(userId, e.Id).Result;
                        })
                        .ToList();
                        
                    result.Message = "Équipements de votre poste récupérés avec succès";
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Récupère un équipement par son ID
        /// Vérifie l'accès à l'équipement
        /// </summary>
        [RequirePermission("VIEW_EQUIPEMENT")]
        [RequireEquipementAccess("Id")]
        [HttpGet("getEquipementById")]
        public async Task<ActionResult<ResponseBase<EquipementResponse>>> GetEquipementById([FromQuery] GetEquipementByIdQuery query)
        {
            var result = await Mediator.Send(new GetEquipementByIdQuery(query.Id));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les équipements d'une cellule
        /// Vérifie l'accès à la cellule
        /// </summary>
        [RequirePermission("VIEW_EQUIPEMENT")]
        [RequireCelluleAccess("CelluleId")]
        [HttpGet("getEquipementByCelluleId")]
        public async Task<ActionResult<ResponseBase<List<EquipementResponse>>>> GetEquipementByCelluleId([FromQuery] GetEquipementByCelluleIdQuery query)
        {
            var result = await Mediator.Send(new GetEquipementByCelluleIdQuery(query.CelluleId));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouvel équipement
        /// Vérifie que la cellule appartient au poste de l'utilisateur (si PosteId assigné)
        /// </summary>
        [RequirePermission("CREATE_EQUIPEMENT")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<EquipementResponse>>> Add([FromBody] EquipementAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un équipement existant
        /// Vérifie l'accès à l'équipement
        /// </summary>
        [RequirePermission("EDIT_EQUIPEMENT")]
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<EquipementResponse>>> Edit([FromBody] EquipementEditCommand command)
        {
            // Vérifier l'accès à cet équipement
            var userId = GetCurrentUserId();
            var hasAccess = await AuthService.UserHasAccessToEquipementAsync(userId, command.Id);

            if (!hasAccess)
            {
                return StatusCode(403, new ResponseBase<EquipementResponse>
                {
                    IsSuccess = false,
                    Message = "Accès refusé : Vous ne pouvez pas modifier cet équipement"
                });
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un équipement
        /// Vérifie l'accès à l'équipement
        /// </summary>
        [RequirePermission("DELETE_EQUIPEMENT")]
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] EquipementDeleteCommand command)
        {
            // Vérifier l'accès à cet équipement
            var userId = GetCurrentUserId();
            var hasAccess = await AuthService.UserHasAccessToEquipementAsync(userId, command.Id);

            if (!hasAccess)
            {
                return StatusCode(403, new ResponseBase<string>
                {
                    IsSuccess = false,
                    Message = "Accès refusé : Vous ne pouvez pas supprimer cet équipement"
                });
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
