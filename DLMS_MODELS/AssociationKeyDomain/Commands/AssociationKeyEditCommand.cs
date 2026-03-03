using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AssociationKeyDomain.Commands
{
    public class AssociationKeyEditCommand : IRequest<ResponseBase<AssociationKeyResponse>>
    {
        public string CompteurId { get; set; }

        public string Type { get; set; } = null!;

        public string Keyvalue { get; set; } = null!;

        public string Keyname { get; set; } = null!;

        public string? Pwd { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AssociationKeyEditCommand()
        {
        }
    }
}
