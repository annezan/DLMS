using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CustomAuthService = DLMS_DAL.Services.IAuthorizationService;

namespace DLMS.API.Controllers;

/// <summary>
/// Contrôleur de base avec fonctionnalités d'autorisation
/// </summary>
[Authorize]
public class AuthorizedApiController : ApiController
{
    private CustomAuthService _authService;

    /// <summary>
    /// Service d'autorisation récupéré automatiquement depuis HttpContext
    /// </summary>
    protected CustomAuthService AuthService => _authService ??= HttpContext.RequestServices.GetService<CustomAuthService>();

    /// <summary>
    /// Constructeur par défaut (sans paramètres)
    /// </summary>
    public AuthorizedApiController()
    {
    }

    /// <summary>
    /// Constructeur avec injection de dépendance (pour compatibilité avec les contrôleurs existants)
    /// </summary>
    public AuthorizedApiController(CustomAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Récupère l'ID de l'utilisateur connecté depuis le token JWT
    /// </summary>
    protected Guid GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) 
                    ?? User.FindFirst("UserId")
                    ?? User.FindFirst(ClaimsKey.UtilisateurId);
        
        if (claim == null || !Guid.TryParse(claim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Utilisateur non authentifié");
        }

        return userId;
    }

    /// <summary>
    /// Vérifie si l'utilisateur connecté a un poste assigné
    /// </summary>
    protected async Task<bool> CurrentUserHasPosteAssignedAsync()
    {
        var userId = GetCurrentUserId();
        return await AuthService.UserHasPosteAssignedAsync(userId);
    }

    /// <summary>
    /// Récupère l'ID du poste assigné à l'utilisateur connecté
    /// </summary>
    protected async Task<int?> GetCurrentUserPosteIdAsync()
    {
        var userId = GetCurrentUserId();
        return await AuthService.GetUserPosteIdAsync(userId);
    }
}

