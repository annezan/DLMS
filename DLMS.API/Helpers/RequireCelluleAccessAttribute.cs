using DLMS_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DLMS.API.Helpers;

/// <summary>
/// Attribut pour vérifier l'accès à une cellule spécifique
/// Un utilisateur avec PosteId assigné ne peut accéder qu'aux cellules de son poste
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RequireCelluleAccessAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _celluleIdParameterName;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="celluleIdParameterName">Nom du paramètre contenant l'ID de la cellule (par défaut "celluleId")</param>
    public RequireCelluleAccessAttribute(string celluleIdParameterName = "celluleId")
    {
        _celluleIdParameterName = celluleIdParameterName;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Récupérer l'ID de l'utilisateur
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? context.HttpContext.User.FindFirst("UserId");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                Message = "Utilisateur non authentifié"
            });
            return;
        }

        // Récupérer l'ID de la cellule depuis les paramètres de la route, query string ou body
        int celluleId = 0;
        
        // Essayer depuis la route
        if (context.RouteData.Values.ContainsKey(_celluleIdParameterName))
        {
            int.TryParse(context.RouteData.Values[_celluleIdParameterName]?.ToString(), out celluleId);
        }
        
        // Essayer depuis la query string si non trouvé
        if (celluleId == 0 && context.HttpContext.Request.Query.ContainsKey(_celluleIdParameterName))
        {
            int.TryParse(context.HttpContext.Request.Query[_celluleIdParameterName].ToString(), out celluleId);
        }

        if (celluleId == 0)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Message = $"Le paramètre '{_celluleIdParameterName}' est requis"
            });
            return;
        }

        // Récupérer le service d'autorisation
        var authService = context.HttpContext.RequestServices
            .GetService<IAuthorizationService>();

        if (authService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        // Vérifier si l'utilisateur a accès à cette cellule
        var hasAccess = await authService.UserHasAccessToCelluleAsync(userId, celluleId);

        if (!hasAccess)
        {
            context.Result = new ForbidResult();
        }
    }
}

