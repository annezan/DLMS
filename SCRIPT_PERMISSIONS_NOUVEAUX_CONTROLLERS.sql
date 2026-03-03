-- ============================================
-- Script de création des permissions pour les nouveaux contrôleurs migrés
-- SYSTÈME PAR CONCEPT (VIEW, CREATE, EDIT, DELETE)
-- Date : Janvier 2026
-- ============================================

USE [DLMS_DB]
GO

-- ============================================
-- RÔLES (Migré depuis PermissionLabel)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les rôles', 'Permet de visualiser la liste des rôles et leurs détails', 'VIEW_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un rôle', 'Permet de créer un nouveau rôle', 'CREATE_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un rôle', 'Permet de modifier un rôle existant', 'EDIT_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un rôle', 'Permet de supprimer un rôle', 'DELETE_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Gérer les rôles', 'Permet la gestion complète des rôles (tous les droits)', 'MANAGE_ROLE', GETDATE(), 0);

-- ============================================
-- PERMISSIONS (Migré depuis PermissionLabel)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les permissions', 'Permet de visualiser la liste des permissions et leurs détails', 'VIEW_PERMISSION', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer une permission', 'Permet de créer une nouvelle permission', 'CREATE_PERMISSION', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier une permission', 'Permet de modifier une permission existante', 'EDIT_PERMISSION', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer une permission', 'Permet de supprimer une permission', 'DELETE_PERMISSION', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Gérer les permissions', 'Permet la gestion complète des permissions (tous les droits)', 'MANAGE_PERMISSION', GETDATE(), 0);

-- ============================================
-- CODE OBIS (Migré)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les codes OBIS', 'Permet de visualiser la liste des codes OBIS DLMS', 'VIEW_CODEOBIS', GETDATE(), 0);

-- Note: Les actions CREATE, EDIT, DELETE ne sont pas implémentées dans le contrôleur
-- Si vous les implémentez, ajoutez les permissions correspondantes :
-- INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
-- VALUES (NEWID(), 'Créer un code OBIS', 'Permet de créer un nouveau code OBIS', 'CREATE_CODEOBIS', GETDATE(), 0);

-- ============================================
-- COMMANDES (Migré avec logique de filtrage par poste)
-- ============================================
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les commandes', 'Permet de visualiser la liste des commandes (filtrée par poste si assigné)', 'VIEW_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer une commande', 'Permet de créer une nouvelle commande', 'CREATE_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier une commande', 'Permet de modifier une commande existante', 'EDIT_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer une commande', 'Permet de supprimer une commande', 'DELETE_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Exécuter une commande', 'Permet d''exécuter une commande sur un équipement', 'EXECUTE_COMMANDE', GETDATE(), 0);

-- ============================================
-- ATTRIBUTION DES PERMISSIONS AUX RÔLES
-- ============================================

-- ==================
-- ADMINISTRATEUR : Tous les droits
-- ==================

-- Permissions Rôles
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action IN ('VIEW_ROLE', 'CREATE_ROLE', 'EDIT_ROLE', 'DELETE_ROLE', 'MANAGE_ROLE');

-- Permissions Permissions
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action IN ('VIEW_PERMISSION', 'CREATE_PERMISSION', 'EDIT_PERMISSION', 'DELETE_PERMISSION', 'MANAGE_PERMISSION');

-- Permissions Code OBIS
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action LIKE '%_CODEOBIS';

-- Permissions Commandes
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'ADMIN'
  AND p.Action IN ('VIEW_COMMANDE', 'CREATE_COMMANDE', 'EDIT_COMMANDE', 'DELETE_COMMANDE', 'EXECUTE_COMMANDE');

-- ==================
-- GESTIONNAIRE DE POSTE : Droits limités
-- ==================

-- Permissions Rôles (lecture seule)
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'GESTIONNAIRE'
  AND p.Action = 'VIEW_ROLE';

-- Permissions Permissions (lecture seule)
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'GESTIONNAIRE'
  AND p.Action = 'VIEW_PERMISSION';

-- Permissions Code OBIS (lecture seule)
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'GESTIONNAIRE'
  AND p.Action = 'VIEW_CODEOBIS';

-- Permissions Commandes (voir, créer, exécuter - limité au poste assigné)
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'GESTIONNAIRE'
  AND p.Action IN ('VIEW_COMMANDE', 'CREATE_COMMANDE', 'EXECUTE_COMMANDE');

-- ==================
-- TECHNICIEN : Droits techniques
-- ==================

-- Permissions Code OBIS (lecture seule)
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'TECHNICIEN'
  AND p.Action = 'VIEW_CODEOBIS';

-- Permissions Commandes (voir et exécuter)
INSERT INTO RolePermission (RoleId, PermissionId)
SELECT r.Id, p.Id
FROM Role r
CROSS JOIN Permissions p
WHERE r.Code = 'TECHNICIEN'
  AND p.Action IN ('VIEW_COMMANDE', 'EXECUTE_COMMANDE');

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
    'VIEW_ROLE', 'CREATE_ROLE', 'EDIT_ROLE', 'DELETE_ROLE', 'MANAGE_ROLE',
    'VIEW_PERMISSION', 'CREATE_PERMISSION', 'EDIT_PERMISSION', 'DELETE_PERMISSION', 'MANAGE_PERMISSION',
    'VIEW_CODEOBIS',
    'VIEW_COMMANDE', 'CREATE_COMMANDE', 'EDIT_COMMANDE', 'DELETE_COMMANDE', 'EXECUTE_COMMANDE'
)
ORDER BY Action;

-- Vérifier l'attribution aux rôles
SELECT 
    r.Libelle AS Role,
    r.Code,
    p.Libelle AS Permission,
    p.Action
FROM RolePermission rp
INNER JOIN Role r ON rp.RoleId = r.Id
INNER JOIN Permissions p ON rp.PermissionId = p.Id
WHERE p.Action IN (
    'VIEW_ROLE', 'CREATE_ROLE', 'EDIT_ROLE', 'DELETE_ROLE', 'MANAGE_ROLE',
    'VIEW_PERMISSION', 'CREATE_PERMISSION', 'EDIT_PERMISSION', 'DELETE_PERMISSION', 'MANAGE_PERMISSION',
    'VIEW_CODEOBIS',
    'VIEW_COMMANDE', 'CREATE_COMMANDE', 'EDIT_COMMANDE', 'DELETE_COMMANDE', 'EXECUTE_COMMANDE'
)
ORDER BY r.Libelle, p.Action;

-- Compter les permissions par rôle
SELECT 
    r.Libelle AS Role,
    COUNT(rp.PermissionId) AS NombrePermissions
FROM Role r
LEFT JOIN RolePermission rp ON r.Id = rp.RoleId
GROUP BY r.Libelle
ORDER BY COUNT(rp.PermissionId) DESC;

GO

PRINT '✅ Permissions par CONCEPT créées et attribuées avec succès !';
PRINT '';
PRINT '📋 Nouvelles permissions créées :';
PRINT '   - RÔLES : VIEW, CREATE, EDIT, DELETE, MANAGE';
PRINT '   - PERMISSIONS : VIEW, CREATE, EDIT, DELETE, MANAGE';
PRINT '   - CODE OBIS : VIEW';
PRINT '   - COMMANDES : VIEW, CREATE, EDIT, DELETE, EXECUTE';
PRINT '';
PRINT '👥 Attribution aux rôles :';
PRINT '   - ADMIN : Tous les droits';
PRINT '   - GESTIONNAIRE : Lecture + Actions limitées (filtré par poste)';
PRINT '   - TECHNICIEN : Lecture + Exécution';

