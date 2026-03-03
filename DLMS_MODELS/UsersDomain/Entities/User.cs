using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Entities;

namespace DLMS_MODELS.UsersDomain.Entities;

public class User : ModelBase
{
    public string Nom { get; set; }
    public string Prenoms { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public string MotDePasse { get; set; }
    public string? CodeValidation { get; set; }
    public DateOnly? DateNaissance { get; set; }
    
    // Relation avec Role (un utilisateur a un seul rôle)
    public Guid? RoleId { get; set; }
    public Role? Role { get; set; }
    
    // Relation avec Poste (pour limiter l'accès à un poste spécifique)
    public int? PosteId { get; set; }
    public Poste? Poste { get; set; }
    
    public bool MustChangePassword { get; set; } = false;
    public int FailedLoginAttempts { get; set; } = 0; // Nombre de tentatives échouées
    public bool IsLocked { get; set; } = false; // Indique si le compte est verrouillé
}
