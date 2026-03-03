-- ================================================================
-- SCRIPT COMPLET DES PERMISSIONS - Application DLMS
-- Date: 5 janvier 2026
-- Description: Liste exhaustive de toutes les 51 permissions du système
-- 
-- ATTENTION: Ce script consolide TOUTES les permissions.
-- Exécutez-le APRÈS avoir nettoyé les anciennes permissions URL si nécessaire.
-- ================================================================

USE [DLMS_DB]
GO

-- ================================================================
-- 1. INFRASTRUCTURE & SÉCURITÉ (11 permissions)
-- ================================================================

-- Utilisateurs (6 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les utilisateurs', 'Permet de visualiser la liste des utilisateurs et leurs détails', 'VIEW_USER', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un utilisateur', 'Permet de créer un nouvel utilisateur dans le système', 'CREATE_USER', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un utilisateur', 'Permet de modifier les informations d''un utilisateur existant', 'EDIT_USER', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un utilisateur', 'Permet de supprimer un utilisateur du système', 'DELETE_USER', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Débloquer un compte utilisateur', 'Permet de débloquer le compte d''un utilisateur verrouillé', 'UNLOCK_USER', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Assigner un poste à un utilisateur', 'Permet d''assigner ou retirer un poste à un utilisateur pour limiter son accès', 'ASSIGN_POSTE', GETDATE(), 0);

-- Rôles (4 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les rôles', 'Permet de visualiser la liste des rôles et leurs détails', 'VIEW_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un rôle', 'Permet de créer un nouveau rôle', 'CREATE_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un rôle', 'Permet de modifier un rôle existant', 'EDIT_ROLE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un rôle', 'Permet de supprimer un rôle', 'DELETE_ROLE', GETDATE(), 0);

-- Permissions (1 permission)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les permissions', 'Permet de visualiser la liste des permissions et leurs détails', 'VIEW_PERMISSION', GETDATE(), 0);

-- ================================================================
-- 2. GESTION DE POSTES (12 permissions)
-- ================================================================

-- Postes (4 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les postes', 'Permet de visualiser les postes électriques. Les utilisateurs avec un poste assigné ne voient que leur poste.', 'VIEW_POSTE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un poste', 'Permet de créer un nouveau poste électrique', 'CREATE_POSTE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un poste', 'Permet de modifier un poste existant. Les utilisateurs avec un poste assigné ne peuvent modifier que leur poste.', 'EDIT_POSTE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un poste', 'Permet de supprimer un poste. Les utilisateurs avec un poste assigné ne peuvent supprimer que leur poste.', 'DELETE_POSTE', GETDATE(), 0);

-- Cellules (4 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les cellules', 'Permet de visualiser les cellules électriques. Les utilisateurs avec un poste assigné ne voient que les cellules de leur poste.', 'VIEW_CELLULE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer une cellule', 'Permet de créer une nouvelle cellule électrique. Les utilisateurs avec un poste assigné ne peuvent créer que dans leur poste.', 'CREATE_CELLULE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier une cellule', 'Permet de modifier une cellule existante. Les utilisateurs avec un poste assigné ne peuvent modifier que les cellules de leur poste.', 'EDIT_CELLULE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer une cellule', 'Permet de supprimer une cellule. Les utilisateurs avec un poste assigné ne peuvent supprimer que les cellules de leur poste.', 'DELETE_CELLULE', GETDATE(), 0);

-- Équipements (4 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les équipements', 'Permet de visualiser les équipements électriques. Les utilisateurs avec un poste assigné ne voient que les équipements de leur poste.', 'VIEW_EQUIPEMENT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un équipement', 'Permet de créer un nouvel équipement électrique. Les utilisateurs avec un poste assigné ne peuvent créer que dans leur poste.', 'CREATE_EQUIPEMENT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un équipement', 'Permet de modifier un équipement existant. Les utilisateurs avec un poste assigné ne peuvent modifier que les équipements de leur poste.', 'EDIT_EQUIPEMENT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un équipement', 'Permet de supprimer un équipement. Les utilisateurs avec un poste assigné ne peuvent supprimer que les équipements de leur poste.', 'DELETE_EQUIPEMENT', GETDATE(), 0);

-- ================================================================
-- 3. GESTION DE COMPTEURS (16 permissions)
-- ================================================================

-- Compteurs (5 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les compteurs', 'Permet de visualiser les compteurs intelligents DLMS. Les utilisateurs avec un poste assigné ne voient que les compteurs associés aux équipements de leur poste.', 'VIEW_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un compteur', 'Permet de créer un nouveau compteur intelligent DLMS', 'CREATE_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un compteur', 'Permet de modifier un compteur existant. Les utilisateurs avec un poste assigné ne peuvent modifier que les compteurs de leur poste.', 'EDIT_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un compteur', 'Permet de supprimer un compteur. Les utilisateurs avec un poste assigné ne peuvent supprimer que les compteurs de leur poste.', 'DELETE_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Synchroniser un compteur', 'Permet de mettre à jour les données d''un compteur DLMS (opération de synchronisation)', 'SYNC_COMPTEUR', GETDATE(), 0);

-- Associations Compteur-Équipement (3 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les associations compteur-équipement', 'Permet de visualiser les associations entre compteurs et équipements', 'VIEW_COMPTEUR_EQUIPEMENT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer une association compteur-équipement', 'Permet d''associer un compteur à un équipement. Nécessite l''accès au compteur ET à l''équipement.', 'CREATE_COMPTEUR_EQUIPEMENT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer une association compteur-équipement', 'Permet de dissocier un compteur d''un équipement. Nécessite l''accès au compteur ET à l''équipement.', 'DELETE_COMPTEUR_EQUIPEMENT', GETDATE(), 0);

-- Commandes DLMS (4 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les commandes', 'Permet de visualiser les commandes DLMS. Les utilisateurs avec un poste assigné ne voient que les commandes dont TOUS les compteurs associés sont dans leur poste.', 'VIEW_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer une commande', 'Permet de créer une nouvelle commande DLMS à envoyer aux compteurs', 'CREATE_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier une commande', 'Permet de modifier une commande existante. L''utilisateur doit avoir accès à tous les compteurs de la commande.', 'EDIT_COMMANDE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer une commande', 'Permet de supprimer une commande. L''utilisateur doit avoir accès à tous les compteurs de la commande.', 'DELETE_COMMANDE', GETDATE(), 0);

-- Associations Commande-Compteur (2 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les associations commande-compteur', 'Permet de visualiser les associations entre commandes et compteurs', 'VIEW_COMMANDE_COMPTEUR', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer une association commande-compteur', 'Permet de retirer un compteur d''une commande. Nécessite l''accès à la commande ET au compteur.', 'DELETE_COMMANDE_COMPTEUR', GETDATE(), 0);

-- Clés d'Association DLMS (2 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les clés d''association', 'Permet de visualiser les clés de chiffrement pour les communications DLMS', 'VIEW_ASSOCIATIONKEY', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer une clé d''association', 'Permet de créer une nouvelle clé de chiffrement pour les communications DLMS', 'CREATE_ASSOCIATIONKEY', GETDATE(), 0);

-- ================================================================
-- 4. TABLES DE RÉFÉRENCE (9 permissions)
-- ================================================================

-- Codes OBIS (1 permission)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les codes OBIS', 'Permet de visualiser les codes OBIS standard du protocole DLMS (table de référence globale)', 'VIEW_CODEOBIS', GETDATE(), 0);

-- Fabricants (4 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les fabricants', 'Permet de visualiser la liste des fabricants de compteurs et équipements électriques (table de référence globale)', 'VIEW_FABRICANT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un fabricant', 'Permet d''ajouter un nouveau fabricant dans la liste de référence', 'CREATE_FABRICANT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Modifier un fabricant', 'Permet de modifier les informations d''un fabricant existant', 'EDIT_FABRICANT', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un fabricant', 'Permet de supprimer un fabricant de la liste de référence', 'DELETE_FABRICANT', GETDATE(), 0);

-- Types de Commandes (1 permission)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les types de commandes', 'Permet de visualiser les types de commandes DLMS disponibles (table de référence globale)', 'VIEW_TYPECOMMANDE', GETDATE(), 0);

-- Alarmes (1 permission)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les alarmes', 'Permet de visualiser les codes d''alarmes standard DLMS (table de référence globale)', 'VIEW_ALARM', GETDATE(), 0);

-- Événements (1 permission)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les événements', 'Permet de visualiser les codes d''événements standard DLMS (table de référence globale)', 'VIEW_EVENT', GETDATE(), 0);

-- Erreurs (1 permission)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les erreurs', 'Permet de visualiser les codes d''erreurs DLMS (table de référence globale)', 'VIEW_ERROR', GETDATE(), 0);

-- ================================================================
-- 5. PROFILS DLMS (3 permissions)
-- ================================================================

-- Profils Génériques DLMS (3 permissions)
INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Voir les profils DLMS', 'Permet de visualiser les profils génériques DLMS et leurs données historiques', 'VIEW_DLMS_PROFILE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Créer un profil DLMS', 'Permet d''ajouter des détails ou événements dans les profils génériques DLMS', 'CREATE_DLMS_PROFILE', GETDATE(), 0);

INSERT INTO Permissions (Id, Libelle, Description, Action, CreatedDate, IsDeleted) 
VALUES (NEWID(), 'Supprimer un profil DLMS', 'Permet de supprimer des détails ou événements dans les profils génériques DLMS', 'DELETE_DLMS_PROFILE', GETDATE(), 0);

-- ================================================================
-- FIN DU SCRIPT
-- ================================================================

PRINT '======================================================';
PRINT 'Script exécuté avec succès !';
PRINT '51 permissions ont été insérées dans la base de données.';
PRINT '======================================================';
PRINT '';
PRINT 'Répartition :';
PRINT '  - Infrastructure & Sécurité : 11 permissions';
PRINT '  - Gestion de Postes         : 12 permissions';
PRINT '  - Gestion de Compteurs      : 16 permissions';
PRINT '  - Tables de Référence       : 9 permissions';
PRINT '  - Profils DLMS              : 3 permissions';
PRINT '======================================================';
PRINT '';
PRINT 'ATTENTION : Les contrôleurs Poste, Cellule et Equipement';
PRINT 'utilisent encore l''ancien système par URL.';
PRINT 'Ils doivent être migrés vers le système par concept.';
PRINT '======================================================';

GO

