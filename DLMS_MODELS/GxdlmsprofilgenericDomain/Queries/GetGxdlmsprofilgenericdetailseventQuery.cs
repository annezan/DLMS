using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Queries
{
    public class GetGxdlmsprofilgenericdetailseventQuery : IRequest<ResponseBase<List<GxdlmsprofilgenericdetailseventResponse>>>
    {
        public string NumeroCompteur { get; set; }
        public Guid UserId { get; set; }
        
        public GetGxdlmsprofilgenericdetailseventQuery(){}
        
        public GetGxdlmsprofilgenericdetailseventQuery(string numeroCompteur)
        {
            this.NumeroCompteur = numeroCompteur;
        }

        public GetGxdlmsprofilgenericdetailseventQuery(string numeroCompteur, Guid userId)
        {
            this.NumeroCompteur = numeroCompteur;
            this.UserId = userId;
        }
    }
}
