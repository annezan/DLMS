using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.CelluleDomain.Responses;

namespace DLMS_MODELS.CelluleDomain.Queries
{
    public class GetCelluleByPosteIdQuery : IRequest<ResponseBase<List<CelluleResponse>>>
    {
        public int PosteId { get; set; }
        public GetCelluleByPosteIdQuery() { }
        public GetCelluleByPosteIdQuery(int posteId)
        {
            this.PosteId = posteId;
        }
    }
}

