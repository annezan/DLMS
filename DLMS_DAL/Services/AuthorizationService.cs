using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS_DAL.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly DLMSDBContext _context;

    public AuthorizationService(DLMSDBContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Vérifie si l'utilisateur a la permission spécifiée
    /// </summary>
    public async Task<bool> UserHasPermissionAsync(Guid userId, string permissionAction)
    {
        var user = await _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permissions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user?.Role == null)
            return false;

        // Vérifier si le rôle a la permission demandée
        return user.Role.RolePermissions
            .Any(rp => rp.Permissions.Action == permissionAction);
    }

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un poste spécifique
    /// Règle : Si l'utilisateur a un PosteId assigné, il ne peut accéder qu'à son poste
    ///        Sinon, il peut accéder à tous les postes (si permission VIEW_POSTE)
    /// </summary>
    public async Task<bool> UserHasAccessToPosteAsync(Guid userId, int posteId)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return false;

        // Si l'utilisateur a un poste assigné, il ne peut accéder qu'à son poste
        if (user.PosteId.HasValue)
        {
            return user.PosteId.Value == posteId;
        }

        // Si l'utilisateur n'a pas de poste assigné, vérifier les permissions générales
        return await UserHasPermissionAsync(userId, "VIEW_POSTE");
    }

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un équipement
    /// Règle : Si l'utilisateur a un PosteId assigné, il ne peut voir que les équipements qui ont des compteurs associés à des cellules de son poste
    ///        Sinon, il peut accéder à tous les équipements (si permission VIEW_EQUIPEMENT)
    /// </summary>
    public async Task<bool> UserHasAccessToEquipementAsync(Guid userId, int equipementId)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return false;

        // Si l'utilisateur a un poste assigné, il ne peut accéder qu'aux équipements qui ont des compteurs dans des cellules de son poste
        if (user.PosteId.HasValue)
        {
            // Vérifier que l'équipement a des compteurs qui appartiennent à des cellules du poste de l'utilisateur
            var equipementBelongsToPoste = await _context.Equipement
                .Include(e => e.EquipementCompteur)
                    .ThenInclude(ec => ec.Compteur)
                        .ThenInclude(c => c.CompteurCellules)
                            .ThenInclude(cc => cc.Cellule)
                .AnyAsync(e => e.Id == equipementId && 
                              e.EquipementCompteur.Any(ec => ec.Compteur.CompteurCellules.Any(cc => cc.Cellule.PosteId == user.PosteId.Value)));

            return equipementBelongsToPoste;
        }

        // Si l'utilisateur n'a pas de poste assigné, vérifier les permissions générales
        return await UserHasPermissionAsync(userId, "VIEW_EQUIPEMENT");
    }

    /// <summary>
    /// Obtient toutes les permissions d'un utilisateur
    /// </summary>
    public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.Role)
                .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permissions)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user?.Role == null)
            return new List<string>();

        return user.Role.RolePermissions
            .Select(rp => rp.Permissions.Action)
            .ToList();
    }

    /// <summary>
    /// Vérifie si l'utilisateur a un poste assigné
    /// (utilisé pour savoir si l'utilisateur est limité à un poste spécifique)
    /// </summary>
    public async Task<bool> UserHasPosteAssignedAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.PosteId.HasValue ?? false;
    }

    /// <summary>
    /// Obtient l'ID du poste assigné à l'utilisateur
    /// </summary>
    public async Task<int?> GetUserPosteIdAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user?.PosteId;
    }

    /// <summary>
    /// Vérifie si l'utilisateur a accès à une cellule spécifique
    /// Règle : Si l'utilisateur a un PosteId assigné, la cellule doit appartenir à son poste
    /// </summary>
    public async Task<bool> UserHasAccessToCelluleAsync(Guid userId, int celluleId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return false;

        // Si l'utilisateur a un poste assigné, vérifier que la cellule appartient à son poste
        if (user.PosteId.HasValue)
        {
            var celluleBelongsToPoste = await _context.Cellule
                .AnyAsync(c => c.Id == celluleId && c.PosteId == user.PosteId.Value);

            return celluleBelongsToPoste;
        }

        // Si l'utilisateur n'a pas de poste assigné, vérifier les permissions générales
        return await UserHasPermissionAsync(userId, "VIEW_CELLULE");
    }

    /// <summary>
    /// Vérifie si l'utilisateur a accès à une commande spécifique
    /// Règle : Une commande est accessible si tous ses compteurs sont associés à des cellules du poste de l'utilisateur
    ///        Si l'utilisateur n'a pas de poste assigné, il peut accéder à toutes les commandes (avec permission VIEW_COMMANDE)
    /// </summary>
    public async Task<bool> UserHasAccessToCommandeAsync(Guid userId, int commandeId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return false;

        // Si l'utilisateur a un poste assigné, vérifier que tous les compteurs de la commande
        // sont associés à des cellules de son poste
        if (user.PosteId.HasValue)
        {
            // Récupérer la commande avec ses relations
            var commande = await _context.Commandes
                .Include(cmd => cmd.CommandeCompteur)
                    .ThenInclude(cc => cc.Compteur)
                        .ThenInclude(c => c.CompteurCellules)
                            .ThenInclude(cc => cc.Cellule)
                .FirstOrDefaultAsync(cmd => cmd.Id == commandeId);

            if (commande == null)
                return false;

            // Si la commande n'a pas de compteurs associés, refuser l'accès
            if (commande.CommandeCompteur == null || !commande.CommandeCompteur.Any())
                return false;

            // Vérifier que TOUS les compteurs de la commande appartiennent au poste de l'utilisateur
            foreach (var commandeCompteur in commande.CommandeCompteur)
            {
                // Si le compteur n'a pas de cellules associées, refuser l'accès
                if (commandeCompteur.Compteur?.CompteurCellules == null || 
                    !commandeCompteur.Compteur.CompteurCellules.Any())
                    return false;

                // Vérifier si au moins une cellule du compteur appartient au poste de l'utilisateur
                var hasCelluleInPoste = commandeCompteur.Compteur.CompteurCellules
                    .Any(cc => cc.Cellule?.PosteId == user.PosteId.Value);

                // Si aucune cellule n'appartient au poste, refuser l'accès
                if (!hasCelluleInPoste)
                    return false;
            }

            // Tous les compteurs appartiennent au poste, autoriser l'accès
            return true;
        }

        // Si l'utilisateur n'a pas de poste assigné, vérifier les permissions générales
        return await UserHasPermissionAsync(userId, "VIEW_COMMANDE");
    }

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un compteur spécifique
    /// Règle : Un compteur est accessible s'il est associé à une cellule du poste de l'utilisateur
    ///        Si l'utilisateur n'a pas de poste assigné, il peut accéder à tous les compteurs (avec permission VIEW_COMPTEUR)
    /// </summary>
    public async Task<bool> UserHasAccessToCompteurAsync(Guid userId, int compteurId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return false;

        // Si l'utilisateur a un poste assigné, vérifier que le compteur
        // est associé à une cellule de son poste
        if (user.PosteId.HasValue)
        {
            // Récupérer le compteur avec ses relations aux cellules
            var compteur = await _context.Compteur
                .Include(c => c.CompteurCellules)
                    .ThenInclude(cc => cc.Cellule)
                .FirstOrDefaultAsync(c => c.Id == compteurId);

            if (compteur == null)
                return false;

            // Vérifier si le compteur est associé à une cellule du poste de l'utilisateur
            var hasCelluleInPoste = compteur.CompteurCellules?
                .Any(cc => cc.Cellule?.PosteId == user.PosteId.Value) == true;

            return hasCelluleInPoste;
        }

        // Si l'utilisateur n'a pas de poste assigné, vérifier les permissions générales
        return await UserHasPermissionAsync(userId, "VIEW_COMPTEUR");
    }

    /// <summary>
    /// Vérifie si l'utilisateur a accès à un compteur par son NumeroCompteur (string)
    /// Utilisé pour les AssociationKeys qui référencent les compteurs par NumeroCompteur
    /// </summary>
    public async Task<bool> UserHasAccessToCompteurByCompteurIdAsync(Guid userId, string numeroCompteur)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return false;

        if (user.PosteId.HasValue)
        {
            // Trouver le compteur par son NumeroCompteur (string) avec ses relations aux cellules
            var compteur = await _context.Compteur
                .Include(c => c.CompteurCellules)
                    .ThenInclude(cc => cc.Cellule)
                .FirstOrDefaultAsync(c => c.NumeroCompteur == numeroCompteur);

            if (compteur == null)
                return false;

            // Vérifier si le compteur est associé à une cellule du poste de l'utilisateur
            var hasCelluleInPoste = compteur.CompteurCellules?
                .Any(cc => cc.Cellule?.PosteId == user.PosteId.Value) == true;

            return hasCelluleInPoste;
        }
        else
        {
            // Si l'utilisateur n'a pas de poste assigné, il peut accéder à tous les compteurs
            // à condition d'avoir la permission générale de voir les compteurs.
            return await UserHasPermissionAsync(userId, "VIEW_COMPTEUR");
        }
    }
}
