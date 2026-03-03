# 📋 Résumé du Système d'Autorisation DLMS

## 🎯 Principe de base

Le système utilise **deux mécanismes indépendants** :

### 1. **Permissions (via Rôles)** → Définissent CE QUE l'utilisateur peut faire
- Les rôles sont créés et gérés par les administrateurs
- Chaque rôle a plusieurs permissions (VIEW, CREATE, EDIT, DELETE, etc.)
- Un utilisateur a **un seul rôle**

### 2. **PosteId** → Définit OÙ l'utilisateur peut le faire
- Si `PosteId` est **NULL** → L'utilisateur peut accéder à tous les postes (selon ses permissions)
- Si `PosteId` est **assigné** → L'utilisateur ne peut accéder qu'à son poste (et ses cellules/équipements)
- Cette règle s'applique **quel que soit le rôle** de l'utilisateur

## ✅ Règles métier implémentées

1. ✅ **Un utilisateur a un seul rôle** (`User.RoleId`)
2. ✅ **Un rôle a plusieurs permissions** (via `RolePermission`)
3. ✅ **Un utilisateur avec PosteId ne voit que son poste** (logique dans `AuthorizationService`)

## 🔧 Comment ça fonctionne ?

### Exemple 1 : Utilisateur SANS PosteId (Administrateur, Superviseur, etc.)
```
User: Jean Dupont
Role: Administrateur
PosteId: NULL
Permissions: VIEW_POSTE, CREATE_POSTE, EDIT_POSTE, DELETE_POSTE, etc.

Résultat:
✅ Peut voir TOUS les postes
✅ Peut créer/modifier/supprimer des postes (selon permissions)
✅ Peut voir TOUS les équipements
```

### Exemple 2 : Utilisateur AVEC PosteId (Gestionnaire de poste, Technicien de poste, etc.)
```
User: Marie Martin
Role: Technicien (ou n'importe quel rôle)
PosteId: 5
Permissions: VIEW_POSTE, VIEW_EQUIPEMENT, EDIT_EQUIPEMENT

Résultat:
✅ Peut voir UNIQUEMENT le poste 5
❌ Ne peut PAS voir les autres postes
✅ Peut voir UNIQUEMENT les équipements du poste 5
❌ Ne peut PAS voir les équipements des autres postes
✅ Peut modifier les équipements du poste 5 (si permission EDIT_EQUIPEMENT)
```

## 🛠️ Utilisation dans le code

### Dans un contrôleur :

```csharp
// Vérifier une permission
[RequirePermission("VIEW_POSTE")]
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var userId = GetCurrentUserId();
    var hasPosteAssigne = await _authService.UserHasPosteAssignedAsync(userId);
    
    if (hasPosteAssigne)
    {
        // Filtrer uniquement son poste
        var posteId = await _authService.GetUserPosteIdAsync(userId);
        // Retourner uniquement ce poste
    }
    else
    {
        // Retourner tous les postes
    }
}

// Vérifier l'accès à un poste spécifique
[RequirePosteAccess("id")]
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    // L'attribut vérifie automatiquement :
    // - Si PosteId != null → id doit être égal à PosteId
    // - Si PosteId == null → vérifier permission VIEW_POSTE
}

// Vérifier l'accès à un équipement
[RequireEquipementAccess("id")]
[HttpGet("equipements/{id}")]
public async Task<IActionResult> GetEquipement(int id)
{
    // L'attribut vérifie automatiquement :
    // - Si PosteId != null → l'équipement doit appartenir au poste de l'utilisateur
    // - Si PosteId == null → vérifier permission VIEW_EQUIPEMENT
}
```

## 📊 Schéma de décision

```
┌─────────────────────────────────────┐
│   Requête utilisateur               │
└──────────────┬──────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│ 1. Vérifier l'authentification       │
│    (Token JWT valide ?)              │
└──────────────┬───────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│ 2. Vérifier la permission            │
│    (L'utilisateur a la permission ?) │
└──────────────┬───────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│ 3. Vérifier le PosteId               │
│    PosteId == NULL ?                 │
└──────┬───────────────────────┬───────┘
       │ OUI                   │ NON
       ▼                       ▼
┌─────────────────┐    ┌──────────────────────┐
│ Accès à tous    │    │ Vérifier que la      │
│ les postes      │    │ ressource appartient │
│                 │    │ au poste assigné     │
└─────────────────┘    └──────────────────────┘
```

## 🎨 Cas d'usage typiques

### Cas 1 : Créer un administrateur système
```sql
INSERT INTO Users (RoleId, PosteId, ...)
VALUES (@AdminRoleId, NULL, ...);  -- PosteId = NULL → accès global
```

### Cas 2 : Créer un gestionnaire de poste
```sql
INSERT INTO Users (RoleId, PosteId, ...)
VALUES (@GestionnaireRoleId, 5, ...);  -- PosteId = 5 → accès limité au poste 5
```

### Cas 3 : Créer un technicien de poste
```sql
INSERT INTO Users (RoleId, PosteId, ...)
VALUES (@TechnicienRoleId, 3, ...);  -- PosteId = 3 → accès limité au poste 3
```

### Cas 4 : Créer un superviseur multi-postes
```sql
INSERT INTO Users (RoleId, PosteId, ...)
VALUES (@SuperviseurRoleId, NULL, ...);  -- PosteId = NULL → accès à tous les postes
```

## 🔑 Points clés à retenir

1. ✅ **Aucun rôle n'est hardcodé** - Tous les rôles sont créés par les administrateurs
2. ✅ **Le PosteId est la clé** - C'est lui qui détermine la limitation géographique
3. ✅ **Les permissions définissent les actions** - Indépendamment du poste
4. ✅ **Système flexible** - Vous pouvez créer n'importe quel rôle avec n'importe quelles permissions
5. ✅ **Assignation dynamique** - Vous pouvez assigner/retirer un poste à tout moment

## 📁 Fichiers principaux

- **`AuthorizationService.cs`** - Logique de vérification des accès
- **`RequirePermissionAttribute.cs`** - Vérification des permissions
- **`RequirePosteAccessAttribute.cs`** - Vérification d'accès aux postes
- **`RequireEquipementAccessAttribute.cs`** - Vérification d'accès aux équipements
- **`SYSTEME_AUTORISATION.md`** - Documentation complète
- **`GUIDE_DEMARRAGE_RAPIDE.md`** - Guide d'implémentation

---
**Le système est maintenant totalement flexible et ne dépend d'aucun rôle spécifique ! 🎉**

