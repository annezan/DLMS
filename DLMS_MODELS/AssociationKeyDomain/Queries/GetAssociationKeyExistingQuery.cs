using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using MediatR;

namespace DLMS_MODELS.AssocationKeyDomain.Queries
{
    public class GetAssociationKeyExistingQuery : IRequest<ResponseBase<AssociationKeyResponse>>
    {
        public string Keyvalue { get; set; }
        public string Compteurid { get; set; }
        public Guid UserId { get; set; } // Added for access checks

        public GetAssociationKeyExistingQuery(){ }
        public GetAssociationKeyExistingQuery(string keyvalue,string compteurid)
        {
            this.Keyvalue = keyvalue;
            this.Compteurid = compteurid;
        }
    }
}
