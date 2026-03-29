-- ============================================================
-- Migration: AddMultiPassReading
-- Tables: ReadingCycle, ReadingSession, SessionPassResult,
--         MeterReadingStatus, IpSessionStats, ReadingConfiguration
-- Compatible SQL Server 2016+
-- ============================================================

-- Verification: ne pas re-executer si deja applique
IF OBJECT_ID('dbo.ReadingCycle', 'U') IS NOT NULL
BEGIN
    PRINT 'Tables multi-pass deja presentes. Migration ignoree.';
    RETURN;
END

BEGIN TRANSACTION;
BEGIN TRY

-- === 1. ReadingCycle ===
CREATE TABLE [dbo].[ReadingCycle] (
    [Id]               INT            IDENTITY(1,1) NOT NULL,
    [Libelle]          NVARCHAR(MAX)  NOT NULL,
    [DateDebut]        DATETIME2      NOT NULL,
    [DateFin]          DATETIME2      NULL,
    [Statut]           INT            NOT NULL,
    [TotalCompteurs]   INT            NOT NULL,
    [CompteursLus]     INT            NOT NULL,
    [MaxSessions]      INT            NOT NULL,
    [SessionActuelle]  INT            NOT NULL,
    [CreatedAt]        DATETIME2      NULL,
    [UpdatedAt]        DATETIME2      NULL,
    [DeletedAt]        DATETIME2      NULL,
    [CreatedBy]        NVARCHAR(MAX)  NOT NULL,
    [UpdatedBy]        NVARCHAR(MAX)  NOT NULL,
    [DeletedBy]        NVARCHAR(MAX)  NOT NULL,
    [IsArchive]        BIT            NOT NULL,
    CONSTRAINT [PK_ReadingCycle] PRIMARY KEY CLUSTERED ([Id])
);

-- === 2. ReadingSession ===
CREATE TABLE [dbo].[ReadingSession] (
    [Id]                       INT            IDENTITY(1,1) NOT NULL,
    [ReadingCycleId]           INT            NOT NULL,
    [NumeroSession]            INT            NOT NULL,
    [DateDebut]                DATETIME2      NOT NULL,
    [DateFin]                  DATETIME2      NULL,
    [Statut]                   INT            NOT NULL,
    [CompteursEnScope]         INT            NOT NULL,
    [CompteursLus]             INT            NOT NULL,
    [CompteursEchoues]         INT            NOT NULL,
    [DureeMs]                  BIGINT         NOT NULL,
    [DureeLectureMs]           BIGINT         NOT NULL,
    [DureePauseMs]             BIGINT         NOT NULL,
    [NombreIps]                INT            NOT NULL,
    [IpsAccessibles]           INT            NOT NULL,
    [TauxReussite]             FLOAT          NOT NULL,
    [DebitCompteursParMinute]  FLOAT          NOT NULL,
    [CreatedAt]                DATETIME2      NULL,
    [UpdatedAt]                DATETIME2      NULL,
    [DeletedAt]                DATETIME2      NULL,
    [CreatedBy]                NVARCHAR(MAX)  NOT NULL,
    [UpdatedBy]                NVARCHAR(MAX)  NOT NULL,
    [DeletedBy]                NVARCHAR(MAX)  NOT NULL,
    [IsArchive]                BIT            NOT NULL,
    CONSTRAINT [PK_ReadingSession] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_ReadingSession_ReadingCycle_ReadingCycleId]
        FOREIGN KEY ([ReadingCycleId]) REFERENCES [dbo].[ReadingCycle]([Id])
        ON DELETE CASCADE
);

CREATE INDEX [IX_ReadingSession_ReadingCycleId]
    ON [dbo].[ReadingSession] ([ReadingCycleId]);

-- === 3. SessionPassResult ===
CREATE TABLE [dbo].[SessionPassResult] (
    [Id]                       INT            IDENTITY(1,1) NOT NULL,
    [ReadingSessionId]         INT            NOT NULL,
    [NumeroPasse]              INT            NOT NULL,
    [DateDebut]                DATETIME2      NOT NULL,
    [DateFin]                  DATETIME2      NULL,
    [BudgetSeconds]            INT            NOT NULL,
    [CompteursEnScope]         INT            NOT NULL,
    [CompteursLus]             INT            NOT NULL,
    [CompteursEchoues]         INT            NOT NULL,
    [CompteursDifferes]        INT            NOT NULL,
    [IpsDifferees]             INT            NOT NULL,
    [DureeMs]                  BIGINT         NOT NULL,
    [CanaryTimeoutSeconds]     INT            NOT NULL,
    [CachedTimeoutSeconds]     INT            NOT NULL,
    [UncachedTimeoutSeconds]   INT            NOT NULL,
    [MaxConsecutiveFailures]   INT            NOT NULL,
    [CooldownCount]            INT            NOT NULL,
    [CooldownSeconds]          INT            NOT NULL,
    [CreatedAt]                DATETIME2      NULL,
    [UpdatedAt]                DATETIME2      NULL,
    [DeletedAt]                DATETIME2      NULL,
    [CreatedBy]                NVARCHAR(MAX)  NOT NULL,
    [UpdatedBy]                NVARCHAR(MAX)  NOT NULL,
    [DeletedBy]                NVARCHAR(MAX)  NOT NULL,
    [IsArchive]                BIT            NOT NULL,
    CONSTRAINT [PK_SessionPassResult] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_SessionPassResult_ReadingSession_ReadingSessionId]
        FOREIGN KEY ([ReadingSessionId]) REFERENCES [dbo].[ReadingSession]([Id])
        ON DELETE CASCADE
);

CREATE INDEX [IX_SessionPassResult_ReadingSessionId]
    ON [dbo].[SessionPassResult] ([ReadingSessionId]);

-- === 4. MeterReadingStatus ===
CREATE TABLE [dbo].[MeterReadingStatus] (
    [Id]                     INT            IDENTITY(1,1) NOT NULL,
    [ReadingSessionId]       INT            NOT NULL,
    [CompteurEquipementId]   INT            NOT NULL,
    [NumeroPasse]            INT            NOT NULL,
    [Resultat]               INT            NOT NULL,
    [MessageErreur]          NVARCHAR(MAX)  NULL,
    [AdresseIp]              NVARCHAR(450)  NULL,
    [Port]                   NVARCHAR(MAX)  NULL,
    [NumeroCompteur]         NVARCHAR(MAX)  NULL,
    [TempsHdlcMs]            BIGINT         NULL,
    [TempsLectureMs]         BIGINT         NULL,
    [TempsClesMs]            BIGINT         NULL,
    [TempsTotalMs]           BIGINT         NULL,
    [TimeoutApplique]        INT            NULL,
    [CreatedAt]              DATETIME2      NULL,
    [UpdatedAt]              DATETIME2      NULL,
    [DeletedAt]              DATETIME2      NULL,
    [CreatedBy]              NVARCHAR(MAX)  NOT NULL,
    [UpdatedBy]              NVARCHAR(MAX)  NOT NULL,
    [DeletedBy]              NVARCHAR(MAX)  NOT NULL,
    [IsArchive]              BIT            NOT NULL,
    CONSTRAINT [PK_MeterReadingStatus] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_MeterReadingStatus_ReadingSession_ReadingSessionId]
        FOREIGN KEY ([ReadingSessionId]) REFERENCES [dbo].[ReadingSession]([Id])
        ON DELETE CASCADE,
    CONSTRAINT [FK_MeterReadingStatus_CompteurEquipement_CompteurEquipementId]
        FOREIGN KEY ([CompteurEquipementId]) REFERENCES [dbo].[CompteurEquipement]([Id])
        ON DELETE NO ACTION
);

CREATE INDEX [IX_MeterReadingStatus_ReadingSessionId_Resultat]
    ON [dbo].[MeterReadingStatus] ([ReadingSessionId], [Resultat]);

CREATE INDEX [IX_MeterReadingStatus_ReadingSessionId_CompteurEquipementId]
    ON [dbo].[MeterReadingStatus] ([ReadingSessionId], [CompteurEquipementId]);

CREATE INDEX [IX_MeterReadingStatus_CompteurEquipementId]
    ON [dbo].[MeterReadingStatus] ([CompteurEquipementId]);

CREATE INDEX [IX_MeterReadingStatus_AdresseIp]
    ON [dbo].[MeterReadingStatus] ([AdresseIp]);

-- === 5. IpSessionStats ===
CREATE TABLE [dbo].[IpSessionStats] (
    [Id]                     INT            IDENTITY(1,1) NOT NULL,
    [ReadingSessionId]       INT            NOT NULL,
    [AdresseIp]              NVARCHAR(MAX)  NOT NULL,
    [Port]                   NVARCHAR(MAX)  NOT NULL,
    [TotalTentatives]        INT            NOT NULL,
    [Reussites]              INT            NOT NULL,
    [Echecs]                 INT            NOT NULL,
    [TauxReussite]           FLOAT          NOT NULL,
    [LatenceMoyenneMs]       FLOAT          NOT NULL,
    [TcpAccessible]          BIT            NOT NULL,
    [TcpLatenceMs]           BIGINT         NOT NULL,
    [CanaryReussi]           BIT            NOT NULL,
    [EchecsConsecutifsMax]   INT            NOT NULL,
    [CreatedAt]              DATETIME2      NULL,
    [UpdatedAt]              DATETIME2      NULL,
    [DeletedAt]              DATETIME2      NULL,
    [CreatedBy]              NVARCHAR(MAX)  NOT NULL,
    [UpdatedBy]              NVARCHAR(MAX)  NOT NULL,
    [DeletedBy]              NVARCHAR(MAX)  NOT NULL,
    [IsArchive]              BIT            NOT NULL,
    CONSTRAINT [PK_IpSessionStats] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_IpSessionStats_ReadingSession_ReadingSessionId]
        FOREIGN KEY ([ReadingSessionId]) REFERENCES [dbo].[ReadingSession]([Id])
        ON DELETE CASCADE
);

CREATE INDEX [IX_IpSessionStats_ReadingSessionId]
    ON [dbo].[IpSessionStats] ([ReadingSessionId]);

-- === 6. ReadingConfiguration ===
CREATE TABLE [dbo].[ReadingConfiguration] (
    [Id]            INT            IDENTITY(1,1) NOT NULL,
    [Cle]           NVARCHAR(450)  NOT NULL,
    [Valeur]        NVARCHAR(MAX)  NOT NULL,
    [Description]   NVARCHAR(MAX)  NULL,
    [GroupeConfig]   NVARCHAR(MAX)  NULL,
    [CreatedAt]     DATETIME2      NULL,
    [UpdatedAt]     DATETIME2      NULL,
    [DeletedAt]     DATETIME2      NULL,
    [CreatedBy]     NVARCHAR(MAX)  NOT NULL,
    [UpdatedBy]     NVARCHAR(MAX)  NOT NULL,
    [DeletedBy]     NVARCHAR(MAX)  NOT NULL,
    [IsArchive]     BIT            NOT NULL,
    CONSTRAINT [PK_ReadingConfiguration] PRIMARY KEY CLUSTERED ([Id])
);

CREATE UNIQUE INDEX [IX_ReadingConfiguration_Cle]
    ON [dbo].[ReadingConfiguration] ([Cle]);

-- === Seed: configuration par defaut des 3 passes ===
INSERT INTO [dbo].[ReadingConfiguration] ([Cle], [Valeur], [Description], [GroupeConfig], [CreatedBy], [UpdatedBy], [DeletedBy], [IsArchive])
VALUES
    ('cycle.max_sessions',             '10',   'Nombre max de sessions par cycle',              'cycle',   'system', '', '', 0),
    ('session.global_ceiling_seconds', '3600', 'Duree max globale d''une session (secondes)',    'session', 'system', '', '', 0),
    ('session.max_concurrent_ips',     '8',    'Nombre max d''IPs traitees en parallele',        'session', 'system', '', '', 0),
    ('pass1.budget_seconds',           '1200', 'Budget temps passe 1',                          'pass1',   'system', '', '', 0),
    ('pass1.canary_timeout_seconds',   '180',  'Timeout canary passe 1',                        'pass1',   'system', '', '', 0),
    ('pass1.cached_timeout_seconds',   '180',  'Timeout compteur cache passe 1',                'pass1',   'system', '', '', 0),
    ('pass1.uncached_timeout_seconds', '300',  'Timeout compteur non-cache passe 1',            'pass1',   'system', '', '', 0),
    ('pass1.max_consecutive_failures', '2',    'Echecs consecutifs max passe 1',                'pass1',   'system', '', '', 0),
    ('pass1.cooldown_count',           '0',    'Nombre de cooldowns passe 1',                   'pass1',   'system', '', '', 0),
    ('pass1.cooldown_seconds',         '0',    'Duree cooldown passe 1',                        'pass1',   'system', '', '', 0),
    ('pass1.pause_after_seconds',      '300',  'Pause apres passe 1',                           'pass1',   'system', '', '', 0),
    ('pass2.budget_seconds',           '900',  'Budget temps passe 2',                          'pass2',   'system', '', '', 0),
    ('pass2.canary_timeout_seconds',   '300',  'Timeout canary passe 2',                        'pass2',   'system', '', '', 0),
    ('pass2.cached_timeout_seconds',   '240',  'Timeout compteur cache passe 2',                'pass2',   'system', '', '', 0),
    ('pass2.uncached_timeout_seconds', '360',  'Timeout compteur non-cache passe 2',            'pass2',   'system', '', '', 0),
    ('pass2.max_consecutive_failures', '3',    'Echecs consecutifs max passe 2',                'pass2',   'system', '', '', 0),
    ('pass2.cooldown_count',           '1',    'Nombre de cooldowns passe 2',                   'pass2',   'system', '', '', 0),
    ('pass2.cooldown_seconds',         '15',   'Duree cooldown passe 2',                        'pass2',   'system', '', '', 0),
    ('pass2.pause_after_seconds',      '300',  'Pause apres passe 2',                           'pass2',   'system', '', '', 0),
    ('pass3.budget_seconds',           '600',  'Budget temps passe 3',                          'pass3',   'system', '', '', 0),
    ('pass3.canary_timeout_seconds',   '420',  'Timeout canary passe 3',                        'pass3',   'system', '', '', 0),
    ('pass3.cached_timeout_seconds',   '300',  'Timeout compteur cache passe 3',                'pass3',   'system', '', '', 0),
    ('pass3.uncached_timeout_seconds', '420',  'Timeout compteur non-cache passe 3',            'pass3',   'system', '', '', 0),
    ('pass3.max_consecutive_failures', '5',    'Echecs consecutifs max passe 3',                'pass3',   'system', '', '', 0),
    ('pass3.cooldown_count',           '1',    'Nombre de cooldowns passe 3',                   'pass3',   'system', '', '', 0),
    ('pass3.cooldown_seconds',         '30',   'Duree cooldown passe 3',                        'pass3',   'system', '', '', 0),
    ('pass3.pause_after_seconds',      '0',    'Pause apres passe 3',                           'pass3',   'system', '', '', 0);

-- Enregistrer dans __EFMigrationsHistory pour que EF sache que c'est applique
IF OBJECT_ID('dbo.__EFMigrationsHistory', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]      NVARCHAR(150) NOT NULL,
        [ProductVersion]   NVARCHAR(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES ('20260312000000_AddMultiPassReading', '8.0.0');

PRINT 'Migration AddMultiPassReading appliquee avec succes.';

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'ERREUR: ' + ERROR_MESSAGE();
    THROW;
END CATCH
GO
