-- Migration : Ajout colonnes Scaler (Unite, Exposant, ScalerGurux) sur Gxdlmsprofilgenericdetails
--             Ajout colonnes TC/TT (RapportTC, RapportTT, numerateurs, denominateurs) sur Compteur
-- Date : 2026-03-31

-- =============================================================
-- 1. Table Gxdlmsprofilgenericdetails : colonnes scaler
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name = 'Unite')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetails ADD Unite NVARCHAR(50) NULL;
    PRINT 'Colonne Unite ajoutée à Gxdlmsprofilgenericdetails';
END
ELSE
    PRINT 'Colonne Unite existe déjà dans Gxdlmsprofilgenericdetails';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name = 'Exposant')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetails ADD Exposant INT NULL;
    PRINT 'Colonne Exposant ajoutée à Gxdlmsprofilgenericdetails';
END
ELSE
    PRINT 'Colonne Exposant existe déjà dans Gxdlmsprofilgenericdetails';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name = 'ScalerGurux')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetails ADD ScalerGurux FLOAT NULL;
    PRINT 'Colonne ScalerGurux ajoutée à Gxdlmsprofilgenericdetails';
END
ELSE
    PRINT 'Colonne ScalerGurux existe déjà dans Gxdlmsprofilgenericdetails';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name = 'DateCreation')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetails ADD DateCreation DATETIME2 NULL;
    PRINT 'Colonne DateCreation ajoutée à Gxdlmsprofilgenericdetails';
END
ELSE
    PRINT 'Colonne DateCreation existe déjà dans Gxdlmsprofilgenericdetails';

-- RawValue (régularisation si absent)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name = 'RawValue')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetails ADD RawValue NVARCHAR(MAX) NULL;
    PRINT 'Colonne RawValue ajoutée à Gxdlmsprofilgenericdetails';
END
ELSE
    PRINT 'Colonne RawValue existe déjà dans Gxdlmsprofilgenericdetails';

-- =============================================================
-- 2. Table Compteur : colonnes TC/TT
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Compteur') AND name = 'RapportTC')
BEGIN
    ALTER TABLE Compteur ADD RapportTC FLOAT NULL;
    PRINT 'Colonne RapportTC ajoutée à Compteur';
END
ELSE
    PRINT 'Colonne RapportTC existe déjà dans Compteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Compteur') AND name = 'RapportTT')
BEGIN
    ALTER TABLE Compteur ADD RapportTT FLOAT NULL;
    PRINT 'Colonne RapportTT ajoutée à Compteur';
END
ELSE
    PRINT 'Colonne RapportTT existe déjà dans Compteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Compteur') AND name = 'TCNumerateur')
BEGIN
    ALTER TABLE Compteur ADD TCNumerateur FLOAT NULL;
    PRINT 'Colonne TCNumerateur ajoutée à Compteur';
END
ELSE
    PRINT 'Colonne TCNumerateur existe déjà dans Compteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Compteur') AND name = 'TCDenominateur')
BEGIN
    ALTER TABLE Compteur ADD TCDenominateur FLOAT NULL;
    PRINT 'Colonne TCDenominateur ajoutée à Compteur';
END
ELSE
    PRINT 'Colonne TCDenominateur existe déjà dans Compteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Compteur') AND name = 'TTNumerateur')
BEGIN
    ALTER TABLE Compteur ADD TTNumerateur FLOAT NULL;
    PRINT 'Colonne TTNumerateur ajoutée à Compteur';
END
ELSE
    PRINT 'Colonne TTNumerateur existe déjà dans Compteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Compteur') AND name = 'TTDenominateur')
BEGIN
    ALTER TABLE Compteur ADD TTDenominateur FLOAT NULL;
    PRINT 'Colonne TTDenominateur ajoutée à Compteur';
END
ELSE
    PRINT 'Colonne TTDenominateur existe déjà dans Compteur';

-- =============================================================
-- 3. Table ResultatCommandeCompteur : colonnes scaler
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ResultatCommandeCompteur') AND name = 'RawValue')
BEGIN
    ALTER TABLE ResultatCommandeCompteur ADD RawValue NVARCHAR(MAX) NULL;
    PRINT 'Colonne RawValue ajoutée à ResultatCommandeCompteur';
END
ELSE
    PRINT 'Colonne RawValue existe déjà dans ResultatCommandeCompteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ResultatCommandeCompteur') AND name = 'Unite')
BEGIN
    ALTER TABLE ResultatCommandeCompteur ADD Unite NVARCHAR(50) NULL;
    PRINT 'Colonne Unite ajoutée à ResultatCommandeCompteur';
END
ELSE
    PRINT 'Colonne Unite existe déjà dans ResultatCommandeCompteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ResultatCommandeCompteur') AND name = 'Exposant')
BEGIN
    ALTER TABLE ResultatCommandeCompteur ADD Exposant INT NULL;
    PRINT 'Colonne Exposant ajoutée à ResultatCommandeCompteur';
END
ELSE
    PRINT 'Colonne Exposant existe déjà dans ResultatCommandeCompteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ResultatCommandeCompteur') AND name = 'ScalerGurux')
BEGIN
    ALTER TABLE ResultatCommandeCompteur ADD ScalerGurux FLOAT NULL;
    PRINT 'Colonne ScalerGurux ajoutée à ResultatCommandeCompteur';
END
ELSE
    PRINT 'Colonne ScalerGurux existe déjà dans ResultatCommandeCompteur';

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('ResultatCommandeCompteur') AND name = 'DateCreation')
BEGIN
    ALTER TABLE ResultatCommandeCompteur ADD DateCreation DATETIME2 NULL;
    PRINT 'Colonne DateCreation ajoutée à ResultatCommandeCompteur';
END
ELSE
    PRINT 'Colonne DateCreation existe déjà dans ResultatCommandeCompteur';

PRINT '';
PRINT '=== Migration scaler + TC/TT + ResultatCommande terminée ===';
