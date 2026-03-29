-- Fix: restaurer les valeurs brutes corrompues par GetDisplayConversionFactor (commit 557a977)
-- Les entrées post-28/03/2026 00:40 UTC ont Value = RawValue * 0.001 au lieu de Value = RawValue
-- Ce script restaure Value = RawValue pour toutes les entrées affectées

-- 1. Vérifier l'étendue des données affectées (lecture seule)
-- SELECT COUNT(*) AS affected_rows
-- FROM gxdlmsprofilgenericdetails
-- WHERE raw_value IS NOT NULL
--   AND value <> raw_value;

-- 2. Appliquer la correction
UPDATE gxdlmsprofilgenericdetails
SET value = raw_value
WHERE raw_value IS NOT NULL
  AND value <> raw_value;
