using DLMS_MODELS.UsersDomain.Entities;

namespace DLMS_DAL.Services;

public interface IAuthorizationService
{
    /// <summary>
    /// Vérifie si l'utilisateur a la permission spécifiée
    /// </summary>
    Task<bool> UserHasPermissionAsync(Guid userId, string permissionAction);

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un poste spécifique
    /// </summary>
    Task<bool> UserHasAccessToPosteAsync(Guid userId, int posteId);

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un équipement (via le poste)
    /// </summary>
    Task<bool> UserHasAccessToEquipementAsync(Guid userId, int equipementId);

    /// <summary>
    /// Obtient toutes les permissions d'un utilisateur
    /// </summary>
    Task<List<string>> GetUserPermissionsAsync(Guid userId);

    /// <summary>
    /// Vérifie si l'utilisateur a un poste assigné
    /// </summary>
    Task<bool> UserHasPosteAssignedAsync(Guid userId);

    /// <summary>
    /// Obtient l'ID du poste assigné à l'utilisateur
    /// </summary>
    Task<int?> GetUserPosteIdAsync(Guid userId);

    /// <summary>
    /// Vérifie si l'utilisateur a accès à une cellule spécifique
    /// </summary>
    Task<bool> UserHasAccessToCelluleAsync(Guid userId, int celluleId);

    /// <summary>
    /// Vérifie si l'utilisateur a accès à une commande spécifique
    /// Une commande est accessible si tous ses compteurs appartiennent à des équipements du poste de l'utilisateur
    /// </summary>
    Task<bool> UserHasAccessToCommandeAsync(Guid userId, int commandeId);

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un compteur spécifique
    /// Un compteur est accessible si au moins un de ses équipements appartient au poste de l'utilisateur
    /// </summary>
    Task<bool> UserHasAccessToCompteurAsync(Guid userId, int compteurId);

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un compteur par son NumeroCompteur (string)
    /// Utilisé pour les AssociationKeys qui référencent les compteurs par NumeroCompteur
    /// </summary>
    Task<bool> UserHasAccessToCompteurByCompteurIdAsync(Guid userId, string numeroCompteur);
}

