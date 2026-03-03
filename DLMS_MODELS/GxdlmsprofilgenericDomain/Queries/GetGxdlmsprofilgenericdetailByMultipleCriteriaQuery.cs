using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Queries
{
    public class GetGxdlmsprofilgenericdetailByMultipleCriteriaQuery : IRequest<ResponseBase<List<GxdlmsprofilgenericdetailResponse>>>
    {
        public List<string> NumeroCompteurs { get; set; }
        public List<int> GxdlmsprofilgenericIds { get; set; }
        public List<int> CodeobisIds { get; set; }
        public DateTime DateEnr { get; set; }
        public Guid UserId { get; set; }
        
        public GetGxdlmsprofilgenericdetailByMultipleCriteriaQuery(){}
        
        public GetGxdlmsprofilgenericdetailByMultipleCriteriaQuery(List<string> numeroCompteurs, List<int> gxdlmsprofilgenericIds, List<int> codeobisIds, DateTime dateEnr)
        {
            this.NumeroCompteurs = numeroCompteurs;
            this.GxdlmsprofilgenericIds = gxdlmsprofilgenericIds;
            this.CodeobisIds = codeobisIds;
            this.DateEnr = dateEnr;
        }

        public GetGxdlmsprofilgenericdetailByMultipleCriteriaQuery(List<string> numeroCompteurs, List<int> gxdlmsprofilgenericIds, List<int> codeobisIds, DateTime dateEnr, Guid userId)
        {
            this.NumeroCompteurs = numeroCompteurs;
            this.GxdlmsprofilgenericIds = gxdlmsprofilgenericIds;
            this.CodeobisIds = codeobisIds;
            this.DateEnr = dateEnr;
            this.UserId = userId;
        }
    }
}
