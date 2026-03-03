using DLMS.API.Helpers;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Commands;
using DLMS_MODELS.CelluleDomain.Queries;
using DLMS_MODELS.CelluleDomain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomAuthService = DLMS_DAL.Services.IAuthorizationService;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CelluleController : AuthorizedApiController
    {
        public CelluleController(CustomAuthService authService) : base(authService)
        {
        }

        /// <summary>
        /// Récupère la liste des cellules
        /// - Utilisateur avec PosteId : voit uniquement les cellules de son poste
        /// - Utilisateur sans PosteId : voit toutes les cellules
        /// </summary>
        [RequirePermission("VIEW_CELLULE")]
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<CelluleResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetCelluleQuery());
            
            if (result == null || !result.IsSuccess)
            {
                return BadRequest(result);
            }

            var userId = GetCurrentUserId();
            var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);

            if (hasPosteAssigne)
            {
                // Filtrer uniquement les cellules de son poste
                var posteId = await AuthService.GetUserPosteIdAsync(userId);
                
                if (posteId.HasValue && result.Data != null)
                {
                    result.Data = result.Data
                        .Where(c => c.PosteId == posteId.Value)
                        .ToList();
                        
                    result.Message = "Cellules de votre poste récupérées avec succès";
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Récupère une cellule par son ID
        /// Vérifie l'accès à la cellule
        /// </summary>
        [RequirePermission("VIEW_CELLULE")]
        [RequireCelluleAccess("IdCellule")]
        [HttpGet("getCelluleById")]
        public async Task<ActionResult<ResponseBase<CelluleResponse>>> GetCelluleById([FromQuery] GetCelluleByIdQuery query)
        {
            var result = await Mediator.Send(new GetCelluleByIdQuery(query.IdCellule));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère les cellules d'un poste
        /// Vérifie l'accès au poste
        /// </summary>
        [RequirePermission("VIEW_CELLULE")]
        [RequirePosteAccess("PosteId")]
        [HttpGet("getCelluleByPosteId")]
        public async Task<ActionResult<ResponseBase<List<CelluleResponse>>>> GetCelluleByPosteId([FromQuery] GetCelluleByPosteIdQuery query)
        {
            var result = await Mediator.Send(new GetCelluleByPosteIdQuery(query.PosteId));
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée une nouvelle cellule
        /// Vérifie que le poste appartient à l'utilisateur (si PosteId assigné)
        /// </summary>
        [RequirePermission("CREATE_CELLULE")]
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<CelluleResponse>>> Add([FromBody] CelluleAddCommand command)
        {
            var userId = GetCurrentUserId();
            var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);

            if (hasPosteAssigne)
            {
                // Vérifier que la cellule est créée dans le poste de l'utilisateur
                var userPosteId = await AuthService.GetUserPosteIdAsync(userId);
                
                if (command.PosteId != userPosteId)
                {
                    return StatusCode(403, new ResponseBase<CelluleResponse>
                    {
                        IsSuccess = false,
                        Message = "Accès refusé : Vous ne pouvez créer des cellules que dans votre poste"
                    });
                }
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie une cellule existante
        /// Vérifie l'accès à la cellule
        /// </summary>
        [RequirePermission("EDIT_CELLULE")]
        [HttpPut("edit")]
        public async Task<ActionResult<ResponseBase<CelluleResponse>>> Edit([FromBody] CelluleEditCommand command)
        {
            // Vérifier l'accès à cette cellule
            var userId = GetCurrentUserId();
            var hasAccess = await AuthService.UserHasAccessToCelluleAsync(userId, command.Id);

            if (!hasAccess)
            {
                return StatusCode(403, new ResponseBase<CelluleResponse>
                {
                    IsSuccess = false,
                    Message = "Accès refusé : Vous ne pouvez pas modifier cette cellule"
                });
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime une cellule
        /// Vérifie l'accès à la cellule
        /// </summary>
        [RequirePermission("DELETE_CELLULE")]
        [HttpDelete("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] CelluleDeleteCommand command)
        {
            // Vérifier l'accès à cette cellule
            var userId = GetCurrentUserId();
            var hasAccess = await AuthService.UserHasAccessToCelluleAsync(userId, command.Id);

            if (!hasAccess)
            {
                return StatusCode(403, new ResponseBase<string>
                {
                    IsSuccess = false,
                    Message = "Accès refusé : Vous ne pouvez pas supprimer cette cellule"
                });
            }

            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
