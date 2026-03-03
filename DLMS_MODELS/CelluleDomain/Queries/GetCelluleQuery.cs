using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.CelluleDomain.Responses;

namespace DLMS_MODELS.CelluleDomain.Queries
{
    public class GetCelluleQuery : IRequest<ResponseBase<List<CelluleResponse>>>
    {
        public GetCelluleQuery()
        {
        }
    }
}

