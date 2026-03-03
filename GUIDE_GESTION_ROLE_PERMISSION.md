# 🔐 Guide Complet : Gestion des Associations Rôle-Permission

## 📋 Vue d'ensemble

Ce document décrit le système complet de gestion des associations entre rôles et permissions qui a été implémenté dans l'application DLMS.

## ✅ Fonctionnalités implémentées

### 1. **Modèles de données**

#### Commands
- ✅ `RolePermissionAddCommand` : Ajouter des permissions à un rôle
- ✅ `RolePermissionDeleteCommand` : Retirer des permissions d'un rôle

#### Queries
- ✅ `GetRolePermissionsQuery` : Récupérer toutes les permissions d'un rôle

#### Responses
- ✅ `RoleResponse` enrichi avec la liste des permissions associées

### 2. **Couche d'accès aux données (DAL)**

#### Repositories Command
- ✅ `IRolePermissionCommandRepository` / `RolePermissionCommandRepository`
  - `AddAsync()` : Ajouter une association
  - `AddRangeAsync()` : Ajouter plusieurs associations
  - `DeleteRangeByRoleAndPermissionsAsync()` : Supprimer des associations spécifiques

#### Repositories Query
- ✅ `IRolePermissionQueryRepository` / `RolePermissionQueryRepository`
  - `GetPermissionsByRoleIdAsync(Guid roleId)` : Récupérer les permissions d'un rôle
  - `ExistsAsync(Guid roleId, Guid permissionId)` : Vérifier si une association existe

#### Extension du RolesQueryRepository
- ✅ `GetAllWithPermissionsAsync()` : Récupérer tous les rôles avec leurs permissions
- ✅ `GetByIdWithPermissionsAsync(Guid id)` : Récupérer un rôle avec ses permissions

### 3. **Couche métier (Business)**

#### Handlers de commandes
- ✅ `RolePermissionAddCommandHandler`
  - Vérifie l'existence du rôle
  - Vérifie l'existence des permissions
  - Évite les doublons
  - Ajoute les nouvelles associations
  
- ✅ `RolePermissionDeleteCommandHandler`
  - Vérifie l'existence du rôle
  - Supprime les associations spécifiées

#### Handlers de requêtes
- ✅ `GetRolePermissionsQueryHandler`
  - Récupère les permissions d'un rôle avec mapping vers DTO

#### Mappers
- ✅ `RolesMappingProfile` mis à jour pour inclure automatiquement les permissions dans `RoleResponse`

### 4. **API Controller**

- ✅ `RolePermissionsController`
  - `GET /api/RolePermissions/{roleId}` : Récupérer les permissions d'un rôle
  - `POST /api/RolePermissions/add` : Ajouter des permissions à un rôle
  - `POST /api/RolePermissions/delete` : Retirer des permissions d'un rôle

### 5. **Injection de dépendances**

- ✅ Enregistrement des nouveaux repositories dans `DependencyInjection.cs`

## 🚀 Utilisation

### 1. Récupérer toutes les permissions d'un rôle

**Endpoint :** `GET /api/RolePermissions/{roleId}`  
**Permission requise :** `VIEW_ROLE`

**Exemple de réponse :**
```json
{
  "isSuccess": true,
  "message": "Permissions du rôle 'Administrateur' récupérées avec succès.",
  "data": [
    {
      "id": "guid-1",
      "libelle": "Voir les utilisateurs",
      "description": "Permet de visualiser la liste des utilisateurs",
      "action": "VIEW_USER"
    },
    {
      "id": "guid-2",
      "libelle": "Créer un utilisateur",
      "description": "Permet de créer un nouvel utilisateur",
      "action": "CREATE_USER"
    }
  ]
}
```

### 2. Ajouter des permissions à un rôle

**Endpoint :** `POST /api/RolePermissions/add`  
**Permission requise :** `EDIT_ROLE`

**Body :**
```json
{
  "roleId": "guid-du-role",
  "permissionIds": [
    "guid-permission-1",
    "guid-permission-2",
    "guid-permission-3"
  ]
}
```

**Réponse :**
```json
{
  "isSuccess": true,
  "message": "3 permission(s) ajoutée(s) au rôle 'Gestionnaire' avec succès.",
  "data": "3 permission(s) ajoutée(s) au rôle 'Gestionnaire' avec succès."
}
```

**Comportement :**
- ✅ Vérifie que le rôle existe
- ✅ Vérifie que toutes les permissions existent
- ✅ Évite les doublons automatiquement
- ✅ Ajoute uniquement les nouvelles associations

### 3. Retirer des permissions d'un rôle

**Endpoint :** `POST /api/RolePermissions/delete`  
**Permission requise :** `EDIT_ROLE`

**Body :**
```json
{
  "roleId": "guid-du-role",
  "permissionIds": [
    "guid-permission-1",
    "guid-permission-2"
  ]
}
```

**Réponse :**
```json
{
  "isSuccess": true,
  "message": "Permission(s) retirée(s) du rôle 'Technicien' avec succès.",
  "data": "Permission(s) retirée(s) du rôle 'Technicien' avec succès."
}
```

### 4. Récupérer un rôle avec ses permissions

**Endpoint :** `GET /api/Roles/getRoleById?id={roleId}`  
**Permission requise :** `VIEW_ROLE`

**Réponse :**
```json
{
  "isSuccess": true,
  "data": {
    "id": "guid-du-role",
    "libelle": "Gestionnaire",
    "code": "GESTIONNAIRE",
    "description": "Gestionnaire de poste",
    "permissions": [
      {
        "id": "guid-permission-1",
        "libelle": "Voir les postes",
        "description": "...",
        "action": "VIEW_POSTE"
      },
      {
        "id": "guid-permission-2",
        "libelle": "Voir les cellules",
        "description": "...",
        "action": "VIEW_CELLULE"
      }
    ]
  }
}
```

**Note :** Les endpoints existants pour récupérer les rôles incluent maintenant automatiquement les permissions !

## 🔒 Sécurité

- ✅ Tous les endpoints nécessitent une authentification (`[Authorize]`)
- ✅ Les endpoints de lecture nécessitent la permission `VIEW_ROLE`
- ✅ Les endpoints de modification nécessitent la permission `EDIT_ROLE`
- ✅ Validation de l'existence des rôles et permissions avant toute opération

## 📊 Architecture

```
┌─────────────────────────────────────────────────┐
│              API Layer                          │
│  RolePermissionsController                      │
└─────────────────┬───────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────────┐
│           Business Layer                        │
│  - RolePermissionAddCommandHandler              │
│  - RolePermissionDeleteCommandHandler           │
│  - GetRolePermissionsQueryHandler               │
└─────────────────┬───────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────────┐
│            Data Layer (DAL)                     │
│  - RolePermissionCommandRepository              │
│  - RolePermissionQueryRepository                │
│  - RolesQueryRepository (extended)              │
└─────────────────┬───────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────────────────┐
│           Database (PostgreSQL)                 │
│  - Roles                                        │
│  - Permissions                                  │
│  - RolePermissions (table de liaison)           │
└─────────────────────────────────────────────────┘
```

## 🎯 Cas d'utilisation typiques

### Scénario 1 : Créer un nouveau rôle avec des permissions

1. Créer le rôle via `POST /api/Roles/add`
2. Ajouter les permissions via `POST /api/RolePermissions/add`

### Scénario 2 : Modifier les permissions d'un rôle existant

1. Récupérer les permissions actuelles via `GET /api/RolePermissions/{roleId}`
2. Ajouter de nouvelles permissions via `POST /api/RolePermissions/add`
3. Ou retirer des permissions via `POST /api/RolePermissions/delete`

### Scénario 3 : Visualiser tous les rôles avec leurs permissions

1. Appeler `GET /api/Roles`
2. La réponse inclut automatiquement toutes les permissions de chaque rôle

## 📝 Bonnes pratiques

### ✅ À faire
- Toujours vérifier que le rôle existe avant de modifier ses permissions
- Utiliser les endpoints de liste pour voir l'état actuel avant modification
- Documenter les permissions assignées à chaque rôle dans votre système

### ❌ À éviter
- Ne pas retirer toutes les permissions d'un rôle (risque de bloquer les utilisateurs)
- Ne pas modifier les permissions du rôle Admin sans comprendre l'impact
- Ne pas créer d'associations manuellement dans la base de données

## 🔄 Améliorations futures possibles

- [ ] Endpoint pour assigner toutes les permissions à un rôle en une seule fois
- [ ] Endpoint pour copier les permissions d'un rôle vers un autre
- [ ] Historique des modifications de permissions par rôle
- [ ] Validation métier plus poussée (ex: certaines permissions obligatoires)
- [ ] API pour obtenir les différences entre deux rôles

## 🎉 Conclusion

Le système de gestion des associations rôle-permission est maintenant complet et fonctionnel. Il permet une gestion fine et flexible des droits d'accès dans l'application DLMS tout en respectant l'architecture Clean Architecture du projet.

---

**Créé le :** 6 janvier 2026  
**Version :** 1.0  
**Système :** DLMS API

