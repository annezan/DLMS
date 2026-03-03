using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.CelluleDomain.Responses;

namespace DLMS_MODELS.CelluleDomain.Queries
{
    public class GetCelluleByIdQuery : IRequest<ResponseBase<CelluleResponse>>
    {
        public int IdCellule { get; set; }
        public GetCelluleByIdQuery() { }
        public GetCelluleByIdQuery(int idCellule)
        {
            this.IdCellule = idCellule;
        }
    }
}

