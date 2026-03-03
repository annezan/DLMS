# Système d'Autorisation et de Permissions DLMS

## 📋 Vue d'ensemble

Ce système implémente un modèle de contrôle d'accès basé sur les rôles (RBAC) avec des règles métier pour limiter l'accès par poste.

## 🔑 Règles métier

1. **Un utilisateur a un seul rôle**
2. **Un rôle a plusieurs permissions** (définies par l'administrateur)
3. **Un utilisateur avec un poste assigné (`PosteId`) ne peut voir que ce qui concerne son poste**
   - Accès limité à son poste assigné
   - Accès aux cellules de son poste
   - Accès aux équipements de son poste (via les cellules)
   - Cette règle s'applique **quel que soit le rôle** de l'utilisateur

## 🏗️ Architecture

### Entités

#### User
```csharp
public class User : ModelBase
{
    // ... autres propriétés
    public Guid? RoleId { get; set; }
    public Role? Role { get; set; }
    
    // Relation avec Poste (pour limiter l'accès à un poste spécifique)
    public int? PosteId { get; set; }
    public Poste? Poste { get; set; }
}
```

#### Role
```csharp
public class Role : ModelBase
{
    public string Libelle { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public ICollection<RolePermission> RolePermissions { get; set; }
}
```

#### Permission
```csharp
public class Permission : ModelBase
{
    public string Libelle { get; set; }
    public string Description { get; set; }
    public string Action { get; set; } // Ex: "VIEW_POSTE", "EDIT_EQUIPEMENT"
    public ICollection<RolePermission> RolePermissions { get; set; }
}
```

#### RolePermission
Table de liaison many-to-many entre Role et Permission.

### Services

#### IAuthorizationService

Service principal pour la vérification des autorisations :

```csharp
public interface IAuthorizationService
{
    Task<bool> UserHasPermissionAsync(Guid userId, string permissionAction);
    Task<bool> UserHasAccessToPosteAsync(Guid userId, int posteId);
    Task<bool> UserHasAccessToEquipementAsync(Guid userId, int equipementId);
    Task<List<string>> GetUserPermissionsAsync(Guid userId);
    Task<bool> UserHasPosteAssignedAsync(Guid userId);
    Task<int?> GetUserPosteIdAsync(Guid userId);
}
```

## 🛡️ Attributs d'autorisation

### 1. RequirePermissionAttribute

Vérifie si l'utilisateur a une permission spécifique.

**Utilisation :**
```csharp
[RequirePermission("VIEW_USERS")]
[HttpGet]
public async Task<IActionResult> GetAllUsers()
{
    // ...
}
```

### 2. RequirePosteAccessAttribute

Vérifie l'accès à un poste spécifique. Les utilisateurs avec PosteId assigné ne peuvent accéder qu'à leur propre poste.

**Utilisation :**
```csharp
[RequirePosteAccess("posteId")]
[HttpGet("postes/{posteId}")]
public async Task<IActionResult> GetPoste(int posteId)
{
    // ...
}
```

### 3. RequireEquipementAccessAttribute

Vérifie l'accès à un équipement. Les utilisateurs avec PosteId assigné ne peuvent accéder qu'aux équipements de leur poste.

**Utilisation :**
```csharp
[RequireEquipementAccess("equipementId")]
[HttpGet("equipements/{equipementId}")]
public async Task<IActionResult> GetEquipement(int equipementId)
{
    // ...
}
```

## 📝 Exemples d'utilisation

### Exemple 1 : Contrôleur de Postes

```csharp
[ApiController]
[Route("api/[controller]")]
public class PosteController : ControllerBase
{
    private readonly IAuthorizationService _authService;
    
    public PosteController(IAuthorizationService authService)
    {
        _authService = authService;
    }
    
    // Tous les utilisateurs avec la permission peuvent lister les postes
    [RequirePermission("VIEW_POSTE")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Pour un utilisateur avec un poste assigné, filtrer uniquement son poste
        var userId = GetCurrentUserId();
        var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(userId);
        
        if (hasPosteAssigne)
        {
            var posteId = await _authService.GetUserPosteIdAsync(userId);
            // Retourner uniquement le poste de l'utilisateur
        }
        
        // Sinon retourner tous les postes
    }
    
    // Seuls les utilisateurs ayant accès au poste peuvent le voir
    [RequirePosteAccess("id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Un utilisateur avec PosteId ne peut voir que son poste
        // Les autres utilisateurs avec VIEW_POSTE peuvent voir n'importe quel poste
    }
}
```

### Exemple 2 : Contrôleur d'Équipements

```csharp
[ApiController]
[Route("api/[controller]")]
public class EquipementController : ControllerBase
{
    // Un utilisateur avec PosteId ne peut voir que les équipements de son poste
    [RequireEquipementAccess("id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // ...
    }
    
    // Créer un équipement dans une cellule
    [RequirePermission("CREATE_EQUIPEMENT")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEquipementCommand command)
    {
        // Vérifier que l'utilisateur avec poste assigné crée l'équipement dans son poste
        var userId = GetCurrentUserId();
        var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(userId);
        
        if (hasPosteAssigne)
        {
            // Vérifier que la cellule appartient au poste de l'utilisateur
            // ...
        }
    }
}
```

## 🗄️ Migration de base de données

Pour appliquer les changements à la base de données :

```bash
cd DLMS_DAL
dotnet ef migrations add AddUserPosteRelation --startup-project ../DLMS.API
dotnet ef database update --startup-project ../DLMS.API
```

## 📊 Données de seed recommandées

### Rôles

```sql
-- Exemples de rôles (à adapter selon vos besoins)

-- Administrateur système
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (NEWID(), 'Administrateur', 'ADMIN', 'Accès complet au système', GETDATE(), 0);

-- Gestionnaire de poste (exemple)
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (NEWID(), 'Gestionnaire de Poste', 'GESTIONNAIRE', 'Gestion d''un poste spécifique', GETDATE(), 0);

-- Technicien
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (NEWID(), 'Technicien', 'TECHNICIEN', 'Accès aux opérations techniques', GETDATE(), 0);

-- Superviseur
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (NEWID(), 'Superviseur', 'SUPERVISEUR', 'Supervision de plusieurs postes', GETDATE(), 0);
```

### Permissions

```sql
-- Permissions pour les postes
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted)
VALUES 
(NEWID(), 'Voir les postes', 'Permet de visualiser les postes', 'VIEW_POSTE', GETDATE(), 0),
(NEWID(), 'Créer un poste', 'Permet de créer un nouveau poste', 'CREATE_POSTE', GETDATE(), 0),
(NEWID(), 'Modifier un poste', 'Permet de modifier un poste', 'EDIT_POSTE', GETDATE(), 0),
(NEWID(), 'Supprimer un poste', 'Permet de supprimer un poste', 'DELETE_POSTE', GETDATE(), 0);

-- Permissions pour les équipements
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted)
VALUES 
(NEWID(), 'Voir les équipements', 'Permet de visualiser les équipements', 'VIEW_EQUIPEMENT', GETDATE(), 0),
(NEWID(), 'Créer un équipement', 'Permet de créer un équipement', 'CREATE_EQUIPEMENT', GETDATE(), 0),
(NEWID(), 'Modifier un équipement', 'Permet de modifier un équipement', 'EDIT_EQUIPEMENT', GETDATE(), 0),
(NEWID(), 'Supprimer un équipement', 'Permet de supprimer un équipement', 'DELETE_EQUIPEMENT', GETDATE(), 0);

-- Permissions pour les utilisateurs
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted)
VALUES 
(NEWID(), 'Voir les utilisateurs', 'Permet de visualiser les utilisateurs', 'VIEW_USERS', GETDATE(), 0),
(NEWID(), 'Créer un utilisateur', 'Permet de créer un utilisateur', 'CREATE_USER', GETDATE(), 0),
(NEWID(), 'Modifier un utilisateur', 'Permet de modifier un utilisateur', 'EDIT_USER', GETDATE(), 0),
(NEWID(), 'Supprimer un utilisateur', 'Permet de supprimer un utilisateur', 'DELETE_USER', GETDATE(), 0);
```

### Attribution des permissions aux rôles

```sql
-- Administrateur : toutes les permissions
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedDate, IsDeleted)
SELECT NEWID(), 
       (SELECT Id FROM Roles WHERE Code = 'ADMIN'),
       Id,
       GETDATE(),
       0
FROM Permissions;

-- Exemple : Attribuer des permissions à un rôle spécifique
-- Les utilisateurs avec un PosteId assigné seront automatiquement limités à leur poste
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedDate, IsDeleted)
SELECT NEWID(),
       (SELECT Id FROM Roles WHERE Code = 'VOTRE_ROLE'),
       Id,
       GETDATE(),
       0
FROM Permissions 
WHERE Action IN ('VIEW_POSTE', 'VIEW_EQUIPEMENT', 'EDIT_EQUIPEMENT');
```

## 🔍 Tests et validation

### Scénario 1 : Utilisateur avec PosteId assigné
- ✅ L'utilisateur peut voir son poste
- ❌ L'utilisateur ne peut pas voir un autre poste
- ✅ L'utilisateur peut voir les équipements de son poste
- ❌ L'utilisateur ne peut pas voir les équipements d'un autre poste

### Scénario 2 : Utilisateur sans PosteId (ex: Administrateur)
- ✅ Peut voir tous les postes (si permission VIEW_POSTE)
- ✅ Peut voir tous les équipements (si permission VIEW_EQUIPEMENT)
- ✅ Peut gérer les utilisateurs (si permissions appropriées)

### Scénario 3 : Attribution d'un poste
- Un utilisateur peut avoir n'importe quel rôle
- Si un PosteId lui est assigné, il sera limité à ce poste
- Les permissions définissent ce qu'il peut faire (lire, créer, modifier, etc.)
- Le PosteId définit où il peut le faire (son poste uniquement)

## 🚀 Intégration dans le projet

1. ✅ Entités modifiées (User, Poste)
2. ✅ Configuration EF Core dans DLMSDBContext
3. ✅ Services d'autorisation créés
4. ✅ Services enregistrés dans DependencyInjection
5. ✅ Attributs d'autorisation créés
6. ⏳ Migration à appliquer
7. ⏳ Seed des données de base (rôles, permissions)
8. ⏳ Application des attributs sur les contrôleurs existants

## 📞 Support

Pour toute question sur le système d'autorisation, consulter :
- `AuthorizationService.cs` pour la logique métier
- `RequirePermissionAttribute.cs` pour l'usage des permissions
- `RequirePosteAccessAttribute.cs` pour l'accès aux postes
- `RequireEquipementAccessAttribute.cs` pour l'accès aux équipements

