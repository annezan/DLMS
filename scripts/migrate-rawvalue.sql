-- Migration : Ajout colonne RawValue pour stocker les valeurs brutes Gurux
-- La colonne Value existante stockera désormais les valeurs converties (kWh, kW, kvar)
-- RawValue stocke les valeurs brutes (Wh, W, var) pour traçabilité

-- Table des détails de profil (registres)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetails') AND name = 'RawValue')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetails ADD RawValue NVARCHAR(MAX) NULL;
    PRINT 'Colonne RawValue ajoutée à Gxdlmsprofilgenericdetails';
END
ELSE
    PRINT 'Colonne RawValue existe déjà dans Gxdlmsprofilgenericdetails';

-- Table des détails de profil événements
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Gxdlmsprofilgenericdetailsevents') AND name = 'RawValue')
BEGIN
    ALTER TABLE Gxdlmsprofilgenericdetailsevents ADD RawValue NVARCHAR(MAX) NULL;
    PRINT 'Colonne RawValue ajoutée à Gxdlmsprofilgenericdetailsevents';
END
ELSE
    PRINT 'Colonne RawValue existe déjà dans Gxdlmsprofilgenericdetailsevents';
