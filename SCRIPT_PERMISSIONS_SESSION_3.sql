-- ================================================================
-- Script de création des permissions - SESSION 3
-- Contrôleurs migrés : Users, CompteurEquipement, CommandeCompteur, Alarms, Events
-- Date : 2026-01-05
-- ================================================================

-- ================================================================
-- 1. PERMISSIONS POUR UsersController
-- ================================================================
INSERT INTO Permissions (Id, Code, Label, Groupe)
VALUES 
    (NEWID(), 'VIEW_USER', 'Consulter les utilisateurs', 'USERS'),
    (NEWID(), 'CREATE_USER', 'Créer un utilisateur', 'USERS'),
    (NEWID(), 'EDIT_USER', 'Modifier un utilisateur', 'USERS'),
    (NEWID(), 'DELETE_USER', 'Supprimer un utilisateur', 'USERS'),
    (NEWID(), 'UNLOCK_USER', 'Débloquer un compte utilisateur', 'USERS'),
    (NEWID(), 'ASSIGN_POSTE', 'Assigner un poste à un utilisateur', 'USERS');

-- ================================================================
-- 2. PERMISSIONS POUR CompteurEquipementController
-- ================================================================
INSERT INTO Permissions (Id, Code, Label, Groupe)
VALUES 
    (NEWID(), 'VIEW_COMPTEUR_EQUIPEMENT', 'Consulter les relations Compteur-Equipement', 'COMPTEUR_EQUIPEMENT'),
    (NEWID(), 'CREATE_COMPTEUR_EQUIPEMENT', 'Créer une relation Compteur-Equipement', 'COMPTEUR_EQUIPEMENT'),
    (NEWID(), 'DELETE_COMPTEUR_EQUIPEMENT', 'Supprimer une relation Compteur-Equipement', 'COMPTEUR_EQUIPEMENT');

-- ================================================================
-- 3. PERMISSIONS POUR CommandeCompteurController
-- ================================================================
INSERT INTO Permissions (Id, Code, Label, Groupe)
VALUES 
    (NEWID(), 'VIEW_COMMANDE_COMPTEUR', 'Consulter les relations Commande-Compteur', 'COMMANDE_COMPTEUR'),
    (NEWID(), 'DELETE_COMMANDE_COMPTEUR', 'Supprimer une relation Commande-Compteur', 'COMMANDE_COMPTEUR');

-- ================================================================
-- 4. PERMISSIONS POUR AlarmsController (Table de référence)
-- ================================================================
INSERT INTO Permissions (Id, Code, Label, Groupe)
VALUES 
    (NEWID(), 'VIEW_ALARM', 'Consulter les alarmes', 'ALARMS');

-- ================================================================
-- 5. PERMISSIONS POUR EventsController (Table de référence)
-- ================================================================
INSERT INTO Permissions (Id, Code, Label, Groupe)
VALUES 
    (NEWID(), 'VIEW_EVENT', 'Consulter les événements', 'EVENTS');

-- ================================================================
-- 6. ATTRIBUTION DES PERMISSIONS AU RÔLE ADMINISTRATEUR
-- ================================================================

-- Récupérer l'ID du rôle Administrateur
DECLARE @AdminRoleId UNIQUEIDENTIFIER;
SELECT @AdminRoleId = Id FROM Roles WHERE Code = 'ADMIN';

-- Attribuer toutes les permissions de cette session au rôle Administrateur
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT @AdminRoleId, Id
FROM Permissions
WHERE Code IN (
    -- Users
    'VIEW_USER', 'CREATE_USER', 'EDIT_USER', 'DELETE_USER', 'UNLOCK_USER', 'ASSIGN_POSTE',
    
    -- CompteurEquipement
    'VIEW_COMPTEUR_EQUIPEMENT', 'CREATE_COMPTEUR_EQUIPEMENT', 'DELETE_COMPTEUR_EQUIPEMENT',
    
    -- CommandeCompteur
    'VIEW_COMMANDE_COMPTEUR', 'DELETE_COMMANDE_COMPTEUR',
    
    -- Alarms et Events
    'VIEW_ALARM', 'VIEW_EVENT'
)
AND NOT EXISTS (
    SELECT 1 
    FROM RolePermissions rp 
    WHERE rp.RoleId = @AdminRoleId 
    AND rp.PermissionId = Permissions.Id
);

-- ================================================================
-- 7. ATTRIBUTION DES PERMISSIONS AU RÔLE GESTIONNAIRE DE POSTE
-- ================================================================

-- Récupérer l'ID du rôle Gestionnaire de Poste
DECLARE @GestionnaireRoleId UNIQUEIDENTIFIER;
SELECT @GestionnaireRoleId = Id FROM Roles WHERE Code = 'GESTIONNAIRE_POSTE';

-- Attribuer les permissions de lecture et de gestion des relations au Gestionnaire
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT @GestionnaireRoleId, Id
FROM Permissions
WHERE Code IN (
    -- Users (lecture seule)
    'VIEW_USER',
    
    -- CompteurEquipement (lecture et création)
    'VIEW_COMPTEUR_EQUIPEMENT', 'CREATE_COMPTEUR_EQUIPEMENT', 'DELETE_COMPTEUR_EQUIPEMENT',
    
    -- CommandeCompteur (lecture et suppression)
    'VIEW_COMMANDE_COMPTEUR', 'DELETE_COMMANDE_COMPTEUR',
    
    -- Alarms et Events (lecture seule)
    'VIEW_ALARM', 'VIEW_EVENT'
)
AND NOT EXISTS (
    SELECT 1 
    FROM RolePermissions rp 
    WHERE rp.RoleId = @GestionnaireRoleId 
    AND rp.PermissionId = Permissions.Id
);

-- ================================================================
-- FIN DU SCRIPT
-- ================================================================

PRINT '✅ Permissions créées avec succès pour la SESSION 3';
PRINT '📊 Contrôleurs migrés : Users, CompteurEquipement, CommandeCompteur, Alarms, Events';

