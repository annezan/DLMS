using DLMS_DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DLMS.API.Helpers;

/// <summary>
/// Attribut pour exiger une permission spécifique pour accéder à une action
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _permissionAction;

    public RequirePermissionAttribute(string permissionAction)
    {
        _permissionAction = permissionAction;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Récupérer l'ID de l'utilisateur depuis les claims
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

        // Récupérer le service d'autorisation
        var authService = context.HttpContext.RequestServices
            .GetService<IAuthorizationService>();

        if (authService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        // Vérifier si l'utilisateur a la permission
        var hasPermission = await authService.UserHasPermissionAsync(userId, _permissionAction);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}

