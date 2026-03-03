using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands;

/// <summary>
/// Commande pour assigner ou retirer un poste à un utilisateur
/// </summary>
public class UsersAssignPosteCommand : IRequest<ResponseBase<UsersResponse>>
{
    /// <summary>
    /// ID de l'utilisateur
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// ID du poste à assigner (null pour retirer le poste)
    /// </summary>
    public int? PosteId { get; set; }
}

