-- =========================================
-- Script de seed pour Rôles et Permissions
-- Système DLMS
-- =========================================

-- ==================
-- 1. CRÉATION DES RÔLES
-- ==================

DECLARE @AdminRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @GestionnaireRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @TechnicienRoleId UNIQUEIDENTIFIER = NEWID();
DECLARE @SuperviseurRoleId UNIQUEIDENTIFIER = NEWID();

-- Rôle Administrateur
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (@AdminRoleId, 'Administrateur', 'ADMIN', 'Accès complet au système', GETDATE(), 0);

-- Rôle Gestionnaire de Poste
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (@GestionnaireRoleId, 'Gestionnaire de Poste', 'GESTIONNAIRE', 'Gestion d''un poste spécifique', GETDATE(), 0);

-- Rôle Technicien
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (@TechnicienRoleId, 'Technicien', 'TECHNICIEN', 'Accès aux opérations techniques', GETDATE(), 0);

-- Rôle Superviseur
INSERT INTO Roles (Id, Libelle, Code, Description, CreatedDate, IsDeleted)
VALUES (@SuperviseurRoleId, 'Superviseur', 'SUPERVISEUR', 'Supervision de plusieurs postes', GETDATE(), 0);

-- ==================
-- 2. CRÉATION DES PERMISSIONS
-- ==================

-- Permissions POSTES
DECLARE @ViewPosteId UNIQUEIDENTIFIER = NEWID();
DECLARE @CreatePosteId UNIQUEIDENTIFIER = NEWID();
DECLARE @EditPosteId UNIQUEIDENTIFIER = NEWID();
DECLARE @DeletePosteId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewPosteId, 'Voir les postes', 'Permet de visualiser les postes', 'VIEW_POSTE', GETDATE(), 0),
(@CreatePosteId, 'Créer un poste', 'Permet de créer un nouveau poste', 'CREATE_POSTE', GETDATE(), 0),
(@EditPosteId, 'Modifier un poste', 'Permet de modifier un poste existant', 'EDIT_POSTE', GETDATE(), 0),
(@DeletePosteId, 'Supprimer un poste', 'Permet de supprimer un poste', 'DELETE_POSTE', GETDATE(), 0);

-- Permissions CELLULES
DECLARE @ViewCelluleId UNIQUEIDENTIFIER = NEWID();
DECLARE @CreateCelluleId UNIQUEIDENTIFIER = NEWID();
DECLARE @EditCelluleId UNIQUEIDENTIFIER = NEWID();
DECLARE @DeleteCelluleId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewCelluleId, 'Voir les cellules', 'Permet de visualiser les cellules', 'VIEW_CELLULE', GETDATE(), 0),
(@CreateCelluleId, 'Créer une cellule', 'Permet de créer une nouvelle cellule', 'CREATE_CELLULE', GETDATE(), 0),
(@EditCelluleId, 'Modifier une cellule', 'Permet de modifier une cellule', 'EDIT_CELLULE', GETDATE(), 0),
(@DeleteCelluleId, 'Supprimer une cellule', 'Permet de supprimer une cellule', 'DELETE_CELLULE', GETDATE(), 0);

-- Permissions ÉQUIPEMENTS
DECLARE @ViewEquipementId UNIQUEIDENTIFIER = NEWID();
DECLARE @CreateEquipementId UNIQUEIDENTIFIER = NEWID();
DECLARE @EditEquipementId UNIQUEIDENTIFIER = NEWID();
DECLARE @DeleteEquipementId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewEquipementId, 'Voir les équipements', 'Permet de visualiser les équipements', 'VIEW_EQUIPEMENT', GETDATE(), 0),
(@CreateEquipementId, 'Créer un équipement', 'Permet de créer un équipement', 'CREATE_EQUIPEMENT', GETDATE(), 0),
(@EditEquipementId, 'Modifier un équipement', 'Permet de modifier un équipement', 'EDIT_EQUIPEMENT', GETDATE(), 0),
(@DeleteEquipementId, 'Supprimer un équipement', 'Permet de supprimer un équipement', 'DELETE_EQUIPEMENT', GETDATE(), 0);

-- Permissions COMPTEURS
DECLARE @ViewCompteurId UNIQUEIDENTIFIER = NEWID();
DECLARE @CreateCompteurId UNIQUEIDENTIFIER = NEWID();
DECLARE @EditCompteurId UNIQUEIDENTIFIER = NEWID();
DECLARE @DeleteCompteurId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewCompteurId, 'Voir les compteurs', 'Permet de visualiser les compteurs', 'VIEW_COMPTEUR', GETDATE(), 0),
(@CreateCompteurId, 'Créer un compteur', 'Permet de créer un compteur', 'CREATE_COMPTEUR', GETDATE(), 0),
(@EditCompteurId, 'Modifier un compteur', 'Permet de modifier un compteur', 'EDIT_COMPTEUR', GETDATE(), 0),
(@DeleteCompteurId, 'Supprimer un compteur', 'Permet de supprimer un compteur', 'DELETE_COMPTEUR', GETDATE(), 0);

-- Permissions COMMANDES
DECLARE @ViewCommandeId UNIQUEIDENTIFIER = NEWID();
DECLARE @CreateCommandeId UNIQUEIDENTIFIER = NEWID();
DECLARE @ExecuteCommandeId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewCommandeId, 'Voir les commandes', 'Permet de visualiser les commandes', 'VIEW_COMMANDE', GETDATE(), 0),
(@CreateCommandeId, 'Créer une commande', 'Permet de créer une commande', 'CREATE_COMMANDE', GETDATE(), 0),
(@ExecuteCommandeId, 'Exécuter une commande', 'Permet d''exécuter une commande', 'EXECUTE_COMMANDE', GETDATE(), 0);

-- Permissions UTILISATEURS
DECLARE @ViewUsersId UNIQUEIDENTIFIER = NEWID();
DECLARE @CreateUserId UNIQUEIDENTIFIER = NEWID();
DECLARE @EditUserId UNIQUEIDENTIFIER = NEWID();
DECLARE @DeleteUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewUsersId, 'Voir les utilisateurs', 'Permet de visualiser les utilisateurs', 'VIEW_USERS', GETDATE(), 0),
(@CreateUserId, 'Créer un utilisateur', 'Permet de créer un utilisateur', 'CREATE_USER', GETDATE(), 0),
(@EditUserId, 'Modifier un utilisateur', 'Permet de modifier un utilisateur', 'EDIT_USER', GETDATE(), 0),
(@DeleteUserId, 'Supprimer un utilisateur', 'Permet de supprimer un utilisateur', 'DELETE_USER', GETDATE(), 0);

-- Permissions RAPPORTS
DECLARE @ViewReportsId UNIQUEIDENTIFIER = NEWID();
DECLARE @ExportReportsId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) VALUES
(@ViewReportsId, 'Voir les rapports', 'Permet de visualiser les rapports', 'VIEW_REPORTS', GETDATE(), 0),
(@ExportReportsId, 'Exporter les rapports', 'Permet d''exporter les rapports', 'EXPORT_REPORTS', GETDATE(), 0);

-- ==================
-- 3. ATTRIBUTION DES PERMISSIONS AUX RÔLES
-- ==================

-- ---------------------
-- ADMINISTRATEUR : Toutes les permissions
-- ---------------------
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedDate, IsDeleted) VALUES
-- Postes
(NEWID(), @AdminRoleId, @ViewPosteId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @CreatePosteId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @EditPosteId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @DeletePosteId, GETDATE(), 0),
-- Cellules
(NEWID(), @AdminRoleId, @ViewCelluleId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @CreateCelluleId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @EditCelluleId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @DeleteCelluleId, GETDATE(), 0),
-- Équipements
(NEWID(), @AdminRoleId, @ViewEquipementId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @CreateEquipementId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @EditEquipementId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @DeleteEquipementId, GETDATE(), 0),
-- Compteurs
(NEWID(), @AdminRoleId, @ViewCompteurId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @CreateCompteurId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @EditCompteurId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @DeleteCompteurId, GETDATE(), 0),
-- Commandes
(NEWID(), @AdminRoleId, @ViewCommandeId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @CreateCommandeId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @ExecuteCommandeId, GETDATE(), 0),
-- Utilisateurs
(NEWID(), @AdminRoleId, @ViewUsersId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @CreateUserId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @EditUserId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @DeleteUserId, GETDATE(), 0),
-- Rapports
(NEWID(), @AdminRoleId, @ViewReportsId, GETDATE(), 0),
(NEWID(), @AdminRoleId, @ExportReportsId, GETDATE(), 0);

-- ---------------------
-- GESTIONNAIRE DE POSTE : Accès limité à son poste (si PosteId assigné)
-- ---------------------
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedDate, IsDeleted) VALUES
-- Lecture de son poste
(NEWID(), @GestionnaireRoleId, @ViewPosteId, GETDATE(), 0),
-- Cellules de son poste
(NEWID(), @GestionnaireRoleId, @ViewCelluleId, GETDATE(), 0),
-- Équipements de son poste
(NEWID(), @GestionnaireRoleId, @ViewEquipementId, GETDATE(), 0),
(NEWID(), @GestionnaireRoleId, @EditEquipementId, GETDATE(), 0),
-- Compteurs
(NEWID(), @GestionnaireRoleId, @ViewCompteurId, GETDATE(), 0),
-- Commandes
(NEWID(), @GestionnaireRoleId, @ViewCommandeId, GETDATE(), 0),
(NEWID(), @GestionnaireRoleId, @CreateCommandeId, GETDATE(), 0),
(NEWID(), @GestionnaireRoleId, @ExecuteCommandeId, GETDATE(), 0),
-- Rapports
(NEWID(), @GestionnaireRoleId, @ViewReportsId, GETDATE(), 0);

-- ---------------------
-- TECHNICIEN : Opérations techniques
-- ---------------------
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedDate, IsDeleted) VALUES
-- Postes
(NEWID(), @TechnicienRoleId, @ViewPosteId, GETDATE(), 0),
-- Cellules
(NEWID(), @TechnicienRoleId, @ViewCelluleId, GETDATE(), 0),
-- Équipements
(NEWID(), @TechnicienRoleId, @ViewEquipementId, GETDATE(), 0),
(NEWID(), @TechnicienRoleId, @EditEquipementId, GETDATE(), 0),
-- Compteurs
(NEWID(), @TechnicienRoleId, @ViewCompteurId, GETDATE(), 0),
(NEWID(), @TechnicienRoleId, @CreateCompteurId, GETDATE(), 0),
(NEWID(), @TechnicienRoleId, @EditCompteurId, GETDATE(), 0),
-- Commandes
(NEWID(), @TechnicienRoleId, @ViewCommandeId, GETDATE(), 0),
(NEWID(), @TechnicienRoleId, @ExecuteCommandeId, GETDATE(), 0),
-- Rapports
(NEWID(), @TechnicienRoleId, @ViewReportsId, GETDATE(), 0);

-- ---------------------
-- SUPERVISEUR : Supervision multi-postes
-- ---------------------
INSERT INTO RolePermissions (Id, RoleId, PermissionId, CreatedDate, IsDeleted) VALUES
-- Postes
(NEWID(), @SuperviseurRoleId, @ViewPosteId, GETDATE(), 0),
-- Cellules
(NEWID(), @SuperviseurRoleId, @ViewCelluleId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @CreateCelluleId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @EditCelluleId, GETDATE(), 0),
-- Équipements
(NEWID(), @SuperviseurRoleId, @ViewEquipementId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @CreateEquipementId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @EditEquipementId, GETDATE(), 0),
-- Compteurs
(NEWID(), @SuperviseurRoleId, @ViewCompteurId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @CreateCompteurId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @EditCompteurId, GETDATE(), 0),
-- Commandes
(NEWID(), @SuperviseurRoleId, @ViewCommandeId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @CreateCommandeId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @ExecuteCommandeId, GETDATE(), 0),
-- Rapports
(NEWID(), @SuperviseurRoleId, @ViewReportsId, GETDATE(), 0),
(NEWID(), @SuperviseurRoleId, @ExportReportsId, GETDATE(), 0);

-- ==================
-- 4. VÉRIFICATION
-- ==================

PRINT '=== RÔLES CRÉÉS ==='
SELECT Libelle, Code, Description FROM Roles WHERE IsDeleted = 0;

PRINT '=== PERMISSIONS CRÉÉES ==='
SELECT Libelle, Action FROM Permissions WHERE IsDeleted = 0;

PRINT '=== ATTRIBUTION DES PERMISSIONS ==='
SELECT 
    r.Libelle AS Role, 
    COUNT(rp.Id) AS NombrePermissions
FROM Roles r
LEFT JOIN RolePermissions rp ON r.Id = rp.RoleId AND rp.IsDeleted = 0
WHERE r.IsDeleted = 0
GROUP BY r.Libelle;

PRINT 'Script terminé avec succès !'

