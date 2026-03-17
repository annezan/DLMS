-- ============================================================
-- Migration: AddProfileReadingOptimization
-- Tables: MeterProfileReadHistory
-- Index:  UQ_ProfileDetail_NoDup on Gxdlmsprofilgenericdetails
-- Seed:   ReadingConfiguration (GroupeConfig = 'ProfileReading')
-- Compatible SQL Server 2016+
-- ============================================================

USE [db_ac2526_dlmsdb_v1_Test];
GO

-- ============================================================
-- Section 1 : MeterProfileReadHistory
-- Suivi du dernier horodatage lu par compteur/profil
-- (lecture incrementale)
-- ============================================================
IF OBJECT_ID('dbo.MeterProfileReadHistory', 'U') IS NOT NULL
BEGIN
    PRINT 'Table MeterProfileReadHistory deja presente. Section 1 ignoree.';
END
ELSE
BEGIN

    BEGIN TRANSACTION;
    BEGIN TRY

    CREATE TABLE [dbo].[MeterProfileReadHistory] (
        [Id]              INT            IDENTITY(1,1) NOT NULL,
        [CompteurSerial]  NVARCHAR(50)   NOT NULL,
        [ProfileObis]     NVARCHAR(20)   NOT NULL,
        [LastReadUpTo]    DATETIME2      NOT NULL,
        [LastReadAt]      DATETIME2      NOT NULL,
        [RowsRead]        INT            NOT NULL DEFAULT 0,
        [ReadDurationMs]  BIGINT         NOT NULL DEFAULT 0,
        [CreatedAt]       DATETIME2      NULL,
        [UpdatedAt]       DATETIME2      NULL,
        [DeletedAt]       DATETIME2      NULL,
        [CreatedBy]       NVARCHAR(100)  NOT NULL DEFAULT '',
        [UpdatedBy]       NVARCHAR(100)  NOT NULL DEFAULT '',
        [DeletedBy]       NVARCHAR(100)  NOT NULL DEFAULT '',
        [IsArchive]       BIT            NOT NULL DEFAULT 0,
        CONSTRAINT [PK_MeterProfileReadHistory] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_MeterProfile] UNIQUE ([CompteurSerial], [ProfileObis])
    );

    CREATE INDEX [IX_MeterProfileReadHistory_Serial]
        ON [dbo].[MeterProfileReadHistory] ([CompteurSerial]);

    PRINT 'Table MeterProfileReadHistory creee avec succes.';

    COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'ERREUR Section 1: ' + ERROR_MESSAGE();
        THROW;
    END CATCH

END
GO

-- ============================================================
-- Section 2 : Index unique sur Gxdlmsprofilgenericdetails
-- Prerequis : suppression des doublons existants
-- (garde le Id le plus recent par partition)
-- ============================================================
IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.Gxdlmsprofilgenericdetails')
      AND name = 'UQ_ProfileDetail_NoDup'
)
BEGIN
    PRINT 'Index UQ_ProfileDetail_NoDup deja present. Section 2 ignoree.';
END
ELSE
BEGIN

    BEGIN TRANSACTION;
    BEGIN TRY

    -- Supprimer les doublons : conserver le Id le plus recent par partition
    WITH cte AS (
        SELECT [Id], ROW_NUMBER() OVER (
            PARTITION BY [NumeroCompteur], [GxdlmsprofilgenericId], [CodeObisId], [DateEnr]
            ORDER BY [Id] DESC
        ) AS rn
        FROM [dbo].[Gxdlmsprofilgenericdetails]
        WHERE [DateEnr] IS NOT NULL
    )
    DELETE FROM cte WHERE rn > 1;

    PRINT 'Doublons supprimes dans Gxdlmsprofilgenericdetails.';

    -- Creer l'index unique filtre (exclut les lignes ou DateEnr IS NULL)
    CREATE UNIQUE INDEX [UQ_ProfileDetail_NoDup]
        ON [dbo].[Gxdlmsprofilgenericdetails]
            ([NumeroCompteur], [GxdlmsprofilgenericId], [CodeObisId], [DateEnr])
        WHERE [DateEnr] IS NOT NULL;

    PRINT 'Index UQ_ProfileDetail_NoDup cree avec succes.';

    COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'ERREUR Section 2: ' + ERROR_MESSAGE();
        THROW;
    END CATCH

END
GO

-- ============================================================
-- Section 3 : Seed ReadingConfiguration — ProfileReading
-- 12 profils ordonnes par priorite
-- ============================================================
IF EXISTS (
    SELECT 1 FROM [dbo].[ReadingConfiguration]
    WHERE [GroupeConfig] = 'ProfileReading'
)
BEGIN
    PRINT 'Configuration ProfileReading deja presente. Section 3 ignoree.';
END
ELSE
BEGIN

    BEGIN TRANSACTION;
    BEGIN TRY

    INSERT INTO [dbo].[ReadingConfiguration]
        ([Cle], [Valeur], [Description], [GroupeConfig], [CreatedBy], [UpdatedBy], [DeletedBy], [IsArchive])
    VALUES
        -- Priorite 1 : Profil 3 — 24h (lecture rapide, fallback 48h)
        ('profile.1.obis',             '1.0.99.3.0.255', 'OBIS du profil 3 (24h)',                  'ProfileReading', 'system', '', '', 0),
        ('profile.1.label',            'Profil 3 (24h)', 'Libelle du profil 3',                     'ProfileReading', 'system', '', '', 0),
        ('profile.1.priority',         '1',              'Priorite de lecture du profil 3',         'ProfileReading', 'system', '', '', 0),
        ('profile.1.timeout_seconds',  '30',             'Timeout lecture profil 3 (secondes)',     'ProfileReading', 'system', '', '', 0),
        ('profile.1.fallback_hours',   '48',             'Horizon de repli profil 3 (heures)',      'ProfileReading', 'system', '', '', 0),

        -- Priorite 2 : Profil 1 — 1h
        ('profile.2.obis',             '1.0.99.1.0.255', 'OBIS du profil 1 (1h)',                   'ProfileReading', 'system', '', '', 0),
        ('profile.2.label',            'Profil 1 (1h)',  'Libelle du profil 1',                     'ProfileReading', 'system', '', '', 0),
        ('profile.2.priority',         '2',              'Priorite de lecture du profil 1',         'ProfileReading', 'system', '', '', 0),
        ('profile.2.timeout_seconds',  '60',             'Timeout lecture profil 1 (secondes)',     'ProfileReading', 'system', '', '', 0),
        ('profile.2.fallback_hours',   '48',             'Horizon de repli profil 1 (heures)',      'ProfileReading', 'system', '', '', 0),

        -- Priorite 3 : Profil 2 — 5min (volume eleve, fallback reduit)
        ('profile.3.obis',             '1.0.99.2.0.255', 'OBIS du profil 2 (5min)',                 'ProfileReading', 'system', '', '', 0),
        ('profile.3.label',            'Profil 2 (5min)','Libelle du profil 2',                     'ProfileReading', 'system', '', '', 0),
        ('profile.3.priority',         '3',              'Priorite de lecture du profil 2',         'ProfileReading', 'system', '', '', 0),
        ('profile.3.timeout_seconds',  '120',            'Timeout lecture profil 2 (secondes)',     'ProfileReading', 'system', '', '', 0),
        ('profile.3.fallback_hours',   '12',             'Horizon de repli profil 2 (heures)',      'ProfileReading', 'system', '', '', 0),

        -- Priorite 4 : Profil defauts (fault)
        ('profile.4.obis',             '0.0.98.1.0.255', 'OBIS profil defauts',                     'ProfileReading', 'system', '', '', 0),
        ('profile.4.label',            'Profil defauts', 'Libelle profil defauts',                  'ProfileReading', 'system', '', '', 0),
        ('profile.4.priority',         '4',              'Priorite lecture profil defauts',         'ProfileReading', 'system', '', '', 0),
        ('profile.4.timeout_seconds',  '20',             'Timeout lecture profil defauts (secondes)','ProfileReading', 'system', '', '', 0),
        ('profile.4.fallback_hours',   '48',             'Horizon de repli profil defauts (heures)','ProfileReading', 'system', '', '', 0),

        -- Priorite 5 : Profil evenements 0 (0.0.99.98.0.255)
        ('profile.5.obis',             '0.0.99.98.0.255','OBIS profil evenements 0',                'ProfileReading', 'system', '', '', 0),
        ('profile.5.label',            'Evenements 0',   'Libelle profil evenements 0',             'ProfileReading', 'system', '', '', 0),
        ('profile.5.priority',         '5',              'Priorite lecture profil evenements 0',    'ProfileReading', 'system', '', '', 0),
        ('profile.5.timeout_seconds',  '20',             'Timeout lecture evenements 0 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.5.fallback_hours',   '48',             'Horizon de repli evenements 0 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 6 : Profil evenements 1 (0.0.99.98.1.255)
        ('profile.6.obis',             '0.0.99.98.1.255','OBIS profil evenements 1',                'ProfileReading', 'system', '', '', 0),
        ('profile.6.label',            'Evenements 1',   'Libelle profil evenements 1',             'ProfileReading', 'system', '', '', 0),
        ('profile.6.priority',         '6',              'Priorite lecture profil evenements 1',    'ProfileReading', 'system', '', '', 0),
        ('profile.6.timeout_seconds',  '20',             'Timeout lecture evenements 1 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.6.fallback_hours',   '48',             'Horizon de repli evenements 1 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 7 : Profil evenements 2 (0.0.99.98.2.255)
        ('profile.7.obis',             '0.0.99.98.2.255','OBIS profil evenements 2',                'ProfileReading', 'system', '', '', 0),
        ('profile.7.label',            'Evenements 2',   'Libelle profil evenements 2',             'ProfileReading', 'system', '', '', 0),
        ('profile.7.priority',         '7',              'Priorite lecture profil evenements 2',    'ProfileReading', 'system', '', '', 0),
        ('profile.7.timeout_seconds',  '20',             'Timeout lecture evenements 2 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.7.fallback_hours',   '48',             'Horizon de repli evenements 2 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 8 : Profil evenements 3 (0.0.99.98.3.255)
        ('profile.8.obis',             '0.0.99.98.3.255','OBIS profil evenements 3',                'ProfileReading', 'system', '', '', 0),
        ('profile.8.label',            'Evenements 3',   'Libelle profil evenements 3',             'ProfileReading', 'system', '', '', 0),
        ('profile.8.priority',         '8',              'Priorite lecture profil evenements 3',    'ProfileReading', 'system', '', '', 0),
        ('profile.8.timeout_seconds',  '20',             'Timeout lecture evenements 3 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.8.fallback_hours',   '48',             'Horizon de repli evenements 3 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 9 : Profil evenements 4 (0.0.99.98.4.255)
        ('profile.9.obis',             '0.0.99.98.4.255','OBIS profil evenements 4',                'ProfileReading', 'system', '', '', 0),
        ('profile.9.label',            'Evenements 4',   'Libelle profil evenements 4',             'ProfileReading', 'system', '', '', 0),
        ('profile.9.priority',         '9',              'Priorite lecture profil evenements 4',    'ProfileReading', 'system', '', '', 0),
        ('profile.9.timeout_seconds',  '20',             'Timeout lecture evenements 4 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.9.fallback_hours',   '48',             'Horizon de repli evenements 4 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 10 : Profil evenements 5 (0.0.99.98.5.255)
        ('profile.10.obis',            '0.0.99.98.5.255','OBIS profil evenements 5',                'ProfileReading', 'system', '', '', 0),
        ('profile.10.label',           'Evenements 5',   'Libelle profil evenements 5',             'ProfileReading', 'system', '', '', 0),
        ('profile.10.priority',        '10',             'Priorite lecture profil evenements 5',    'ProfileReading', 'system', '', '', 0),
        ('profile.10.timeout_seconds', '20',             'Timeout lecture evenements 5 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.10.fallback_hours',  '48',             'Horizon de repli evenements 5 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 11 : Profil evenements 6 (0.0.99.98.6.255)
        ('profile.11.obis',            '0.0.99.98.6.255','OBIS profil evenements 6',                'ProfileReading', 'system', '', '', 0),
        ('profile.11.label',           'Evenements 6',   'Libelle profil evenements 6',             'ProfileReading', 'system', '', '', 0),
        ('profile.11.priority',        '11',             'Priorite lecture profil evenements 6',    'ProfileReading', 'system', '', '', 0),
        ('profile.11.timeout_seconds', '20',             'Timeout lecture evenements 6 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.11.fallback_hours',  '48',             'Horizon de repli evenements 6 (heures)',  'ProfileReading', 'system', '', '', 0),

        -- Priorite 12 : Profil evenements 7 (0.0.99.98.7.255)
        ('profile.12.obis',            '0.0.99.98.7.255','OBIS profil evenements 7',                'ProfileReading', 'system', '', '', 0),
        ('profile.12.label',           'Evenements 7',   'Libelle profil evenements 7',             'ProfileReading', 'system', '', '', 0),
        ('profile.12.priority',        '12',             'Priorite lecture profil evenements 7',    'ProfileReading', 'system', '', '', 0),
        ('profile.12.timeout_seconds', '20',             'Timeout lecture evenements 7 (secondes)', 'ProfileReading', 'system', '', '', 0),
        ('profile.12.fallback_hours',  '48',             'Horizon de repli evenements 7 (heures)',  'ProfileReading', 'system', '', '', 0);

    PRINT 'Configuration ProfileReading seedee avec succes (12 profils).';

    COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        PRINT 'ERREUR Section 3: ' + ERROR_MESSAGE();
        THROW;
    END CATCH

END
GO

PRINT 'Migration AddProfileReadingOptimization terminee.';
GO
