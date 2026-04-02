-- =============================================================
-- Script de reconfiguration des profils de lecture DLMS
-- Supprime et recrée la configuration ProfileReading
-- =============================================================

-- 1. Identifier les tables de configuration (avec et sans 's')
-- Vérifier laquelle est utilisée par le service
IF OBJECT_ID('ReadingConfigurations', 'U') IS NOT NULL
    PRINT 'Table ReadingConfigurations (avec s) existe';
IF OBJECT_ID('ReadingConfiguration', 'U') IS NOT NULL
    PRINT 'Table ReadingConfiguration (sans s) existe';

-- =============================================================
-- 2. Nettoyer les anciennes configs ProfileReading
-- =============================================================

-- Table SANS s (utilisée par le service via EF)
IF OBJECT_ID('ReadingConfiguration', 'U') IS NOT NULL
BEGIN
    DELETE FROM ReadingConfiguration WHERE GroupeConfig = 'ProfileReading';
    PRINT 'Anciennes configs ProfileReading supprimées de ReadingConfiguration';
END

-- Table AVEC s (si elle existe aussi)
IF OBJECT_ID('ReadingConfigurations', 'U') IS NOT NULL
BEGIN
    DELETE FROM ReadingConfigurations WHERE GroupeConfig = 'ProfileReading';
    PRINT 'Anciennes configs ProfileReading supprimées de ReadingConfigurations';
END

-- =============================================================
-- 3. Insérer la nouvelle configuration
-- Table ReadingConfiguration (sans s) = celle utilisée par EF
-- =============================================================

INSERT INTO ReadingConfiguration (Cle, Valeur, Description, GroupeConfig, CreatedBy, UpdatedBy, DeletedBy, IsArchive)
VALUES
-- Priorité 1 : BILLING (mensuel, fallback 30 jours)
('profile.1.obis',             '0.0.98.1.0.255',   'Billing period',                          'ProfileReading', 'system', '', '', 0),
('profile.1.priority',         '1',                 'Priorité max',                            'ProfileReading', 'system', '', '', 0),
('profile.1.timeout_seconds',  '90',                'Timeout 90s',                             'ProfileReading', 'system', '', '', 0),
('profile.1.fallback_hours',   '720',               'Fallback 30 jours (billing mensuel)',     'ProfileReading', 'system', '', '', 0),

-- Priorité 2 : TECHNIQUE (courant, tension, fréquence — intervalle ~5min)
('profile.2.obis',             '1.0.99.3.0.255',   'Profil technique',                        'ProfileReading', 'system', '', '', 0),
('profile.2.priority',         '2',                 'Priorité 2',                              'ProfileReading', 'system', '', '', 0),
('profile.2.timeout_seconds',  '90',                'Timeout 90s',                             'ProfileReading', 'system', '', '', 0),
('profile.2.fallback_hours',   '24',                'Fallback 24h',                            'ProfileReading', 'system', '', '', 0),

-- Priorité 3 : ÉNERGIE (import/export — intervalle ~1h)
('profile.3.obis',             '1.0.99.1.0.255',   'Profil énergie',                          'ProfileReading', 'system', '', '', 0),
('profile.3.priority',         '3',                 'Priorité 3',                              'ProfileReading', 'system', '', '', 0),
('profile.3.timeout_seconds',  '60',                'Timeout 60s',                             'ProfileReading', 'system', '', '', 0),
('profile.3.fallback_hours',   '24',                'Fallback 24h',                            'ProfileReading', 'system', '', '', 0),

-- Priorité 4 : TECHNIQUE 2 (puissances instantanées)
('profile.4.obis',             '1.0.99.2.0.255',   'Profil technique 2',                      'ProfileReading', 'system', '', '', 0),
('profile.4.priority',         '4',                 'Priorité 4',                              'ProfileReading', 'system', '', '', 0),
('profile.4.timeout_seconds',  '120',               'Timeout 120s',                            'ProfileReading', 'system', '', '', 0),
('profile.4.fallback_hours',   '12',                'Fallback 12h',                            'ProfileReading', 'system', '', '', 0),

-- Priorité 5-12 : ÉVÉNEMENTS
('profile.5.obis',             '0.0.99.98.0.255',  'Événements groupe 0',                     'ProfileReading', 'system', '', '', 0),
('profile.5.priority',         '5',                 'Priorité 5',                              'ProfileReading', 'system', '', '', 0),
('profile.5.timeout_seconds',  '20',                'Timeout 20s',                             'ProfileReading', 'system', '', '', 0),
('profile.5.fallback_hours',   '24',                'Fallback 24h',                            'ProfileReading', 'system', '', '', 0),

('profile.6.obis',             '0.0.99.98.1.255',  'Événements groupe 1',                     'ProfileReading', 'system', '', '', 0),
('profile.6.priority',         '6',                 'Priorité 6',                              'ProfileReading', 'system', '', '', 0),
('profile.6.timeout_seconds',  '20',                'Timeout 20s',                             'ProfileReading', 'system', '', '', 0),
('profile.6.fallback_hours',   '24',                'Fallback 24h',                            'ProfileReading', 'system', '', '', 0),

('profile.7.obis',             '0.0.99.98.2.255',  'Événements groupe 2',                     'ProfileReading', 'system', '', '', 0),
('profile.7.priority',         '7',                 'Priorité 7',                              'ProfileReading', 'system', '', '', 0),
('profile.7.timeout_seconds',  '20',                'Timeout 20s',                             'ProfileReading', 'system', '', '', 0),
('profile.7.fallback_hours',   '24',                'Fallback 24h',                            'ProfileReading', 'system', '', '', 0),

('profile.8.obis',             '0.0.99.98.3.255',  'Événements groupe 3',                     'ProfileReading', 'system', '', '', 0),
('profile.8.priority',         '8',                 'Priorité 8',                              'ProfileReading', 'system', '', '', 0),
('profile.8.timeout_seconds',  '20',                'Timeout 20s',                             'ProfileReading', 'system', '', '', 0),
('profile.8.fallback_hours',   '24',                'Fallback 24h',                            'ProfileReading', 'system', '', '', 0);

-- =============================================================
-- 4. Vérification
-- =============================================================
PRINT '';
PRINT '=== Configuration ProfileReading recréée ===';
SELECT Cle, Valeur, Description FROM ReadingConfiguration WHERE GroupeConfig = 'ProfileReading' ORDER BY Cle;
