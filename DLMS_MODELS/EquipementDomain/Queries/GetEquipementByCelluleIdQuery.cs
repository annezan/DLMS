using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;

namespace DLMS_MODELS.EquipementDomain.Queries
{
    public class GetEquipementByCelluleIdQuery : IRequest<ResponseBase<List<EquipementResponse>>>
    {
        public int CelluleId { get; set; }
        public GetEquipementByCelluleIdQuery() { }
        public GetEquipementByCelluleIdQuery(int celluleId)
        {
            this.CelluleId = celluleId;
        }
    }
}

