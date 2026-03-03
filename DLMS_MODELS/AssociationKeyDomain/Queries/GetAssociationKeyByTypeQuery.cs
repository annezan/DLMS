using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AssocationKeyDomain.Queries
{
    public class GetAssociationKeyByTypeQuery : IRequest<ResponseBase<List<AssociationKeyResponse>>>
    {
        public string CompteurId { get; set; }
        public string Type { get; set; }
        public Guid UserId { get; set; } // Added for access checks
        
        public GetAssociationKeyByTypeQuery(){ }
        public GetAssociationKeyByTypeQuery(string type,string compteurid)
        {
            this.Type = type;
            this.CompteurId = compteurid;
        }
    }
}
