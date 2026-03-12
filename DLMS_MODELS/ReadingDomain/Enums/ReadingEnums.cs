namespace DLMS_MODELS.ReadingDomain.Enums;

public enum ReadingCycleStatus
{
    EnCours = 0,
    Termine = 1,
    Annule = 2
}

public enum ReadingSessionStatus
{
    EnCours = 0,
    Termine = 1,
    BudgetExpire = 2,
    Annule = 3
}

public enum MeterReadingResult
{
    NonTraite = 0,
    Lu = 1,
    IpMorte = 2,
    ConcentrateurInstable = 3,
    IpLente = 4,
    BudgetExpire = 5,
    AbandonDefinitif = 6,
    EchecLecture = 7,
    DifferePasseSuivante = 8,
    CleManquante = 9
}
