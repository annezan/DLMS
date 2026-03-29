-- ============================================================
-- NETTOYAGE des donnees du 27/03/2026
-- Session #24 (Cycle #24) — donnees corrompues/incompletes
--
-- Contexte : 185 "lus" mais seulement 18 en base,
-- 1638 duplicate key, 188 ObjectDisposedException
-- ============================================================
-- EXECUTER EN TRANSACTION — VERIFIER LES COUNTS AVANT COMMIT

BEGIN TRANSACTION;

-- ============================================================
-- 1. Profils generiques (donnees de mesure)
-- ============================================================

-- 1a. Compter avant suppression
SELECT 'Gxdlmsprofilgenericdetails' AS TableName, COUNT(*) AS RowCount
FROM dbo.Gxdlmsprofilgenericdetails
WHERE DateEnr >= '2026-03-27 00:00:00' AND DateEnr < '2026-03-28 00:00:00';

SELECT 'Gxdlmsprofilgenericdetailsevents' AS TableName, COUNT(*) AS RowCount
FROM dbo.Gxdlmsprofilgenericdetailsevents
WHERE DateEnr >= '2026-03-27 00:00:00' AND DateEnr < '2026-03-28 00:00:00';

-- 1b. Supprimer les profils du 27/03
DELETE FROM dbo.Gxdlmsprofilgenericdetails
WHERE DateEnr >= '2026-03-27 00:00:00' AND DateEnr < '2026-03-28 00:00:00';

DELETE FROM dbo.Gxdlmsprofilgenericdetailsevents
WHERE DateEnr >= '2026-03-27 00:00:00' AND DateEnr < '2026-03-28 00:00:00';

-- ============================================================
-- 2. Historique de lecture incremental (curseurs)
--    IMPORTANT : reinitialiser pour que le prochain cycle
--    relise les donnees du 27/03
-- ============================================================

SELECT 'MeterProfileReadHistory' AS TableName, COUNT(*) AS RowCount
FROM dbo.MeterProfileReadHistory
WHERE LastReadAt >= '2026-03-27 00:00:00' AND LastReadAt < '2026-03-28 00:00:00';

-- Remettre le curseur au 26/03 23:00 pour forcer la relecture
UPDATE dbo.MeterProfileReadHistory
SET LastReadUpTo = '2026-03-26 23:00:00',
    UpdatedAt = GETUTCDATE()
WHERE LastReadAt >= '2026-03-27 00:00:00' AND LastReadAt < '2026-03-28 00:00:00';

-- ============================================================
-- 3. Sessions multi-pass (metadata de la session #24)
--    Les FK sont en CASCADE, donc supprimer ReadingSession
--    supprime automatiquement SessionPassResult,
--    MeterReadingStatus, IpSessionStats
-- ============================================================

SELECT 'ReadingSession' AS TableName, COUNT(*) AS RowCount
FROM dbo.ReadingSession
WHERE DateDebut >= '2026-03-27 00:00:00' AND DateDebut < '2026-03-28 00:00:00';

DELETE FROM dbo.ReadingSession
WHERE DateDebut >= '2026-03-27 00:00:00' AND DateDebut < '2026-03-28 00:00:00';

-- ============================================================
-- 4. Cycles de lecture
-- ============================================================

SELECT 'ReadingCycle' AS TableName, COUNT(*) AS RowCount
FROM dbo.ReadingCycle
WHERE DateDebut >= '2026-03-27 00:00:00' AND DateDebut < '2026-03-28 00:00:00';

DELETE FROM dbo.ReadingCycle
WHERE DateDebut >= '2026-03-27 00:00:00' AND DateDebut < '2026-03-28 00:00:00';

-- ============================================================
-- VERIFICATION FINALE
-- ============================================================

SELECT 'POST-CLEANUP Gxdlmsprofilgenericdetails' AS Check_, COUNT(*) AS Remaining
FROM dbo.Gxdlmsprofilgenericdetails
WHERE DateEnr >= '2026-03-27 00:00:00' AND DateEnr < '2026-03-28 00:00:00';

SELECT 'POST-CLEANUP MeterProfileReadHistory curseurs' AS Check_, COUNT(*) AS ResetCount
FROM dbo.MeterProfileReadHistory
WHERE LastReadUpTo = '2026-03-26 23:00:00';

-- Si tout est OK :
-- COMMIT TRANSACTION;

-- Si probleme :
-- ROLLBACK TRANSACTION;
