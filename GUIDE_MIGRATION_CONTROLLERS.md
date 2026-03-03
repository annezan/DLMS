# 🔄 Guide de Migration des Contrôleurs

## ⚙️ SYSTÈME UTILISÉ : Permissions par CONCEPT

Ce guide utilise le système de permissions **par CONCEPT** :
- Format : `VIEW_XXX`, `CREATE_XXX`, `EDIT_XXX`, `DELETE_XXX`
- Exemple : `[RequirePermission("VIEW_FABRICANT")]`

✅ **Avantages :**
- Plus simple et lisible
- Indépendant des URLs
- Réutilisable (une permission pour plusieurs endpoints)
- Cohérent avec l'existant (Poste, Cellule, Equipement)

---

## 📊 ÉTAT DES LIEUX

### ✅ Contrôleurs déjà migrés
- ✅ PosteController (VIEW_POSTE, CREATE_POSTE, etc.)
- ✅ CelluleController (VIEW_CELLULE, CREATE_CELLULE, etc.)
- ✅ EquipementController (VIEW_EQUIPEMENT, CREATE_EQUIPEMENT, etc.)
- ✅ UsersController (avec ASSIGN_USER_POSTE)
- ✅ FabricantController (VIEW_FABRICANT, CREATE_FABRICANT, etc.)
- ✅ TypecommandeController (VIEW_TYPECOMMANDE)
- ✅ CompteurController (VIEW_COMPTEUR, CREATE_COMPTEUR, SYNC_COMPTEUR, etc.)
- ✅ AuthController (pas besoin de migration - auth)

### ⏳ Contrôleurs à migrer
- ⏳ RolesController (remplacer PermissionLabel par RequirePermission)
- ⏳ PermissionsController (remplacer PermissionLabel par RequirePermission)
- ⏳ CommandeController
- ⏳ AlarmsController
- ⏳ ErrorController
- ⏳ EventsController
- ⏳ CompteurEquipementController
- ⏳ CommandeCompteurController
- ⏳ CodeObisController
- ⏳ AssociationKeyController
- ⏳ GxdlmsprofilgenericController

---

## 🎯 TEMPLATE DE MIGRATION

### **Type 1 : Contrôleur SANS gestion de poste**

Pour les entités qui ne sont PAS liées aux postes (Fabricant, Rôles, Permissions, etc.)

```csharp
using DLMS.API.Helpers;
using DLMS_MODELS.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // ✅ Ajouter
    public class XxxController : ApiController  // Peut rester ApiController
    {
        /// <summary>
        /// Récupère la liste des xxx
        /// </summary>
        [RequirePermission("VIEW_XXX")]  // ✅ Format par concept
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<XxxResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetXxxQuery());
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Récupère un xxx par son ID
        /// </summary>
        [RequirePermission("VIEW_XXX")]  // ✅ Même permission pour getById
        [HttpGet("getXxxById")]
        public async Task<ActionResult<ResponseBase<XxxResponse>>> GetById([FromQuery] GetXxxByIdQuery query)
        {
            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouveau xxx
        /// </summary>
        [RequirePermission("CREATE_XXX")]  // ✅ Format par concept
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<XxxResponse>>> Add([FromBody] XxxAddCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un xxx existant
        /// </summary>
        [RequirePermission("EDIT_XXX")]  // ✅ Format par concept
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<XxxResponse>>> Edit([FromBody] XxxEditCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un xxx
        /// </summary>
        [RequirePermission("DELETE_XXX")]  // ✅ Format par concept
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] XxxDeleteCommand command)
        {
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
```

---

### **Type 2 : Contrôleur AVEC gestion de poste**

Pour les entités liées aux postes (Compteur avec Equipement, Commande, etc.)

```csharp
using DLMS.API.Helpers;
using DLMS_DAL.Services;
using DLMS_MODELS.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class XxxController : AuthorizedApiController  // ✅ Hériter
    {
        /// <summary>
        /// Récupère la liste des xxx
        /// Filtre automatiquement par poste si l'utilisateur a un poste assigné
        /// </summary>
        [RequirePermission("VIEW_XXX")]  // ✅ Format par concept
        [HttpGet]
        public async Task<ActionResult<ResponseBase<List<XxxResponse>>>> Get()
        {
            var result = await Mediator.Send(new GetXxxQuery());
            
            if (result == null || !result.IsSuccess)
            {
                return BadRequest(result);
            }

            // ✅ Filtrer par poste si nécessaire
            var userId = GetCurrentUserId();
            var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);

            if (hasPosteAssigne)
            {
                var posteId = await AuthService.GetUserPosteIdAsync(userId);
                
                // TODO: Filtrer les données selon votre logique métier
                // Exemple : result.Data = result.Data.Where(x => x.PosteId == posteId).ToList();
            }

            return Ok(result);
        }

        /// <summary>
        /// Récupère un xxx par son ID
        /// </summary>
        [RequirePermission("VIEW_XXX")]  // ✅ Même permission
        [HttpGet("getXxxById")]
        public async Task<ActionResult<ResponseBase<XxxResponse>>> GetById([FromQuery] GetXxxByIdQuery query)
        {
            // TODO: Vérification d'accès si nécessaire
            var userId = GetCurrentUserId();
            var hasPosteAssigne = await AuthService.UserHasPosteAssignedAsync(userId);

            if (hasPosteAssigne)
            {
                // Vérifier que l'entité appartient au poste de l'utilisateur
            }

            var result = await Mediator.Send(query);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Crée un nouveau xxx
        /// </summary>
        [RequirePermission("CREATE_XXX")]  // ✅ Format par concept
        [HttpPost("add")]
        public async Task<ActionResult<ResponseBase<XxxResponse>>> Add([FromBody] XxxAddCommand command)
        {
            // TODO: Vérifier l'accès avant création
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Modifie un xxx existant
        /// </summary>
        [RequirePermission("EDIT_XXX")]  // ✅ Format par concept
        [HttpPost("edit")]
        public async Task<ActionResult<ResponseBase<XxxResponse>>> Edit([FromBody] XxxEditCommand command)
        {
            // TODO: Vérifier l'accès avant modification
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }

        /// <summary>
        /// Supprime un xxx
        /// </summary>
        [RequirePermission("DELETE_XXX")]  // ✅ Format par concept
        [HttpPost("delete")]
        public async Task<ActionResult<ResponseBase<string>>> Delete([FromBody] XxxDeleteCommand command)
        {
            // TODO: Vérifier l'accès avant suppression
            var result = await Mediator.Send(command);
            return result != null ? Ok(result) : (ActionResult)BadRequest(result);
        }
    }
}
```

---

## 📝 CHECKLIST DE MIGRATION

Pour chaque contrôleur :

### 1. **Déterminer le type**
- [ ] Type 1 (SANS poste) : Fabricant, Rôles, Permissions, TypeCommande, etc.
- [ ] Type 2 (AVEC poste) : Compteur, Commande, Alarms, Events, etc.

### 2. **Modifier la classe**
- [ ] Ajouter/décommenter `[Authorize]` au niveau de la classe
- [ ] Hériter de `AuthorizedApiController` si Type 2
- [ ] Conserver `ApiController` si Type 1

### 3. **Ajouter les attributs par concept**
- [ ] `[RequirePermission("VIEW_XXX")]` pour Get et GetById
- [ ] `[RequirePermission("CREATE_XXX")]` pour Add/Create
- [ ] `[RequirePermission("EDIT_XXX")]` pour Edit/Update
- [ ] `[RequirePermission("DELETE_XXX")]` pour Delete
- [ ] Actions spéciales : `SYNC_XXX`, `ASSIGN_XXX`, `EXPORT_XXX`, etc.

### 4. **Ajouter le filtrage (Type 2 uniquement)**
- [ ] Filtrer les listes par poste
- [ ] Vérifier l'accès dans les méthodes de modification/suppression

### 5. **Créer les permissions dans la base**
- [ ] Exécuter le script SQL correspondant

### 6. **Tester**
- [ ] Tester avec utilisateur SANS PosteId (doit tout voir)
- [ ] Tester avec utilisateur AVEC PosteId (doit voir uniquement son poste)
- [ ] Tester les erreurs 403 Forbidden

---

## 📋 CONVENTION DES PERMISSIONS

### Format
```
<ACTION>_<ENTITÉ>
```

### Actions standard
- **VIEW** : Voir (Get, GetById, GetByCode, etc.)
- **CREATE** : Créer (Add)
- **EDIT** : Modifier (Edit, Update)
- **DELETE** : Supprimer (Delete)
- **MANAGE** : Gestion complète (tous les droits)

### Actions spéciales
- **SYNC** : Synchroniser (ex: MAJCompteur)
- **EXPORT** : Exporter
- **IMPORT** : Importer
- **ASSIGN** : Assigner (ex: assign-poste)
- **APPROVE** : Approuver
- **VALIDATE** : Valider

### Exemples complets
```csharp
// Fabricant
VIEW_FABRICANT
CREATE_FABRICANT
EDIT_FABRICANT
DELETE_FABRICANT

// Compteur
VIEW_COMPTEUR
CREATE_COMPTEUR
EDIT_COMPTEUR
DELETE_COMPTEUR
SYNC_COMPTEUR        // Pour MAJCompteur

// Commande
VIEW_COMMANDE
CREATE_COMMANDE
EDIT_COMMANDE
DELETE_COMMANDE
APPROVE_COMMANDE     // Pour approuver une commande
EXECUTE_COMMANDE     // Pour exécuter une commande

// Rôle
VIEW_ROLE
CREATE_ROLE
EDIT_ROLE
DELETE_ROLE
MANAGE_ROLE          // Gestion complète des rôles

// Permission
VIEW_PERMISSION
MANAGE_PERMISSION    // Gestion complète des permissions
```

---

## 🎨 EXEMPLES PAR CATÉGORIE

### **Catégorie A : Indépendants du poste (Type 1)**

- **RolesController** → `VIEW_ROLE`, `CREATE_ROLE`, `EDIT_ROLE`, `DELETE_ROLE`
- **PermissionsController** → `VIEW_PERMISSION`, `MANAGE_PERMISSION`
- **FabricantController** → `VIEW_FABRICANT`, `CREATE_FABRICANT`, `EDIT_FABRICANT`, `DELETE_FABRICANT`
- **TypeCommandeController** → `VIEW_TYPECOMMANDE`
- **CodeObisController** → `VIEW_CODEOBIS`, `CREATE_CODEOBIS`, `EDIT_CODEOBIS`, `DELETE_CODEOBIS`

### **Catégorie B : Liés au poste (Type 2)**

- **CompteurController** → `VIEW_COMPTEUR`, `CREATE_COMPTEUR`, `EDIT_COMPTEUR`, `DELETE_COMPTEUR`, `SYNC_COMPTEUR`
- **CommandeController** → `VIEW_COMMANDE`, `CREATE_COMMANDE`, `EDIT_COMMANDE`, `DELETE_COMMANDE`
- **AlarmsController** → `VIEW_ALARM`, `CREATE_ALARM`, `EDIT_ALARM`, `DELETE_ALARM`
- **ErrorController** → `VIEW_ERROR`
- **EventsController** → `VIEW_EVENT`

---

## 📋 FORMAT DES PERMISSIONS SQL

Pour chaque entité, créer les permissions correspondantes :

```sql
-- Format standard pour chaque entité
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(NEWID(), 'Voir les xxx', 'Permet de visualiser la liste des xxx et leurs détails', 'VIEW_XXX', GETDATE(), 0),
(NEWID(), 'Créer un xxx', 'Permet de créer un nouveau xxx', 'CREATE_XXX', GETDATE(), 0),
(NEWID(), 'Modifier un xxx', 'Permet de modifier un xxx existant', 'EDIT_XXX', GETDATE(), 0),
(NEWID(), 'Supprimer un xxx', 'Permet de supprimer un xxx', 'DELETE_XXX', GETDATE(), 0);

-- Exemple concret : Fabricant
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(NEWID(), 'Voir les fabricants', 'Permet de visualiser la liste des fabricants', 'VIEW_FABRICANT', GETDATE(), 0),
(NEWID(), 'Créer un fabricant', 'Permet de créer un nouveau fabricant', 'CREATE_FABRICANT', GETDATE(), 0),
(NEWID(), 'Modifier un fabricant', 'Permet de modifier un fabricant', 'EDIT_FABRICANT', GETDATE(), 0),
(NEWID(), 'Supprimer un fabricant', 'Permet de supprimer un fabricant', 'DELETE_FABRICANT', GETDATE(), 0);
```

---

## 🚀 ORDRE DE MIGRATION RECOMMANDÉ

### **Phase 1 : Les plus simples (Catégorie A)**

1. ✅ FabricantController (FAIT)
2. ✅ TypeCommandeController (FAIT)
3. ⏳ RolesController (remplacer PermissionLabel)
4. ⏳ PermissionsController (remplacer PermissionLabel)
5. ⏳ CodeObisController

### **Phase 2 : Ceux avec logique métier (Catégorie B)**

6. ✅ CompteurController (FAIT)
7. ⏳ CommandeController
8. ⏳ AlarmsController
9. ⏳ ErrorController
10. ⏳ EventsController
11. ⏳ GxdlmsprofilgenericController

### **Phase 3 : Tables de liaison (Catégorie C)**

12. ⏳ CommandeCompteurController
13. ⏳ CompteurEquipementController
14. ⏳ AssociationKeyController

---

## ⚠️ POINTS D'ATTENTION

### 1. **PermissionLabel vs RequirePermission**

**Ancien système** : `[PermissionLabel("...")]` (juste un label, pas de vérification)
**Nouveau système** : `[RequirePermission("VIEW_XXX")]` (vérification active)

⚠️ Il faut remplacer et créer les permissions dans la base !

### 2. **Cohérence des noms**

Utilisez toujours le nom de l'entité au SINGULIER en MAJUSCULES :
- ✅ `VIEW_FABRICANT` (singulier)
- ❌ `VIEW_FABRICANTS` (pluriel)

### 3. **Réutilisation des permissions**

Une même permission peut protéger plusieurs endpoints :
```csharp
[RequirePermission("VIEW_FABRICANT")]
[HttpGet]
public async Task<ActionResult> Get() { }

[RequirePermission("VIEW_FABRICANT")]  // Même permission
[HttpGet("getFabricantById")]
public async Task<ActionResult> GetById() { }
```

### 4. **Permissions spéciales**

Pour les actions qui ne sont pas CRUD standard, utilisez des verbes spécifiques :
- `SYNC_COMPTEUR` pour la synchronisation
- `ASSIGN_USER_POSTE` pour l'assignation
- `APPROVE_COMMANDE` pour l'approbation
- `EXPORT_DATA` pour l'export
- `IMPORT_DATA` pour l'import

---

## 🎯 PROCHAINES ÉTAPES

1. ✅ Lire ce guide
2. ✅ Comprendre le système par concept
3. ⏳ Migrer RolesController et PermissionsController
4. ⏳ Utiliser le template pour les autres contrôleurs
5. ⏳ Créer les permissions dans la base de données
6. ⏳ Tester chaque contrôleur migré

---

**🎉 Bonne migration avec le système par CONCEPT ! 🚀**
