# 🚀 Guide de Démarrage Rapide - Système d'Autorisation

## ✅ Ce qui a été fait

1. ✅ **Modèle de données**
   - Ajout de `PosteId` à l'entité `User`
   - Configuration des relations dans `DLMSDBContext`
   - Relations : User ↔ Role ↔ Permission ↔ Poste

2. ✅ **Services d'autorisation**
   - `IAuthorizationService` et `AuthorizationService`
   - Enregistrement dans `DependencyInjection.cs`
   - Logique métier pour utilisateurs avec poste assigné

3. ✅ **Attributs d'autorisation**
   - `RequirePermissionAttribute` : vérification de permissions
   - `RequirePosteAccessAttribute` : accès aux postes
   - `RequireEquipementAccessAttribute` : accès aux équipements

4. ✅ **Documentation et exemples**
   - Documentation complète : `SYSTEME_AUTORISATION.md`
   - Script SQL de seed : `SCRIPT_SEED_PERMISSIONS.sql`
   - Exemple de contrôleur : `EXEMPLE_CONTROLLER_AVEC_AUTORISATION.cs`

## 📋 Prochaines étapes

### Étape 1 : Créer et appliquer la migration

```bash
# Dans le terminal PowerShell
cd DLMS_DAL
dotnet ef migrations add AddUserPosteRelation --startup-project ..\DLMS.API
dotnet ef database update --startup-project ..\DLMS.API
```

### Étape 2 : Exécuter le script de seed

Exécutez le fichier `SCRIPT_SEED_PERMISSIONS.sql` dans SQL Server Management Studio ou via la ligne de commande :

```bash
sqlcmd -S localhost -d DLMS_DB -i SCRIPT_SEED_PERMISSIONS.sql
```

Cela créera :
- 4 rôles exemples (Admin, Gestionnaire, Technicien, Superviseur)
- Toutes les permissions nécessaires
- Les attributions de permissions aux rôles

### Étape 3 : Créer un utilisateur avec un poste assigné

```sql
-- Créer un poste de test
INSERT INTO Poste (Numero, Libelle, Adresse, CreatedDate)
VALUES ('P001', 'Poste Test', '123 Rue Test', GETDATE());

-- Récupérer l'ID du poste créé
DECLARE @PosteId INT = (SELECT TOP 1 Id FROM Poste WHERE Numero = 'P001');

-- Récupérer l'ID d'un rôle (peu importe le rôle)
DECLARE @RoleId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Roles);

-- Créer un utilisateur avec un poste assigné
-- Cet utilisateur sera automatiquement limité à son poste, quel que soit son rôle
INSERT INTO Users (Id, Nom, Prenoms, Email, Mobile, MotDePasse, RoleId, PosteId, CreatedDate, IsDeleted, MustChangePassword)
VALUES (
    NEWID(),
    'Dupont',
    'Jean',
    'jean.dupont@dlms.com',
    '0123456789',
    '$2a$11$...',  -- Mot de passe hashé avec BCrypt
    @RoleId,
    @PosteId,        -- L'assignation du poste limite l'accès
    GETDATE(),
    0,
    1
);
```

### Étape 4 : Utiliser les attributs dans vos contrôleurs

#### Exemple simple :

```csharp
using DLMS.API.Helpers;

[ApiController]
[Route("api/[controller]")]
public class PosteController : ControllerBase
{
    // Seuls les utilisateurs avec VIEW_POSTE peuvent accéder
    [RequirePermission("VIEW_POSTE")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // ...
    }

    // Vérification d'accès au poste spécifique
    [RequirePosteAccess("id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Un utilisateur avec PosteId ne peut voir que son poste
        // Les utilisateurs sans PosteId peuvent voir tous les postes
    }
}
```

### Étape 5 : Vérifier que JWT inclut UserId

Dans votre système d'authentification, assurez-vous que le token JWT contient l'ID de l'utilisateur :

```csharp
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim("UserId", user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role.Code)
};
```

## 🧪 Tests recommandés

### Test 1 : Utilisateur avec PosteId accède à son poste
```http
GET /api/postes/{sonPosteId}
Authorization: Bearer {token_utilisateur_avec_poste}
```
**Résultat attendu** : ✅ 200 OK

### Test 2 : Utilisateur avec PosteId accède à un autre poste
```http
GET /api/postes/{autrePosteId}
Authorization: Bearer {token_utilisateur_avec_poste}
```
**Résultat attendu** : ❌ 403 Forbidden

### Test 3 : Utilisateur sans PosteId accède à n'importe quel poste
```http
GET /api/postes/{anyPosteId}
Authorization: Bearer {token_utilisateur_sans_poste}
```
**Résultat attendu** : ✅ 200 OK (si permission VIEW_POSTE)

### Test 4 : Utilisateur avec PosteId liste les postes
```http
GET /api/postes
Authorization: Bearer {token_utilisateur_avec_poste}
```
**Résultat attendu** : ✅ 200 OK (uniquement son poste dans la liste)

## 📊 Schéma des relations

```
User
├── RoleId (FK) → Role
└── PosteId (FK) → Poste (nullable, pour limiter l'accès à un poste spécifique)

Role
└── RolePermissions (collection) → RolePermission

RolePermission (table de liaison)
├── RoleId (FK) → Role
└── PermissionId (FK) → Permission

Permission
└── Action (string) : "VIEW_POSTE", "CREATE_USER", etc.

Poste
├── Cellules (collection) → Cellule
└── Users (collection) → User (utilisateurs assignés à ce poste)

Cellule
└── Equipements (collection) → Equipement
```

## 🔍 Debugging

### Vérifier les permissions d'un utilisateur

```sql
SELECT 
    u.Email,
    r.Libelle AS Role,
    p.Libelle AS Permission,
    p.Action
FROM Users u
INNER JOIN Roles r ON u.RoleId = r.Id
LEFT JOIN RolePermissions rp ON r.Id = rp.RoleId
LEFT JOIN Permissions p ON rp.PermissionId = p.Id
WHERE u.Email = 'jean.dupont@dlms.com'
AND u.IsDeleted = 0
AND rp.IsDeleted = 0;
```

### Vérifier l'attribution d'un poste

```sql
SELECT 
    u.Email,
    u.Nom,
    u.Prenoms,
    r.Libelle AS Role,
    p.Numero AS NumeroPoste,
    p.Libelle AS PosteLibelle
FROM Users u
LEFT JOIN Roles r ON u.RoleId = r.Id
LEFT JOIN Poste p ON u.PosteId = p.Id
WHERE u.PosteId IS NOT NULL;  -- Tous les utilisateurs avec un poste assigné
```

## 📝 Notes importantes

1. **Flexibilité des rôles** : Les rôles et permissions sont totalement gérés par les administrateurs. Le système vérifie uniquement la présence d'un `PosteId` pour limiter l'accès à un poste spécifique.

2. **Séparation des responsabilités** :
   - **Permissions** (via rôles) → définissent CE QUE l'utilisateur peut faire (VIEW, CREATE, EDIT, DELETE)
   - **PosteId** → définit OÙ l'utilisateur peut le faire (son poste uniquement ou partout si null)

3. **Gestion des claims** : Les attributs recherchent les claims `ClaimTypes.NameIdentifier` ou `UserId`. Assurez-vous que votre JWT en contient au moins un.

4. **Performance** : Pour de meilleures performances, envisagez de mettre en cache les permissions des utilisateurs.

5. **Sécurité** : Toujours utiliser `[Authorize]` au niveau du contrôleur + les attributs de permission au niveau des actions.

## 🎯 Résumé des fichiers créés/modifiés

### Modifiés
- ✅ `DLMS_MODELS/UsersDomain/Entities/User.cs`
- ✅ `DLMS_MODELS/PosteDomain/Entities/Poste.cs`
- ✅ `DLMS_DAL/Datas/DLMSDBContext.cs`
- ✅ `DLMS_DAL/DependencyInjection.cs`

### Créés
- ✅ `DLMS_DAL/Services/IAuthorizationService.cs`
- ✅ `DLMS_DAL/Services/AuthorizationService.cs`
- ✅ `DLMS.API/Helpers/RequirePermissionAttribute.cs`
- ✅ `DLMS.API/Helpers/RequirePosteAccessAttribute.cs`
- ✅ `DLMS.API/Helpers/RequireEquipementAccessAttribute.cs`
- ✅ `SYSTEME_AUTORISATION.md` (documentation complète)
- ✅ `SCRIPT_SEED_PERMISSIONS.sql` (données de base)
- ✅ `EXEMPLE_CONTROLLER_AVEC_AUTORISATION.cs` (exemples)
- ✅ `GUIDE_DEMARRAGE_RAPIDE.md` (ce fichier)

## 🆘 Besoin d'aide ?

Consultez les fichiers suivants :
- **Documentation complète** : `SYSTEME_AUTORISATION.md`
- **Exemples pratiques** : `EXEMPLE_CONTROLLER_AVEC_AUTORISATION.cs`
- **Script SQL** : `SCRIPT_SEED_PERMISSIONS.sql`

---
**Bon développement ! 🚀**

