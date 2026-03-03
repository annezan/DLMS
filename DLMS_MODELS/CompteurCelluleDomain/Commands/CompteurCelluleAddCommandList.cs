using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurCelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurCelluleDomain.Commands
{
    public class CompteurCelluleAddCommandList : IRequest<ResponseBase<List<CompteurCelluleResponse>>>
    {
        public List<CompteurCelluleAddCommand> Commands { get; set; }

        public CompteurCelluleAddCommandList(List<CompteurCelluleAddCommand> commands)
        {
            Commands = commands ?? new List<CompteurCelluleAddCommand>();
        }
    }
}
