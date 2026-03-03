-- ============================================
-- Script de création des permissions pour les contrôleurs migrés
-- SYSTÈME PAR CONCEPT (VIEW, CREATE, EDIT, DELETE)
-- Date : Janvier 2026
-- ============================================

USE [DLMS_DB]
GO

-- ============================================
-- FABRICANT (Migré)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les fabricants', 'Permet de visualiser la liste des fabricants de compteurs et leurs détails', 'VIEW_FABRICANT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un fabricant', 'Permet de créer un nouveau fabricant de compteurs', 'CREATE_FABRICANT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un fabricant', 'Permet de modifier un fabricant existant', 'EDIT_FABRICANT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un fabricant', 'Permet de supprimer un fabricant', 'DELETE_FABRICANT', GETDATE(), 0);

-- ============================================
-- TYPE COMMANDE (Migré)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les types de commandes', 'Permet de visualiser la liste des types de commandes DLMS', 'VIEW_TYPECOMMANDE', GETDATE(), 0);

-- TODO: Ajouter CREATE_TYPECOMMANDE, EDIT_TYPECOMMANDE, DELETE_TYPECOMMANDE si ces fonctionnalités sont implémentées

-- ============================================
-- COMPTEUR (Migré avec logique de filtrage par poste)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les compteurs', 'Permet de visualiser la liste des compteurs (filtrée par poste si assigné)', 'VIEW_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un compteur', 'Permet de créer un nouveau compteur', 'CREATE_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un compteur', 'Permet de modifier un compteur existant', 'EDIT_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un compteur', 'Permet de supprimer un compteur', 'DELETE_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Synchroniser un compteur', 'Permet de synchroniser les données d''un compteur avec l''équipement', 'SYNC_COMPTEUR', GETDATE(), 0);

-- ============================================
-- CODE OBIS (À migrer)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les codes OBIS', 'Permet de visualiser la liste des codes OBIS DLMS', 'VIEW_CODEOBIS', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un code OBIS', 'Permet de créer un nouveau code OBIS', 'CREATE_CODEOBIS', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un code OBIS', 'Permet de modifier un code OBIS', 'EDIT_CODEOBIS', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un code OBIS', 'Permet de supprimer un code OBIS', 'DELETE_CODEOBIS', GETDATE(), 0);

-- ============================================
-- ATTRIBUTION DES PERMISSIONS AUX RÔLES
-- ============================================

-- Attribuer toutes les permissions Fabricant à Administrateur
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action IN ('VIEW_FABRICANT', 'CREATE_FABRICANT', 'EDIT_FABRICANT', 'DELETE_FABRICANT');

-- Attribuer les permissions lecture Fabricant au Gestionnaire
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'GESTIONNAIRE'
  AND p.Action = 'VIEW_FABRICANT';

-- Attribuer toutes les permissions TypeCommande à Administrateur
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action LIKE '%_TYPECOMMANDE';

-- Attribuer toutes les permissions Compteur à Administrateur
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action IN ('VIEW_COMPTEUR', 'CREATE_COMPTEUR', 'EDIT_COMPTEUR', 'DELETE_COMPTEUR', 'SYNC_COMPTEUR');

-- Attribuer les permissions lecture et synchronisation Compteur au Gestionnaire de Poste
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'GESTIONNAIRE'
  AND p.Action IN ('VIEW_COMPTEUR', 'SYNC_COMPTEUR');

-- Attribuer toutes les permissions CodeObis à Administrateur
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action LIKE '%_CODEOBIS';

-- ============================================
-- VÉRIFICATION
-- ============================================

-- Vérifier les permissions créées
SELECT 
    Id,
    Libelle,
    Action,
    CreatedDate
FROM Permissions
WHERE Action IN (
    'VIEW_FABRICANT', 'CREATE_FABRICANT', 'EDIT_FABRICANT', 'DELETE_FABRICANT',
    'VIEW_TYPECOMMANDE',
    'VIEW_COMPTEUR', 'CREATE_COMPTEUR', 'EDIT_COMPTEUR', 'DELETE_COMPTEUR', 'SYNC_COMPTEUR',
    'VIEW_CODEOBIS', 'CREATE_CODEOBIS', 'EDIT_CODEOBIS', 'DELETE_CODEOBIS'
)
ORDER BY Action;

-- Vérifier l'attribution aux rôles
SELECT 
    r.Libelle AS Role,
    p.Libelle AS Permission,
    p.Action
FROM RolePermission rp
INNER JOIN Role r ON rp.RoleId = r.Id
INNER JOIN Permissions p ON rp.PermissionId = p.Id
WHERE p.Action IN (
    'VIEW_FABRICANT', 'CREATE_FABRICANT', 'EDIT_FABRICANT', 'DELETE_FABRICANT',
    'VIEW_TYPECOMMANDE',
    'VIEW_COMPTEUR', 'CREATE_COMPTEUR', 'EDIT_COMPTEUR', 'DELETE_COMPTEUR', 'SYNC_COMPTEUR',
    'VIEW_CODEOBIS', 'CREATE_CODEOBIS', 'EDIT_CODEOBIS', 'DELETE_CODEOBIS'
)
ORDER BY r.Libelle, p.Action;

GO

PRINT '✅ Permissions par CONCEPT créées et attribuées avec succès !';
PRINT '📋 Format: VIEW_XXX, CREATE_XXX, EDIT_XXX, DELETE_XXX, SYNC_XXX';
