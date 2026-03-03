using DLMS_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DLMS.API.Helpers;

/// <summary>
/// Attribut pour vérifier l'accès à un poste spécifique
/// Un utilisateur avec PosteId assigné ne peut accéder qu'à son propre poste
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RequirePosteAccessAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _posteIdParameterName;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="posteIdParameterName">Nom du paramètre contenant l'ID du poste (par défaut "posteId")</param>
    public RequirePosteAccessAttribute(string posteIdParameterName = "posteId")
    {
        _posteIdParameterName = posteIdParameterName;
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

        // Récupérer l'ID du poste depuis les paramètres de la route, query string ou body
        int posteId = 0;
        
        // Essayer depuis la route
        if (context.RouteData.Values.ContainsKey(_posteIdParameterName))
        {
            int.TryParse(context.RouteData.Values[_posteIdParameterName]?.ToString(), out posteId);
        }
        
        // Essayer depuis la query string si non trouvé
        if (posteId == 0 && context.HttpContext.Request.Query.ContainsKey(_posteIdParameterName))
        {
            int.TryParse(context.HttpContext.Request.Query[_posteIdParameterName].ToString(), out posteId);
        }

        if (posteId == 0)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Message = $"Le paramètre '{_posteIdParameterName}' est requis"
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

        // Vérifier si l'utilisateur a accès à ce poste
        var hasAccess = await authService.UserHasAccessToPosteAsync(userId, posteId);

        if (!hasAccess)
        {
            context.Result = new ForbidResult();
        }
    }
}

