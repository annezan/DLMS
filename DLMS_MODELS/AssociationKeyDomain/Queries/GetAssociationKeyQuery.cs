using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AssociationKeyDomain.Queries
{
    public class GetAssociationKeyQuery : IRequest<ResponseBase<List<AssociationKeyResponse>>>
    {
        public Guid UserId { get; set; } // Added for access checks
        
        public GetAssociationKeyQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
