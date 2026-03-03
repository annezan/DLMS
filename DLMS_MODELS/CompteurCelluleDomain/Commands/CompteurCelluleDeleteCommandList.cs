using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurCelluleDomain.Commands
{
    public class CompteurCelluleDeleteCommandList : IRequest<ResponseBase<List<CompteurCelluleResponse>>>
    {
        public List<CompteurCelluleDeleteCommand> Commands { get; set; }

        public CompteurCelluleDeleteCommandList(List<CompteurCelluleDeleteCommand> commands)
        {
            Commands = commands ?? new List<CompteurCelluleDeleteCommand>();
        }
    }
}
