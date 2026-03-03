using DLMS_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DLMS.API.Helpers;

/// <summary>
/// Attribut pour vérifier l'accès à un équipement spécifique
/// Un utilisateur avec PosteId assigné ne peut accéder qu'aux équipements de son poste
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RequireEquipementAccessAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _equipementIdParameterName;

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="equipementIdParameterName">Nom du paramètre contenant l'ID de l'équipement (par défaut "equipementId")</param>
    public RequireEquipementAccessAttribute(string equipementIdParameterName = "equipementId")
    {
        _equipementIdParameterName = equipementIdParameterName;
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

        // Récupérer l'ID de l'équipement depuis les paramètres de la route, query string ou body
        int equipementId = 0;
        
        // Essayer depuis la route
        if (context.RouteData.Values.ContainsKey(_equipementIdParameterName))
        {
            int.TryParse(context.RouteData.Values[_equipementIdParameterName]?.ToString(), out equipementId);
        }
        
        // Essayer depuis la query string si non trouvé
        if (equipementId == 0 && context.HttpContext.Request.Query.ContainsKey(_equipementIdParameterName))
        {
            int.TryParse(context.HttpContext.Request.Query[_equipementIdParameterName].ToString(), out equipementId);
        }

        if (equipementId == 0)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Message = $"Le paramètre '{_equipementIdParameterName}' est requis"
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

        // Vérifier si l'utilisateur a accès à cet équipement
        var hasAccess = await authService.UserHasAccessToEquipementAsync(userId, equipementId);

        if (!hasAccess)
        {
            context.Result = new ForbidResult();
        }
    }
}

