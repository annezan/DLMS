# 📚 Catalogue Complet des Permissions - Application DLMS

**Date :** 5 janvier 2026  
**Objectif :** Lister toutes les fonctionnalités de l'application et leurs permissions associées

---

## 📊 Vue d'Ensemble

| Catégorie | Nombre d'Entités | Nombre de Permissions | Filtrage par Poste |
|-----------|------------------|----------------------|-------------------|
| **Infrastructure** | 3 | 12 | ❌ Non |
| **Gestion de Postes** | 3 | 12 | ✅ Oui |
| **Gestion de Compteurs** | 5 | 23 | ✅ Oui (via Equipement) |
| **Tables de Référence** | 5 | 9 | ❌ Non |
| **Profils DLMS** | 1 | 3 | ⚠️ Partiel (TODO) |
| **TOTAL** | **17** | **59** | - |

---

## 🔐 1. INFRASTRUCTURE & SÉCURITÉ

Ces entités gèrent l'authentification, les rôles et les permissions. Elles sont **globales** et ne sont **pas filtrées par poste**.

### 1.1 Users (Utilisateurs) 👥

**Description :** Gestion des utilisateurs de l'application  
**Filtrage par Poste :** ❌ **NON** (gestion globale)  
**Contrôleur :** `UsersController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les utilisateurs | `/api/Users` | `VIEW_USER` | GET | Récupère tous les utilisateurs |
| 2 | Voir un utilisateur par ID | `/api/Users/getuserbyId` | `VIEW_USER` | GET | Récupère un utilisateur spécifique |
| 3 | Voir un utilisateur par email | `/api/Users/getuserbyemail` | `VIEW_USER` | GET | Recherche par email |
| 4 | Créer un utilisateur | `/api/Users/add` | `CREATE_USER` | POST | Crée un nouvel utilisateur |
| 5 | Modifier un utilisateur | `/api/Users/edit` | `EDIT_USER` | POST | Modifie un utilisateur existant |
| 6 | Supprimer un utilisateur | `/api/Users/delete` | `DELETE_USER` | POST | Supprime un utilisateur |
| 7 | Débloquer un compte | `/api/Users/dislockuseraccount` | `UNLOCK_USER` | POST | Débloque le compte d'un utilisateur |
| 8 | Assigner un poste | `/api/Users/assign-poste` | `ASSIGN_POSTE` | POST | Assigne/retire un poste à un utilisateur |

**Total Permissions :** 6 permissions uniques (`VIEW_USER`, `CREATE_USER`, `EDIT_USER`, `DELETE_USER`, `UNLOCK_USER`, `ASSIGN_POSTE`)

---

### 1.2 Roles (Rôles) 🎭

**Description :** Gestion des rôles utilisateurs  
**Filtrage par Poste :** ❌ **NON**  
**Contrôleur :** `RolesController.cs`  
**Base Class :** `ApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les rôles | `/api/Roles` | `VIEW_ROLE` | GET | Récupère tous les rôles |
| 2 | Voir un rôle par ID | `/api/Roles/getRoleById` | `VIEW_ROLE` | GET | Récupère un rôle spécifique |
| 3 | Voir un rôle par code | `/api/Roles/getRoleByCode` | `VIEW_ROLE` | GET | Recherche par code |
| 4 | Créer un rôle | `/api/Roles/add` | `CREATE_ROLE` | POST | Crée un nouveau rôle |
| 5 | Modifier un rôle | `/api/Roles/edit` | `EDIT_ROLE` | POST | Modifie un rôle existant |
| 6 | Supprimer un rôle | `/api/Roles/delete` | `DELETE_ROLE` | POST | Supprime un rôle |

**Total Permissions :** 4 permissions uniques (`VIEW_ROLE`, `CREATE_ROLE`, `EDIT_ROLE`, `DELETE_ROLE`)

---

### 1.3 Permissions (Permissions) 🔑

**Description :** Gestion des permissions système  
**Filtrage par Poste :** ❌ **NON**  
**Contrôleur :** `PermissionsController.cs`  
**Base Class :** `ApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les permissions | `/api/Permissions` | `VIEW_PERMISSION` | GET | Récupère toutes les permissions |
| 2 | Voir une permission par ID | `/api/Permissions/getPermissionsById` | `VIEW_PERMISSION` | GET | Récupère une permission spécifique |

**Total Permissions :** 1 permission unique (`VIEW_PERMISSION`)

**Note :** Les opérations CREATE, EDIT, DELETE ne sont pas exposées par ce contrôleur (gestion en base de données directe).

---

## 🏢 2. GESTION DES POSTES ET INFRASTRUCTURE ÉLECTRIQUE

Ces entités représentent la structure physique des installations électriques. Elles sont **filtrées par poste** pour les utilisateurs ayant un `PosteId` assigné.

### 2.1 Poste (Postes) 🏭

**Description :** Gestion des postes électriques  
**Filtrage par Poste :** ✅ **OUI** (utilisateur avec `PosteId` ne voit que son poste)  
**Contrôleur :** `PosteController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les postes | `/api/Poste` | `GET:/api/Poste` | GET | Récupère les postes accessibles | ✅ Filtré |
| 2 | Voir un poste par ID | `/api/Poste/getPosteById` | `GET:/api/Poste/getPosteById` | GET | Récupère un poste spécifique | ✅ Vérifié |
| 3 | Créer un poste | `/api/Poste/add` | `POST:/api/Poste/add` | POST | Crée un nouveau poste | ❌ Non filtré |
| 4 | Modifier un poste | `/api/Poste/edit` | `POST:/api/Poste/edit` | POST | Modifie un poste existant | ✅ Vérifié |
| 5 | Supprimer un poste | `/api/Poste/delete` | `POST:/api/Poste/delete` | POST | Supprime un poste | ✅ Vérifié |

**Total Permissions :** 4 permissions uniques

⚠️ **ATTENTION :** Ce contrôleur utilise encore l'**ancien système par URL**. Il devrait être migré vers le **système par concept** :
- `GET:/api/Poste` → `VIEW_POSTE`
- `POST:/api/Poste/add` → `CREATE_POSTE`
- `POST:/api/Poste/edit` → `EDIT_POSTE`
- `POST:/api/Poste/delete` → `DELETE_POSTE`

---

### 2.2 Cellule (Cellules) 📦

**Description :** Gestion des cellules électriques (sous-divisions d'un poste)  
**Filtrage par Poste :** ✅ **OUI** (via `Cellule.PosteId`)  
**Contrôleur :** `CelluleController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les cellules | `/api/Cellule` | `GET:/api/Cellule` | GET | Récupère les cellules accessibles | ✅ Filtré |
| 2 | Voir une cellule par ID | `/api/Cellule/getCelluleById` | `GET:/api/Cellule/getCelluleById` | GET | Récupère une cellule spécifique | ✅ Vérifié |
| 3 | Voir les cellules d'un poste | `/api/Cellule/getCelluleByPosteId` | `GET:/api/Cellule/getCelluleByPosteId` | GET | Récupère toutes les cellules d'un poste | ✅ Vérifié |
| 4 | Créer une cellule | `/api/Cellule/add` | `POST:/api/Cellule/add` | POST | Crée une nouvelle cellule | ✅ Vérifié |
| 5 | Modifier une cellule | `/api/Cellule/edit` | `PUT:/api/Cellule/edit` | PUT | Modifie une cellule existante | ✅ Vérifié |
| 6 | Supprimer une cellule | `/api/Cellule/delete` | `DELETE:/api/Cellule/delete` | DELETE | Supprime une cellule | ✅ Vérifié |

**Total Permissions :** 4 permissions uniques

⚠️ **ATTENTION :** Ce contrôleur utilise encore l'**ancien système par URL**. Il devrait être migré vers le **système par concept** :
- `GET:/api/Cellule` → `VIEW_CELLULE`
- `POST:/api/Cellule/add` → `CREATE_CELLULE`
- `PUT:/api/Cellule/edit` → `EDIT_CELLULE`
- `DELETE:/api/Cellule/delete` → `DELETE_CELLULE`

---

### 2.3 Equipement (Équipements) ⚙️

**Description :** Gestion des équipements électriques (disjoncteurs, transformateurs, etc.)  
**Filtrage par Poste :** ✅ **OUI** (via `Equipement.Cellule.PosteId`)  
**Contrôleur :** `EquipementController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les équipements | `/api/Equipement` | `GET:/api/Equipement` | GET | Récupère les équipements accessibles | ✅ Filtré |
| 2 | Voir un équipement par ID | `/api/Equipement/getEquipementById` | `GET:/api/Equipement/getEquipementById` | GET | Récupère un équipement spécifique | ✅ Vérifié |
| 3 | Voir les équipements d'une cellule | `/api/Equipement/getEquipementByCelluleId` | `GET:/api/Equipement/getEquipementByCelluleId` | GET | Récupère tous les équipements d'une cellule | ✅ Vérifié |
| 4 | Créer un équipement | `/api/Equipement/add` | `POST:/api/Equipement/add` | POST | Crée un nouvel équipement | ✅ Vérifié |
| 5 | Modifier un équipement | `/api/Equipement/edit` | `POST:/api/Equipement/edit` | POST | Modifie un équipement existant | ✅ Vérifié |
| 6 | Supprimer un équipement | `/api/Equipement/delete` | `POST:/api/Equipement/delete` | POST | Supprime un équipement | ✅ Vérifié |

**Total Permissions :** 4 permissions uniques

⚠️ **ATTENTION :** Ce contrôleur utilise encore l'**ancien système par URL**. Il devrait être migré vers le **système par concept** :
- `GET:/api/Equipement` → `VIEW_EQUIPEMENT`
- `POST:/api/Equipement/add` → `CREATE_EQUIPEMENT`
- `POST:/api/Equipement/edit` → `EDIT_EQUIPEMENT`
- `POST:/api/Equipement/delete` → `DELETE_EQUIPEMENT`

---

## 🔌 3. GESTION DES COMPTEURS ET COMMANDES DLMS

Ces entités gèrent les compteurs électriques intelligents (DLMS) et leurs relations avec les équipements et les commandes.

### 3.1 Compteur (Compteurs) 🔋

**Description :** Gestion des compteurs électriques intelligents (DLMS/COSEM)  
**Filtrage par Poste :** ✅ **OUI** (via `CompteurEquipement → Equipement → Cellule → Poste`)  
**Contrôleur :** `CompteurController.cs`  
**Base Class :** `AuthorizedApiController`  
**Architecture :** ✅ **Clean Architecture** (logique dans les handlers)

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les compteurs | `/api/Compteur` | `VIEW_COMPTEUR` | GET | Récupère les compteurs accessibles | ✅ Handler |
| 2 | Voir un compteur par ID | `/api/Compteur/getCompteurById` | `VIEW_COMPTEUR` | GET | Récupère un compteur spécifique | ✅ Handler |
| 3 | Créer un compteur | `/api/Compteur/add` | `CREATE_COMPTEUR` | POST | Crée un nouveau compteur | ❌ Non filtré |
| 4 | Modifier un compteur | `/api/Compteur/edit` | `EDIT_COMPTEUR` | PUT | Modifie un compteur existant | ✅ Handler |
| 5 | Supprimer un compteur | `/api/Compteur/delete` | `DELETE_COMPTEUR` | DELETE | Supprime un compteur | ✅ Handler |
| 6 | Synchroniser un compteur | `/api/Compteur/MAJCompteur` | `SYNC_COMPTEUR` | POST | Met à jour les données d'un compteur | ✅ Handler |

**Total Permissions :** 5 permissions uniques (`VIEW_COMPTEUR`, `CREATE_COMPTEUR`, `EDIT_COMPTEUR`, `DELETE_COMPTEUR`, `SYNC_COMPTEUR`)

✅ **CORRECT :** Ce contrôleur utilise le **système par concept** et suit l'**architecture propre** (logique dans les handlers).

---

### 3.2 CompteurEquipement (Associations Compteur-Équipement) 🔗

**Description :** Gestion des relations Many-to-Many entre compteurs et équipements  
**Filtrage par Poste :** ✅ **OUI** (double vérification : Compteur ET Equipement)  
**Contrôleur :** `CompteurEquipementController.cs`  
**Base Class :** `AuthorizedApiController`  
**Architecture :** ✅ **Clean Architecture** (logique dans les handlers)

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Associer compteur(s) à équipement(s) | `/api/CompteurEquipement/add` | `CREATE_COMPTEUR_EQUIPEMENT` | POST | Crée une ou plusieurs associations | ✅ Handler (double) |
| 2 | Supprimer association(s) | `/api/CompteurEquipement/delete` | `DELETE_COMPTEUR_EQUIPEMENT` | DELETE | Supprime une ou plusieurs associations | ✅ Handler (double) |
| 3 | Voir équipements d'un compteur | `/api/CompteurEquipement/getCompteurEquipementByIdCompteur` | `VIEW_COMPTEUR_EQUIPEMENT` | GET | Récupère les équipements d'un compteur | ✅ Handler |
| 4 | Voir compteurs d'un équipement | `/api/CompteurEquipement/getCompteurEquipementByIdEquipement` | `VIEW_COMPTEUR_EQUIPEMENT` | GET | Récupère les compteurs d'un équipement | ✅ Handler |

**Total Permissions :** 3 permissions uniques (`VIEW_COMPTEUR_EQUIPEMENT`, `CREATE_COMPTEUR_EQUIPEMENT`, `DELETE_COMPTEUR_EQUIPEMENT`)

✅ **CORRECT :** Ce contrôleur utilise le **système par concept** et suit l'**architecture propre** (double vérification d'accès dans les handlers).

---

### 3.3 Commande (Commandes DLMS) 📡

**Description :** Gestion des commandes à envoyer aux compteurs DLMS  
**Filtrage par Poste :** ✅ **OUI** (via `CommandeCompteur → Compteur → CompteurEquipement → Equipement → Cellule → Poste`)  
**Contrôleur :** `CommandeController.cs`  
**Base Class :** `AuthorizedApiController`  
**Architecture :** ✅ **Clean Architecture** (logique dans les handlers)

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les commandes | `/api/Commande` | `VIEW_COMMANDE` | GET | Récupère les commandes accessibles | ✅ Handler |
| 2 | Voir une commande par ID | `/api/Commande/getCommandeById` | `VIEW_COMMANDE` | GET | Récupère une commande spécifique | ✅ Handler |
| 3 | Créer une commande | `/api/Commande/add` | `CREATE_COMMANDE` | POST | Crée une nouvelle commande | ❌ Non filtré |
| 4 | Modifier une commande | `/api/Commande/edit` | `EDIT_COMMANDE` | POST | Modifie une commande existante | ✅ Handler |
| 5 | Supprimer une commande | `/api/Commande/delete` | `DELETE_COMMANDE` | DELETE | Supprime une commande | ✅ Handler |

**Total Permissions :** 4 permissions uniques (`VIEW_COMMANDE`, `CREATE_COMMANDE`, `EDIT_COMMANDE`, `DELETE_COMMANDE`)

✅ **CORRECT :** Ce contrôleur utilise le **système par concept** et suit l'**architecture propre**.

**Logique de filtrage complexe :**
- Une commande est accessible uniquement si **TOUS** les compteurs associés sont accessibles (via `CommandeCompteur`).
- Un compteur est accessible si **AU MOINS UN** de ses équipements associés appartient au poste de l'utilisateur.

---

### 3.4 CommandeCompteur (Associations Commande-Compteur) 🔗

**Description :** Gestion des relations Many-to-Many entre commandes et compteurs  
**Filtrage par Poste :** ✅ **OUI** (double vérification : Commande ET Compteur)  
**Contrôleur :** `CommandeCompteurController.cs`  
**Base Class :** `AuthorizedApiController`  
**Architecture :** ✅ **Clean Architecture** (logique dans les handlers)

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les associations | `/api/CommandeCompteur` | `VIEW_COMMANDE_COMPTEUR` | GET | Récupère les associations accessibles | ✅ Handler (double) |
| 2 | Voir une association par ID | `/api/CommandeCompteur/getCommandeCompteurById` | `VIEW_COMMANDE_COMPTEUR` | GET | Récupère une association spécifique | ✅ Handler (double) |
| 3 | Supprimer une association | `/api/CommandeCompteur/delete` | `DELETE_COMMANDE_COMPTEUR` | DELETE | Supprime une association | ✅ Handler (double) |

**Total Permissions :** 2 permissions uniques (`VIEW_COMMANDE_COMPTEUR`, `DELETE_COMMANDE_COMPTEUR`)

✅ **CORRECT :** Ce contrôleur utilise le **système par concept** et suit l'**architecture propre** (double vérification d'accès dans les handlers).

---

### 3.5 AssociationKey (Clés d'Association DLMS) 🔐

**Description :** Gestion des clés de chiffrement pour les communications DLMS  
**Filtrage par Poste :** ✅ **OUI** (via `NumeroCompteur` string → Compteur)  
**Contrôleur :** `AssociationKeyController.cs`  
**Base Class :** `AuthorizedApiController`  
**Architecture :** ✅ **Clean Architecture** (logique dans les handlers)

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Lister les clés | `/api/AssociationKey` | `VIEW_ASSOCIATIONKEY` | GET | Récupère les clés accessibles | ✅ Handler |
| 2 | Voir les clés par type | `/api/AssociationKey/getAssociationKeyByType` | `VIEW_ASSOCIATIONKEY` | GET | Récupère les clés d'un compteur par type | ✅ Handler |
| 3 | Vérifier clé existante | `/api/AssociationKey/getAssociationKeyExisting` | `VIEW_ASSOCIATIONKEY` | GET | Vérifie si une clé existe | ✅ Handler |
| 4 | Créer des clés | `/api/AssociationKey/add` | `CREATE_ASSOCIATIONKEY` | POST | Crée une ou plusieurs clés | ✅ Handler |

**Total Permissions :** 2 permissions uniques (`VIEW_ASSOCIATIONKEY`, `CREATE_ASSOCIATIONKEY`)

✅ **CORRECT :** Ce contrôleur utilise le **système par concept** et suit l'**architecture propre**.

**Particularité :** Utilise `NumeroCompteur` (string) au lieu de `CompteurId` (int), nécessitant une méthode spéciale `UserHasAccessToCompteurByCompteurIdAsync()` dans `AuthorizationService`.

---

## 📋 4. TABLES DE RÉFÉRENCE (GLOBALES)

Ces entités sont des tables de référence contenant des codes et des constantes utilisés par le protocole DLMS. Elles sont **globales** et ne sont **pas filtrées par poste**.

### 4.1 CodeObis (Codes OBIS) 📊

**Description :** Codes OBIS standard (Object Identification System) pour le protocole DLMS  
**Filtrage par Poste :** ❌ **NON** (table de référence globale)  
**Contrôleur :** `CodeObisController.cs`  
**Base Class :** `ApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les codes OBIS | `/api/CodeObis` | `VIEW_CODEOBIS` | GET | Récupère tous les codes OBIS |
| 2 | Voir un code par ID | `/api/CodeObis/getCodeObisById` | `VIEW_CODEOBIS` | GET | Récupère un code OBIS spécifique |
| 3 | Voir un code par valeur | `/api/CodeObis/getCodeObisByValue` | `VIEW_CODEOBIS` | GET | Recherche par valeur OBIS |

**Total Permissions :** 1 permission unique (`VIEW_CODEOBIS`)

---

### 4.2 Fabricant (Fabricants) 🏭

**Description :** Liste des fabricants de compteurs et équipements électriques  
**Filtrage par Poste :** ❌ **NON** (table de référence globale)  
**Contrôleur :** `FabricantController.cs`  
**Base Class :** `ApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les fabricants | `/api/Fabricant` | `VIEW_FABRICANT` | GET | Récupère tous les fabricants |
| 2 | Voir un fabricant par ID | `/api/Fabricant/getFabricantById` | `VIEW_FABRICANT` | GET | Récupère un fabricant spécifique |
| 3 | Créer un fabricant | `/api/Fabricant/add` | `CREATE_FABRICANT` | POST | Crée un nouveau fabricant |
| 4 | Modifier un fabricant | `/api/Fabricant/edit` | `EDIT_FABRICANT` | POST | Modifie un fabricant existant |
| 5 | Supprimer un fabricant | `/api/Fabricant/delete` | `DELETE_FABRICANT` | POST | Supprime un fabricant |

**Total Permissions :** 4 permissions uniques (`VIEW_FABRICANT`, `CREATE_FABRICANT`, `EDIT_FABRICANT`, `DELETE_FABRICANT`)

---

### 4.3 TypeCommande (Types de Commandes) 📝

**Description :** Liste des types de commandes DLMS disponibles  
**Filtrage par Poste :** ❌ **NON** (table de référence globale)  
**Contrôleur :** `TypecommandeController.cs`  
**Base Class :** `ApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les types de commandes | `/api/Typecommande` | `VIEW_TYPECOMMANDE` | GET | Récupère tous les types de commandes |

**Total Permissions :** 1 permission unique (`VIEW_TYPECOMMANDE`)

**Note :** Les opérations CRUD complètes ne sont pas encore implémentées (marqué TODO dans le contrôleur).

---

### 4.4 Alarms (Alarmes) 🚨

**Description :** Codes d'alarmes standard DLMS  
**Filtrage par Poste :** ❌ **NON** (table de référence globale)  
**Contrôleur :** `AlarmsController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les alarmes | `/api/Alarms` | `VIEW_ALARM` | GET | Récupère tous les codes d'alarmes |
| 2 | Voir une alarme par ID | `/api/Alarms/getAlarmsById` | `VIEW_ALARM` | GET | Récupère une alarme spécifique |
| 3 | Voir une alarme par valeur | `/api/Alarms/getAlarmsByValue` | `VIEW_ALARM` | GET | Recherche par valeur d'alarme |

**Total Permissions :** 1 permission unique (`VIEW_ALARM`)

---

### 4.5 Events (Événements) 📅

**Description :** Codes d'événements standard DLMS  
**Filtrage par Poste :** ❌ **NON** (table de référence globale)  
**Contrôleur :** `EventsController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les événements | `/api/Events` | `VIEW_EVENT` | GET | Récupère tous les codes d'événements |
| 2 | Voir un événement par ID | `/api/Events/getEventsById` | `VIEW_EVENT` | GET | Récupère un événement spécifique |
| 3 | Voir un événement par valeur | `/api/Events/getEventsByValue` | `VIEW_EVENT` | GET | Recherche par valeur d'événement |

**Total Permissions :** 1 permission unique (`VIEW_EVENT`)

---

### 4.6 Error (Erreurs) ⚠️

**Description :** Codes d'erreurs DLMS  
**Filtrage par Poste :** ❌ **NON** (table de référence globale)  
**Contrôleur :** `ErrorController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description |
|---|----------------|----------|------------|--------------|-------------|
| 1 | Lister les erreurs | `/api/Error` | `VIEW_ERROR` | GET | Récupère tous les codes d'erreurs |
| 2 | Voir une erreur par ID | `/api/Error/getErrorById` | `VIEW_ERROR` | GET | Récupère une erreur spécifique |
| 3 | Voir une erreur par valeur | `/api/Error/getErrorByValue` | `VIEW_ERROR` | GET | Recherche par valeur d'erreur |

**Total Permissions :** 1 permission unique (`VIEW_ERROR`)

---

## 📊 5. PROFILS DLMS (DONNÉES HISTORIQUES)

### 5.1 Gxdlmsprofilgeneric (Profils Génériques DLMS) 📈

**Description :** Gestion des profils génériques DLMS (données historiques des compteurs)  
**Filtrage par Poste :** ⚠️ **PARTIEL** (TODO - basé sur `NumeroCompteur` string)  
**Contrôleur :** `GxdlmsprofilgenericController.cs`  
**Base Class :** `AuthorizedApiController`

| # | Fonctionnalité | Endpoint | Permission | Méthode HTTP | Description | Filtrage |
|---|----------------|----------|------------|--------------|-------------|----------|
| 1 | Ajouter détail de profil | `/api/Gxdlmsprofilgeneric/Addprofilgenericdetail` | `CREATE_DLMS_PROFILE` | POST | Ajoute un détail de profil | ⚠️ TODO |
| 2 | Ajouter événement de profil | `/api/Gxdlmsprofilgeneric/Addprofilgenericdetailsevent` | `CREATE_DLMS_PROFILE` | POST | Ajoute un événement de profil | ⚠️ TODO |
| 3 | Supprimer détail de profil | `/api/Gxdlmsprofilgeneric/deleteprofilgenericdetail` | `DELETE_DLMS_PROFILE` | POST | Supprime un détail de profil | ❌ Non |
| 4 | Supprimer événement de profil | `/api/Gxdlmsprofilgeneric/deleteprofilgenericdetailsevent` | `DELETE_DLMS_PROFILE` | POST | Supprime un événement de profil | ❌ Non |
| 5 | Voir profil par Logical Name | `/api/Gxdlmsprofilgeneric/profilgenericByLN` | `VIEW_DLMS_PROFILE` | GET | Récupère un profil par LN | ❌ Non |
| 6 | Voir profils par statut | `/api/Gxdlmsprofilgeneric/profilgenericByStatus` | `VIEW_DLMS_PROFILE` | GET | Récupère les profils par statut d'archivage | ❌ Non |
| 7 | Voir détails de profil | `/api/Gxdlmsprofilgeneric/profilgenericdetailByStatus` | `VIEW_DLMS_PROFILE` | GET | Récupère les détails d'un profil par compteur | ⚠️ TODO |
| 8 | Voir événements de profil | `/api/Gxdlmsprofilgeneric/profilgenericdetailseventByStatus` | `VIEW_DLMS_PROFILE` | GET | Récupère les événements d'un profil par compteur | ⚠️ TODO |

**Total Permissions :** 3 permissions uniques (`VIEW_DLMS_PROFILE`, `CREATE_DLMS_PROFILE`, `DELETE_DLMS_PROFILE`)

⚠️ **ATTENTION :** Le filtrage par poste n'est **pas encore implémenté** pour ce contrôleur. Les TODO dans le code indiquent qu'il faut ajouter une méthode `UserHasAccessToCompteurByNumeroAsync()` dans `AuthorizationService` car ce contrôleur utilise `NumeroCompteur` (string) au lieu de `CompteurId` (int).

---

## 📊 RÉCAPITULATIF DES PERMISSIONS PAR CATÉGORIE

### Vue Détaillée

| Catégorie | Entités | Permissions | Détail |
|-----------|---------|-------------|--------|
| **Infrastructure & Sécurité** | Users, Roles, Permissions | 11 | `VIEW_USER`, `CREATE_USER`, `EDIT_USER`, `DELETE_USER`, `UNLOCK_USER`, `ASSIGN_POSTE`, `VIEW_ROLE`, `CREATE_ROLE`, `EDIT_ROLE`, `DELETE_ROLE`, `VIEW_PERMISSION` |
| **Gestion de Postes** | Poste, Cellule, Equipement | 12 | 4 permissions × 3 entités (VIEW, CREATE, EDIT, DELETE) |
| **Gestion de Compteurs** | Compteur, CompteurEquipement, Commande, CommandeCompteur, AssociationKey | 16 | Permissions variées incluant SYNC, double vérification |
| **Tables de Référence** | CodeObis, Fabricant, TypeCommande, Alarms, Events, Error | 9 | Principalement VIEW, quelques CRUD |
| **Profils DLMS** | Gxdlmsprofilgeneric | 3 | `VIEW_DLMS_PROFILE`, `CREATE_DLMS_PROFILE`, `DELETE_DLMS_PROFILE` |
| **TOTAL** | **17** | **51** | (certaines permissions sont utilisées par plusieurs endpoints) |

---

## 🎯 LISTE COMPLÈTE DES PERMISSIONS UNIQUES

Voici la liste exhaustive de toutes les permissions à enregistrer dans votre base de données :

### Infrastructure & Sécurité (11 permissions)
1. `VIEW_USER` - Voir les utilisateurs
2. `CREATE_USER` - Créer un utilisateur
3. `EDIT_USER` - Modifier un utilisateur
4. `DELETE_USER` - Supprimer un utilisateur
5. `UNLOCK_USER` - Débloquer un compte utilisateur
6. `ASSIGN_POSTE` - Assigner un poste à un utilisateur
7. `VIEW_ROLE` - Voir les rôles
8. `CREATE_ROLE` - Créer un rôle
9. `EDIT_ROLE` - Modifier un rôle
10. `DELETE_ROLE` - Supprimer un rôle
11. `VIEW_PERMISSION` - Voir les permissions

### Gestion de Postes (12 permissions)
12. `VIEW_POSTE` - Voir les postes ⚠️ (actuellement `GET:/api/Poste`)
13. `CREATE_POSTE` - Créer un poste ⚠️ (actuellement `POST:/api/Poste/add`)
14. `EDIT_POSTE` - Modifier un poste ⚠️ (actuellement `POST:/api/Poste/edit`)
15. `DELETE_POSTE` - Supprimer un poste ⚠️ (actuellement `POST:/api/Poste/delete`)
16. `VIEW_CELLULE` - Voir les cellules ⚠️ (actuellement `GET:/api/Cellule`)
17. `CREATE_CELLULE` - Créer une cellule ⚠️ (actuellement `POST:/api/Cellule/add`)
18. `EDIT_CELLULE` - Modifier une cellule ⚠️ (actuellement `PUT:/api/Cellule/edit`)
19. `DELETE_CELLULE` - Supprimer une cellule ⚠️ (actuellement `DELETE:/api/Cellule/delete`)
20. `VIEW_EQUIPEMENT` - Voir les équipements ⚠️ (actuellement `GET:/api/Equipement`)
21. `CREATE_EQUIPEMENT` - Créer un équipement ⚠️ (actuellement `POST:/api/Equipement/add`)
22. `EDIT_EQUIPEMENT` - Modifier un équipement ⚠️ (actuellement `POST:/api/Equipement/edit`)
23. `DELETE_EQUIPEMENT` - Supprimer un équipement ⚠️ (actuellement `POST:/api/Equipement/delete`)

### Gestion de Compteurs (16 permissions)
24. `VIEW_COMPTEUR` - Voir les compteurs ✅
25. `CREATE_COMPTEUR` - Créer un compteur ✅
26. `EDIT_COMPTEUR` - Modifier un compteur ✅
27. `DELETE_COMPTEUR` - Supprimer un compteur ✅
28. `SYNC_COMPTEUR` - Synchroniser un compteur ✅
29. `VIEW_COMPTEUR_EQUIPEMENT` - Voir les associations compteur-équipement ✅
30. `CREATE_COMPTEUR_EQUIPEMENT` - Créer une association compteur-équipement ✅
31. `DELETE_COMPTEUR_EQUIPEMENT` - Supprimer une association compteur-équipement ✅
32. `VIEW_COMMANDE` - Voir les commandes ✅
33. `CREATE_COMMANDE` - Créer une commande ✅
34. `EDIT_COMMANDE` - Modifier une commande ✅
35. `DELETE_COMMANDE` - Supprimer une commande ✅
36. `VIEW_COMMANDE_COMPTEUR` - Voir les associations commande-compteur ✅
37. `DELETE_COMMANDE_COMPTEUR` - Supprimer une association commande-compteur ✅
38. `VIEW_ASSOCIATIONKEY` - Voir les clés d'association ✅
39. `CREATE_ASSOCIATIONKEY` - Créer une clé d'association ✅

### Tables de Référence (9 permissions)
40. `VIEW_CODEOBIS` - Voir les codes OBIS ✅
41. `VIEW_FABRICANT` - Voir les fabricants ✅
42. `CREATE_FABRICANT` - Créer un fabricant ✅
43. `EDIT_FABRICANT` - Modifier un fabricant ✅
44. `DELETE_FABRICANT` - Supprimer un fabricant ✅
45. `VIEW_TYPECOMMANDE` - Voir les types de commandes ✅
46. `VIEW_ALARM` - Voir les alarmes ✅
47. `VIEW_EVENT` - Voir les événements ✅
48. `VIEW_ERROR` - Voir les erreurs ✅

### Profils DLMS (3 permissions)
49. `VIEW_DLMS_PROFILE` - Voir les profils DLMS ✅
50. `CREATE_DLMS_PROFILE` - Créer un profil DLMS ✅
51. `DELETE_DLMS_PROFILE` - Supprimer un profil DLMS ✅

---

## ⚠️ ACTIONS REQUISES

### 1. Migration Urgente - Système par Concept

Les contrôleurs suivants utilisent encore l'**ancien système par URL** et doivent être migrés vers le **système par concept** :

❌ **PosteController** - 4 permissions à migrer
❌ **CelluleController** - 4 permissions à migrer  
❌ **EquipementController** - 4 permissions à migrer

**Total :** 12 permissions à corriger

### 2. Implémentation du Filtrage par Poste

Le contrôleur suivant nécessite l'implémentation du filtrage par poste :

⚠️ **GxdlmsprofilgenericController** - Filtrage partiel (TODO)

**Action :** Ajouter la méthode `UserHasAccessToCompteurByNumeroAsync(Guid userId, string numeroCompteur)` dans `AuthorizationService` (similaire à celle créée pour `AssociationKeyController`).

### 3. Scripts de Permissions à Mettre à Jour

Les scripts SQL suivants doivent être mis à jour :

1. `SCRIPT_SEED_PERMISSIONS.sql` - Remplacer les permissions URL par les permissions concept pour Poste, Cellule, Equipement
2. Créer un nouveau script `SCRIPT_CORRECTION_PERMISSIONS_URL_TO_CONCEPT.sql` pour corriger les permissions existantes en base de données

---

## 💡 RECOMMANDATIONS

### 1. Standardisation du Nommage

Toutes les permissions suivent le format `{ACTION}_{ENTITE}` :
- **Actions :** `VIEW`, `CREATE`, `EDIT`, `DELETE`, `SYNC`, `UNLOCK`, `ASSIGN`, `MANAGE`
- **Entités :** `USER`, `ROLE`, `PERMISSION`, `POSTE`, `CELLULE`, `EQUIPEMENT`, `COMPTEUR`, etc.

### 2. Attribution des Permissions aux Rôles

**Rôle Administrateur (ADMIN) :**
- Toutes les permissions (51 permissions)

**Rôle Gestionnaire (GESTIONNAIRE) :**
- Toutes les permissions `VIEW_*` (16 permissions)
- Permissions de gestion de son poste uniquement (filtrées automatiquement par `PosteId`)
- `CREATE_`, `EDIT_`, `DELETE_` pour : CELLULE, EQUIPEMENT, COMPTEUR, COMMANDE, COMPTEUR_EQUIPEMENT, COMMANDE_COMPTEUR, ASSOCIATIONKEY

**Rôle Technicien (TECHNICIEN) :**
- Toutes les permissions `VIEW_*` (16 permissions)
- `CREATE_COMMANDE`, `SYNC_COMPTEUR`
- Lecture seule pour le reste

**Rôle Superviseur (SUPERVISEUR) :**
- Toutes les permissions `VIEW_*` (16 permissions)
- Accès à tous les postes (pas de `PosteId` assigné)

### 3. Architecture Propre

✅ **Contrôleurs à suivre comme exemple :**
- `CompteurController` - Architecture propre, logique dans les handlers
- `CommandeController` - Architecture propre, logique dans les handlers
- `CompteurEquipementController` - Architecture propre, double vérification dans les handlers
- `CommandeCompteurController` - Architecture propre, double vérification dans les handlers
- `AssociationKeyController` - Architecture propre, gestion des string IDs

---

## 🎉 CONCLUSION

Vous avez **51 permissions uniques** à enregistrer dans votre système, réparties sur **17 entités fonctionnelles**.

**État actuel :**
- ✅ **39 permissions** utilisent déjà le système par concept
- ⚠️ **12 permissions** doivent être migrées du système URL vers le système concept

**Prochaines étapes :**
1. Migrer PosteController, CelluleController, EquipementController vers le système par concept
2. Compléter l'implémentation du filtrage pour GxdlmsprofilgenericController
3. Mettre à jour les scripts SQL de permissions
4. Attribuer les permissions aux rôles selon les recommandations

---

**Dernière mise à jour :** 5 janvier 2026  
**Document généré automatiquement** à partir de l'analyse des contrôleurs


