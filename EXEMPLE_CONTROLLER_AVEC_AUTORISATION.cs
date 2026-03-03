using DLMS.API.Helpers;
using DLMS_DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DLMS.API.Controllers.Examples;

/// <summary>
/// EXEMPLE : Contrôleur de Postes avec système d'autorisation
/// Ce fichier montre comment utiliser les attributs d'autorisation
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PosteExampleController : ControllerBase
{
    private readonly IAuthorizationService _authService;
    
    public PosteExampleController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Obtenir l'ID de l'utilisateur connecté depuis les claims
    /// </summary>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? User.FindFirst("UserId");
        return Guid.Parse(userIdClaim.Value);
    }

    // =====================================================
    // EXEMPLE 1 : Liste des postes avec filtrage automatique
    // =====================================================
    /// <summary>
    /// Récupère la liste des postes
    /// - Utilisateur avec PosteId : voit uniquement son poste
    /// - Utilisateur sans PosteId : voit tous les postes (si permission)
    /// </summary>
    [RequirePermission("VIEW_POSTE")]
    [HttpGet]
    public async Task<IActionResult> GetAllPostes()
    {
        var userId = GetCurrentUserId();
        var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(userId);

        if (hasPosteAssigne)
        {
            // L'utilisateur avec un poste assigné ne voit que son poste
            var posteId = await _authService.GetUserPosteIdAsync(userId);
            if (!posteId.HasValue)
            {
                return BadRequest(new { Message = "Aucun poste assigné à cet utilisateur" });
            }

            // TODO: Récupérer uniquement le poste de l'utilisateur
            // var poste = await _posteRepository.GetByIdAsync(posteId.Value);
            // return Ok(new List<Poste> { poste });
            
            return Ok(new { Message = $"Poste ID: {posteId.Value}" });
        }

        // Les utilisateurs sans poste assigné voient tous les postes
        // TODO: Récupérer tous les postes
        // var postes = await _posteRepository.GetAllAsync();
        // return Ok(postes);
        
        return Ok(new { Message = "Tous les postes" });
    }

    // =====================================================
    // EXEMPLE 2 : Détails d'un poste avec vérification d'accès
    // =====================================================
    /// <summary>
    /// Récupère les détails d'un poste
    /// - Utilisateur avec PosteId : peut voir uniquement son poste
    /// - Utilisateur sans PosteId : peut voir n'importe quel poste (si permission)
    /// </summary>
    [RequirePosteAccess("id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPosteById(int id)
    {
        // L'attribut RequirePosteAccess a déjà vérifié que l'utilisateur
        // a le droit d'accéder à ce poste
        
        // TODO: Récupérer le poste
        // var poste = await _posteRepository.GetByIdAsync(id);
        // return Ok(poste);
        
        return Ok(new { Message = $"Détails du poste {id}" });
    }

    // =====================================================
    // EXEMPLE 3 : Création d'un poste (Admin uniquement)
    // =====================================================
    /// <summary>
    /// Crée un nouveau poste
    /// Nécessite la permission CREATE_POSTE (généralement Admin uniquement)
    /// </summary>
    [RequirePermission("CREATE_POSTE")]
    [HttpPost]
    public async Task<IActionResult> CreatePoste([FromBody] CreatePosteRequest request)
    {
        // TODO: Créer le poste
        // var poste = await _posteRepository.CreateAsync(request);
        // return Ok(poste);
        
        return Ok(new { Message = "Poste créé avec succès" });
    }

    // =====================================================
    // EXEMPLE 4 : Modification d'un poste avec vérification
    // =====================================================
    /// <summary>
    /// Modifie un poste
    /// Nécessite la permission EDIT_POSTE
    /// </summary>
    [RequirePermission("EDIT_POSTE")]
    [RequirePosteAccess("id")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePoste(int id, [FromBody] UpdatePosteRequest request)
    {
        // Double vérification :
        // 1. L'utilisateur a la permission EDIT_POSTE
        // 2. L'utilisateur a accès à ce poste spécifique
        
        // TODO: Mettre à jour le poste
        // var poste = await _posteRepository.UpdateAsync(id, request);
        // return Ok(poste);
        
        return Ok(new { Message = $"Poste {id} mis à jour" });
    }

    // =====================================================
    // EXEMPLE 5 : Obtenir les cellules d'un poste
    // =====================================================
    /// <summary>
    /// Récupère les cellules d'un poste
    /// - Utilisateur avec PosteId : peut voir uniquement les cellules de son poste
    /// - Utilisateur sans PosteId : peut voir les cellules de n'importe quel poste
    /// </summary>
    [RequirePosteAccess("posteId")]
    [HttpGet("{posteId}/cellules")]
    public async Task<IActionResult> GetCellulesByPoste(int posteId)
    {
        // TODO: Récupérer les cellules du poste
        // var cellules = await _celluleRepository.GetByPosteIdAsync(posteId);
        // return Ok(cellules);
        
        return Ok(new { Message = $"Cellules du poste {posteId}" });
    }

    // =====================================================
    // EXEMPLE 6 : Obtenir les équipements d'un poste
    // =====================================================
    /// <summary>
    /// Récupère tous les équipements d'un poste (via les cellules)
    /// - Utilisateur avec PosteId : peut voir uniquement les équipements de son poste
    /// - Utilisateur sans PosteId : peut voir les équipements de n'importe quel poste
    /// </summary>
    [RequirePosteAccess("posteId")]
    [HttpGet("{posteId}/equipements")]
    public async Task<IActionResult> GetEquipementsByPoste(int posteId)
    {
        // TODO: Récupérer les équipements du poste
        // var equipements = await _equipementRepository.GetByPosteIdAsync(posteId);
        // return Ok(equipements);
        
        return Ok(new { Message = $"Équipements du poste {posteId}" });
    }

    // =====================================================
    // EXEMPLE 7 : Vérification manuelle avec le service
    // =====================================================
    /// <summary>
    /// Exemple de vérification manuelle sans attribut
    /// Utile pour une logique métier complexe
    /// </summary>
    [HttpGet("{posteId}/statistiques")]
    public async Task<IActionResult> GetStatistiques(int posteId)
    {
        var userId = GetCurrentUserId();
        
        // Vérification manuelle de l'accès
        var hasAccess = await _authService.UserHasAccessToPosteAsync(userId, posteId);
        
        if (!hasAccess)
        {
            return Forbid();
        }

        // Vérification de permissions spécifiques
        var canViewReports = await _authService.UserHasPermissionAsync(userId, "VIEW_REPORTS");
        
        if (!canViewReports)
        {
            return StatusCode(403, new { Message = "Permission VIEW_REPORTS requise" });
        }

        // TODO: Calculer les statistiques
        return Ok(new { Message = $"Statistiques du poste {posteId}" });
    }

    // =====================================================
    // EXEMPLE 8 : Obtenir les permissions de l'utilisateur
    // =====================================================
    /// <summary>
    /// Récupère toutes les permissions de l'utilisateur connecté
    /// Utile pour l'interface utilisateur (affichage conditionnel)
    /// </summary>
    [HttpGet("my-permissions")]
    public async Task<IActionResult> GetMyPermissions()
    {
        var userId = GetCurrentUserId();
        var permissions = await _authService.GetUserPermissionsAsync(userId);
        
        return Ok(new
        {
            UserId = userId,
            Permissions = permissions
        });
    }
}

// =====================================================
// MODÈLES DE REQUÊTE (à adapter selon vos besoins)
// =====================================================

public class CreatePosteRequest
{
    public string Numero { get; set; }
    public string Libelle { get; set; }
    public string Adresse { get; set; }
}

public class UpdatePosteRequest
{
    public string? Numero { get; set; }
    public string? Libelle { get; set; }
    public string? Adresse { get; set; }
}

