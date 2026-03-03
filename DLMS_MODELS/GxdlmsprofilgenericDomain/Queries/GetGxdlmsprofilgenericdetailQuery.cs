using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Queries
{
    public class GetGxdlmsprofilgenericdetailQuery : IRequest<ResponseBase<List<GxdlmsprofilgenericdetailResponse>>>
    {
        public string NumeroCompteur { get; set; }
        public int GxdlmsprofilgenericId { get; set; }
        public DateTime DateEnr { get; set; }
        public Guid UserId { get; set; }
        
        public GetGxdlmsprofilgenericdetailQuery(){}
        
        public GetGxdlmsprofilgenericdetailQuery(string numeroCompteur, int gxdlmsprofilgenericId, DateTime dateEnr)
        {
            this.NumeroCompteur = numeroCompteur;
            this.GxdlmsprofilgenericId = gxdlmsprofilgenericId;
            this.DateEnr = dateEnr;
        }

        public GetGxdlmsprofilgenericdetailQuery(string numeroCompteur, int gxdlmsprofilgenericId, DateTime dateEnr, Guid userId)
        {
            this.NumeroCompteur = numeroCompteur;
            this.GxdlmsprofilgenericId = gxdlmsprofilgenericId;
            this.DateEnr = dateEnr;
            this.UserId = userId;
        }
    }
}
