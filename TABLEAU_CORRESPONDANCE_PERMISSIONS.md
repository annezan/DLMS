# 📋 Tableau de Correspondance des Permissions

## 🎯 Système Utilisé : Permissions par CONCEPT

Format : `<ACTION>_<ENTITÉ>`

---

## 📊 CONTRÔLEURS MIGRÉS ✅

### 1. PosteController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Poste` | GET | `VIEW_POSTE` | ✅ |
| `/api/Poste/getPosteById` | GET | `VIEW_POSTE` | ✅ |
| `/api/Poste/add` | POST | `CREATE_POSTE` | ✅ |
| `/api/Poste/edit` | POST | `EDIT_POSTE` | ✅ |
| `/api/Poste/delete` | POST | `DELETE_POSTE` | ✅ |

**Filtrage par poste** : Oui

---

### 2. CelluleController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Cellule` | GET | `VIEW_CELLULE` | ✅ |
| `/api/Cellule/getCelluleById` | GET | `VIEW_CELLULE` | ✅ |
| `/api/Cellule/add` | POST | `CREATE_CELLULE` | ✅ |
| `/api/Cellule/edit` | POST | `EDIT_CELLULE` | ✅ |
| `/api/Cellule/delete` | POST | `DELETE_CELLULE` | ✅ |

**Filtrage par poste** : Oui (via relation Cellule → Poste)

---

### 3. EquipementController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Equipement` | GET | `VIEW_EQUIPEMENT` | ✅ |
| `/api/Equipement/getEquipementById` | GET | `VIEW_EQUIPEMENT` | ✅ |
| `/api/Equipement/add` | POST | `CREATE_EQUIPEMENT` | ✅ |
| `/api/Equipement/edit` | POST | `EDIT_EQUIPEMENT` | ✅ |
| `/api/Equipement/delete` | POST | `DELETE_EQUIPEMENT` | ✅ |

**Filtrage par poste** : Oui (via relation Equipement → Cellule → Poste)

---

### 4. UsersController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Users` | GET | `VIEW_USER` | ✅ |
| `/api/Users/getUserById` | GET | `VIEW_USER` | ✅ |
| `/api/Users/add` | POST | `CREATE_USER` | ✅ |
| `/api/Users/edit` | POST | `EDIT_USER` | ✅ |
| `/api/Users/delete` | POST | `DELETE_USER` | ✅ |
| `/api/Users/assign-poste` | POST | `ASSIGN_USER_POSTE` | ✅ |

**Filtrage par poste** : Non (gestion globale des utilisateurs)

---

### 5. FabricantController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Fabricant` | GET | `VIEW_FABRICANT` | ✅ |
| `/api/Fabricant/getFabricantById` | GET | `VIEW_FABRICANT` | ✅ |
| `/api/Fabricant/add` | POST | `CREATE_FABRICANT` | ✅ |
| `/api/Fabricant/edit` | POST | `EDIT_FABRICANT` | ✅ |
| `/api/Fabricant/delete` | POST | `DELETE_FABRICANT` | ✅ |

**Filtrage par poste** : Non (données globales)

---

### 6. TypecommandeController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Typecommande` | GET | `VIEW_TYPECOMMANDE` | ✅ |

**Filtrage par poste** : Non (données globales)

**Note** : Seulement la méthode GET est implémentée. Les autres sont commentées.

---

### 7. CompteurController ✅

| Endpoint | Méthode HTTP | Permission | Status |
|----------|--------------|------------|--------|
| `/api/Compteur` | GET | `VIEW_COMPTEUR` | ✅ |
| `/api/Compteur/getCompteurById` | GET | `VIEW_COMPTEUR` | ✅ |
| `/api/Compteur/add` | POST | `CREATE_COMPTEUR` | ✅ |
| `/api/Compteur/edit` | PUT | `EDIT_COMPTEUR` | ✅ |
| `/api/Compteur/delete` | DELETE | `DELETE_COMPTEUR` | ✅ |
| `/api/Compteur/MAJCompteur` | POST | `SYNC_COMPTEUR` | ✅ |

**Filtrage par poste** : Oui (TODO à implémenter - via relation Compteur → Equipement → Cellule → Poste)

**Note** : La logique de filtrage par poste nécessite une implémentation spécifique car la relation est indirecte.

---

## ⏳ CONTRÔLEURS À MIGRER

### 8. RolesController ⏳

| Endpoint | Méthode HTTP | Permission Actuelle | Permission Cible | Status |
|----------|--------------|---------------------|------------------|--------|
| `/api/Roles` | GET | `[PermissionLabel("...")]` | `VIEW_ROLE` | ⏳ |
| `/api/Roles/getRoleById` | GET | `[PermissionLabel("...")]` | `VIEW_ROLE` | ⏳ |
| `/api/Roles/getRoleByCode` | GET | `[PermissionLabel("...")]` | `VIEW_ROLE` | ⏳ |
| `/api/Roles/add` | POST | `[PermissionLabel("...")]` | `CREATE_ROLE` | ⏳ |
| `/api/Roles/edit` | POST | `[PermissionLabel("...")]` | `EDIT_ROLE` | ⏳ |
| `/api/Roles/delete` | POST | `[PermissionLabel("...")]` | `DELETE_ROLE` | ⏳ |

**Filtrage par poste** : Non (gestion globale)

**Action** : Remplacer `[PermissionLabel]` par `[RequirePermission]`

---

### 9. PermissionsController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/Permissions` | GET | `VIEW_PERMISSION` | ⏳ |
| `/api/Permissions/getPermissionById` | GET | `VIEW_PERMISSION` | ⏳ |
| `/api/Permissions/add` | POST | `CREATE_PERMISSION` | ⏳ |
| `/api/Permissions/edit` | POST | `EDIT_PERMISSION` | ⏳ |
| `/api/Permissions/delete` | POST | `DELETE_PERMISSION` | ⏳ |
| `/api/Permissions/assign` | POST | `MANAGE_PERMISSION` | ⏳ |

**Filtrage par poste** : Non (gestion globale)

**Action** : Remplacer `[PermissionLabel]` par `[RequirePermission]`

---

### 10. CommandeController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/Commande` | GET | `VIEW_COMMANDE` | ⏳ |
| `/api/Commande/getCommandeById` | GET | `VIEW_COMMANDE` | ⏳ |
| `/api/Commande/add` | POST | `CREATE_COMMANDE` | ⏳ |
| `/api/Commande/edit` | POST | `EDIT_COMMANDE` | ⏳ |
| `/api/Commande/delete` | POST | `DELETE_COMMANDE` | ⏳ |
| `/api/Commande/execute` | POST | `EXECUTE_COMMANDE` | ⏳ |

**Filtrage par poste** : Oui (via relation Commande → Compteur → Equipement → Cellule → Poste)

---

### 11. AlarmsController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/Alarms` | GET | `VIEW_ALARM` | ⏳ |
| `/api/Alarms/getAlarmById` | GET | `VIEW_ALARM` | ⏳ |
| `/api/Alarms/acknowledge` | POST | `ACKNOWLEDGE_ALARM` | ⏳ |

**Filtrage par poste** : Oui (via relation Alarm → Equipement → Cellule → Poste)

---

### 12. ErrorController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/Error` | GET | `VIEW_ERROR` | ⏳ |
| `/api/Error/getErrorById` | GET | `VIEW_ERROR` | ⏳ |

**Filtrage par poste** : Oui (via relation Error → Equipement → Cellule → Poste)

---

### 13. EventsController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/Events` | GET | `VIEW_EVENT` | ⏳ |
| `/api/Events/getEventById` | GET | `VIEW_EVENT` | ⏳ |

**Filtrage par poste** : Oui (via relation Event → Equipement → Cellule → Poste)

---

### 14. CodeObisController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/CodeObis` | GET | `VIEW_CODEOBIS` | ⏳ |
| `/api/CodeObis/getCodeObisById` | GET | `VIEW_CODEOBIS` | ⏳ |
| `/api/CodeObis/add` | POST | `CREATE_CODEOBIS` | ⏳ |
| `/api/CodeObis/edit` | POST | `EDIT_CODEOBIS` | ⏳ |
| `/api/CodeObis/delete` | POST | `DELETE_CODEOBIS` | ⏳ |

**Filtrage par poste** : Non (codes OBIS globaux)

---

### 15. CompteurEquipementController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/CompteurEquipement` | GET | `VIEW_COMPTEUREQUIPEMENT` | ⏳ |
| `/api/CompteurEquipement/add` | POST | `CREATE_COMPTEUREQUIPEMENT` | ⏳ |
| `/api/CompteurEquipement/delete` | POST | `DELETE_COMPTEUREQUIPEMENT` | ⏳ |

**Filtrage par poste** : Oui (liaison entre Compteur et Equipement)

---

### 16. CommandeCompteurController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/CommandeCompteur` | GET | `VIEW_COMMANDECOMPTEUR` | ⏳ |
| `/api/CommandeCompteur/add` | POST | `CREATE_COMMANDECOMPTEUR` | ⏳ |
| `/api/CommandeCompteur/delete` | POST | `DELETE_COMMANDECOMPTEUR` | ⏳ |

**Filtrage par poste** : Oui (liaison entre Commande et Compteur)

---

### 17. AssociationKeyController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/AssociationKey` | GET | `VIEW_ASSOCIATIONKEY` | ⏳ |
| `/api/AssociationKey/add` | POST | `CREATE_ASSOCIATIONKEY` | ⏳ |
| `/api/AssociationKey/edit` | POST | `EDIT_ASSOCIATIONKEY` | ⏳ |
| `/api/AssociationKey/delete` | POST | `DELETE_ASSOCIATIONKEY` | ⏳ |

**Filtrage par poste** : À définir selon la logique métier

---

### 18. GxdlmsprofilgenericController ⏳

| Endpoint | Méthode HTTP | Permission Cible | Status |
|----------|--------------|------------------|--------|
| `/api/Gxdlmsprofilgeneric` | GET | `VIEW_DLMSPROFILE` | ⏳ |
| `/api/Gxdlmsprofilgeneric/add` | POST | `CREATE_DLMSPROFILE` | ⏳ |
| `/api/Gxdlmsprofilgeneric/edit` | POST | `EDIT_DLMSPROFILE` | ⏳ |
| `/api/Gxdlmsprofilgeneric/delete` | POST | `DELETE_DLMSPROFILE` | ⏳ |

**Filtrage par poste** : Non (profils DLMS globaux)

---

## 📊 STATISTIQUES

### Progression
- **Contrôleurs migrés** : 7/18 (39%)
- **Contrôleurs à migrer** : 11/18 (61%)

### Par catégorie
- **Type 1 (SANS poste)** : 4 migrés, 4 à migrer
- **Type 2 (AVEC poste)** : 3 migrés, 7 à migrer

---

## 🎯 LISTE COMPLÈTE DES PERMISSIONS

### Permissions implémentées ✅

```sql
-- Postes
VIEW_POSTE
CREATE_POSTE
EDIT_POSTE
DELETE_POSTE

-- Cellules
VIEW_CELLULE
CREATE_CELLULE
EDIT_CELLULE
DELETE_CELLULE

-- Équipements
VIEW_EQUIPEMENT
CREATE_EQUIPEMENT
EDIT_EQUIPEMENT
DELETE_EQUIPEMENT

-- Utilisateurs
VIEW_USER
CREATE_USER
EDIT_USER
DELETE_USER
ASSIGN_USER_POSTE

-- Fabricants
VIEW_FABRICANT
CREATE_FABRICANT
EDIT_FABRICANT
DELETE_FABRICANT

-- Type Commande
VIEW_TYPECOMMANDE

-- Compteurs
VIEW_COMPTEUR
CREATE_COMPTEUR
EDIT_COMPTEUR
DELETE_COMPTEUR
SYNC_COMPTEUR
```

### Permissions à créer ⏳

```sql
-- Rôles
VIEW_ROLE
CREATE_ROLE
EDIT_ROLE
DELETE_ROLE
MANAGE_ROLE

-- Permissions
VIEW_PERMISSION
CREATE_PERMISSION
EDIT_PERMISSION
DELETE_PERMISSION
MANAGE_PERMISSION

-- Commandes
VIEW_COMMANDE
CREATE_COMMANDE
EDIT_COMMANDE
DELETE_COMMANDE
EXECUTE_COMMANDE
APPROVE_COMMANDE

-- Alarmes
VIEW_ALARM
ACKNOWLEDGE_ALARM

-- Erreurs
VIEW_ERROR

-- Événements
VIEW_EVENT

-- Codes OBIS
VIEW_CODEOBIS
CREATE_CODEOBIS
EDIT_CODEOBIS
DELETE_CODEOBIS

-- Compteur-Équipement
VIEW_COMPTEUREQUIPEMENT
CREATE_COMPTEUREQUIPEMENT
DELETE_COMPTEUREQUIPEMENT

-- Commande-Compteur
VIEW_COMMANDECOMPTEUR
CREATE_COMMANDECOMPTEUR
DELETE_COMMANDECOMPTEUR

-- Clés d'association
VIEW_ASSOCIATIONKEY
CREATE_ASSOCIATIONKEY
EDIT_ASSOCIATIONKEY
DELETE_ASSOCIATIONKEY

-- Profils DLMS
VIEW_DLMSPROFILE
CREATE_DLMSPROFILE
EDIT_DLMSPROFILE
DELETE_DLMSPROFILE
```

---

## 🔗 LIENS UTILES

- [GUIDE_MIGRATION_CONTROLLERS.md](GUIDE_MIGRATION_CONTROLLERS.md) - Guide complet de migration
- [SCRIPT_PERMISSIONS_CONTROLLERS_MIGRES.sql](SCRIPT_PERMISSIONS_CONTROLLERS_MIGRES.sql) - Script SQL des permissions
- [COMPARAISON_SYSTEMES_PERMISSIONS.md](COMPARAISON_SYSTEMES_PERMISSIONS.md) - Explication du système par concept
- [SUIVI_MIGRATION_CONTROLLERS.md](SUIVI_MIGRATION_CONTROLLERS.md) - Tableau de suivi détaillé

---

**Dernière mise à jour** : Janvier 2026

