using DLMS_MODELS.AssociationKeyDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.AssociationKeyDomain.Commands
{
    public class AssociationKeyAddCommand : IRequest<ResponseBase<AssociationKeyResponse>>
    {
        public string CompteurId { get; set; }

        public string Type { get; set; } = null!;

        public string Keyvalue { get; set; } = null!;

        public string Keyname { get; set; } = null!;

        public string? Pwd { get; set; }
        
        public Guid UserId { get; set; } // Added for access checks
        
        public string CreatedBy { get; set; }

    }
}
