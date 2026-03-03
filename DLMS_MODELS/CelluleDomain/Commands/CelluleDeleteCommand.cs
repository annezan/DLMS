using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CelluleDomain.Commands
{
    public class CelluleDeleteCommand : IRequest<ResponseBase<CelluleResponse>>
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; }
        public CelluleDeleteCommand()
        {
        }
    }
}

