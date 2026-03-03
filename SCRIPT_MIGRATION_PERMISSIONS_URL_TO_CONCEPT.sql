-- ================================================================
-- SCRIPT DE MIGRATION DES PERMISSIONS URL VERS CONCEPT
-- Date: 5 janvier 2026
-- Description: Met à jour les permissions existantes en base de données
--              du système par URL vers le système par concept
-- 
-- IMPORTANT: Ce script met à jour uniquement les permissions.
--            Il ne modifie PAS les associations RolePermissions.
-- ================================================================

USE [DLMS_DB]
GO

PRINT '======================================================';
PRINT 'Début de la migration des permissions URL → Concept';
PRINT '======================================================';
PRINT '';

-- ================================================================
-- 1. POSTE (4 permissions)
-- ================================================================

PRINT '-- Migration des permissions POSTE...';

-- VIEW_POSTE
UPDATE Permissions
SET Action = 'VIEW_POSTE',
    Libelle = 'Voir les postes',
    Description = 'Permet de visualiser les postes électriques. Les utilisateurs avec un poste assigné ne voient que leur poste.'
WHERE Action = 'GET:/api/Poste';

PRINT '  ✓ GET:/api/Poste → VIEW_POSTE';

-- Note: GET:/api/Poste/getPosteById utilise aussi VIEW_POSTE, donc on le supprime
-- pour éviter les doublons après la première migration
DELETE FROM Permissions
WHERE Action = 'GET:/api/Poste/getPosteById';

PRINT '  ✓ GET:/api/Poste/getPosteById → Fusionné avec VIEW_POSTE';

-- CREATE_POSTE
UPDATE Permissions
SET Action = 'CREATE_POSTE',
    Libelle = 'Créer un poste',
    Description = 'Permet de créer un nouveau poste électrique'
WHERE Action = 'POST:/api/Poste/add';

PRINT '  ✓ POST:/api/Poste/add → CREATE_POSTE';

-- EDIT_POSTE
UPDATE Permissions
SET Action = 'EDIT_POSTE',
    Libelle = 'Modifier un poste',
    Description = 'Permet de modifier un poste existant. Les utilisateurs avec un poste assigné ne peuvent modifier que leur poste.'
WHERE Action = 'POST:/api/Poste/edit';

PRINT '  ✓ POST:/api/Poste/edit → EDIT_POSTE';

-- DELETE_POSTE
UPDATE Permissions
SET Action = 'DELETE_POSTE',
    Libelle = 'Supprimer un poste',
    Description = 'Permet de supprimer un poste. Les utilisateurs avec un poste assigné ne peuvent supprimer que leur poste.'
WHERE Action = 'POST:/api/Poste/delete';

PRINT '  ✓ POST:/api/Poste/delete → DELETE_POSTE';

-- ================================================================
-- 2. CELLULE (4 permissions)
-- ================================================================

PRINT '';
PRINT '-- Migration des permissions CELLULE...';

-- VIEW_CELLULE
UPDATE Permissions
SET Action = 'VIEW_CELLULE',
    Libelle = 'Voir les cellules',
    Description = 'Permet de visualiser les cellules électriques. Les utilisateurs avec un poste assigné ne voient que les cellules de leur poste.'
WHERE Action = 'GET:/api/Cellule';

PRINT '  ✓ GET:/api/Cellule → VIEW_CELLULE';

-- Fusionner les autres endpoints GET avec VIEW_CELLULE
DELETE FROM Permissions
WHERE Action IN (
    'GET:/api/Cellule/getCelluleById',
    'GET:/api/Cellule/getCelluleByPosteId'
);

PRINT '  ✓ GET:/api/Cellule/getCelluleById → Fusionné avec VIEW_CELLULE';
PRINT '  ✓ GET:/api/Cellule/getCelluleByPosteId → Fusionné avec VIEW_CELLULE';

-- CREATE_CELLULE
UPDATE Permissions
SET Action = 'CREATE_CELLULE',
    Libelle = 'Créer une cellule',
    Description = 'Permet de créer une nouvelle cellule électrique. Les utilisateurs avec un poste assigné ne peuvent créer que dans leur poste.'
WHERE Action = 'POST:/api/Cellule/add';

PRINT '  ✓ POST:/api/Cellule/add → CREATE_CELLULE';

-- EDIT_CELLULE
UPDATE Permissions
SET Action = 'EDIT_CELLULE',
    Libelle = 'Modifier une cellule',
    Description = 'Permet de modifier une cellule existante. Les utilisateurs avec un poste assigné ne peuvent modifier que les cellules de leur poste.'
WHERE Action IN ('PUT:/api/Cellule/edit', 'POST:/api/Cellule/edit');

PRINT '  ✓ PUT:/api/Cellule/edit → EDIT_CELLULE';

-- DELETE_CELLULE
UPDATE Permissions
SET Action = 'DELETE_CELLULE',
    Libelle = 'Supprimer une cellule',
    Description = 'Permet de supprimer une cellule. Les utilisateurs avec un poste assigné ne peuvent supprimer que les cellules de leur poste.'
WHERE Action IN ('DELETE:/api/Cellule/delete', 'POST:/api/Cellule/delete');

PRINT '  ✓ DELETE:/api/Cellule/delete → DELETE_CELLULE';

-- ================================================================
-- 3. EQUIPEMENT (4 permissions)
-- ================================================================

PRINT '';
PRINT '-- Migration des permissions EQUIPEMENT...';

-- VIEW_EQUIPEMENT
UPDATE Permissions
SET Action = 'VIEW_EQUIPEMENT',
    Libelle = 'Voir les équipements',
    Description = 'Permet de visualiser les équipements électriques. Les utilisateurs avec un poste assigné ne voient que les équipements de leur poste.'
WHERE Action = 'GET:/api/Equipement';

PRINT '  ✓ GET:/api/Equipement → VIEW_EQUIPEMENT';

-- Fusionner les autres endpoints GET avec VIEW_EQUIPEMENT
DELETE FROM Permissions
WHERE Action IN (
    'GET:/api/Equipement/getEquipementById',
    'GET:/api/Equipement/getEquipementByCelluleId'
);

PRINT '  ✓ GET:/api/Equipement/getEquipementById → Fusionné avec VIEW_EQUIPEMENT';
PRINT '  ✓ GET:/api/Equipement/getEquipementByCelluleId → Fusionné avec VIEW_EQUIPEMENT';

-- CREATE_EQUIPEMENT
UPDATE Permissions
SET Action = 'CREATE_EQUIPEMENT',
    Libelle = 'Créer un équipement',
    Description = 'Permet de créer un nouvel équipement électrique. Les utilisateurs avec un poste assigné ne peuvent créer que dans leur poste.'
WHERE Action = 'POST:/api/Equipement/add';

PRINT '  ✓ POST:/api/Equipement/add → CREATE_EQUIPEMENT';

-- EDIT_EQUIPEMENT
UPDATE Permissions
SET Action = 'EDIT_EQUIPEMENT',
    Libelle = 'Modifier un équipement',
    Description = 'Permet de modifier un équipement existant. Les utilisateurs avec un poste assigné ne peuvent modifier que les équipements de leur poste.'
WHERE Action IN ('PUT:/api/Equipement/edit', 'POST:/api/Equipement/edit');

PRINT '  ✓ POST:/api/Equipement/edit → EDIT_EQUIPEMENT';

-- DELETE_EQUIPEMENT
UPDATE Permissions
SET Action = 'DELETE_EQUIPEMENT',
    Libelle = 'Supprimer un équipement',
    Description = 'Permet de supprimer un équipement. Les utilisateurs avec un poste assigné ne peuvent supprimer que les équipements de leur poste.'
WHERE Action IN ('DELETE:/api/Equipement/delete', 'POST:/api/Equipement/delete');

PRINT '  ✓ POST:/api/Equipement/delete → DELETE_EQUIPEMENT';

-- ================================================================
-- FIN DU SCRIPT
-- ================================================================

PRINT '';
PRINT '======================================================';
PRINT 'Migration terminée avec succès !';
PRINT '======================================================';
PRINT '';
PRINT 'Résumé des modifications :';
PRINT '  - POSTE      : 4 permissions migrées (1 fusionnée)';
PRINT '  - CELLULE    : 4 permissions migrées (2 fusionnées)';
PRINT '  - EQUIPEMENT : 4 permissions migrées (2 fusionnées)';
PRINT '';
PRINT 'Total : 12 permissions migrées, 5 permissions fusionnées';
PRINT '';
PRINT '⚠️ ATTENTION :';
PRINT 'Les associations RolePermissions ont été conservées.';
PRINT 'Les permissions fusionnées ont été supprimées automatiquement.';
PRINT '======================================================';

-- Vérification finale
PRINT '';
PRINT 'Vérification des permissions migrées :';
SELECT 
    Action,
    Libelle,
    Description
FROM Permissions
WHERE Action IN (
    'VIEW_POSTE', 'CREATE_POSTE', 'EDIT_POSTE', 'DELETE_POSTE',
    'VIEW_CELLULE', 'CREATE_CELLULE', 'EDIT_CELLULE', 'DELETE_CELLULE',
    'VIEW_EQUIPEMENT', 'CREATE_EQUIPEMENT', 'EDIT_EQUIPEMENT', 'DELETE_EQUIPEMENT'
)
ORDER BY Action;

GO

