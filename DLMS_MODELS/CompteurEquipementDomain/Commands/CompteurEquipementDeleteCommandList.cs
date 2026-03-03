using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using DLMS_MODELS.EquipementDomain.Entities;
using MediatR;

namespace DLMS_MODELS.CompteurEquipementDomain.Commands
{
    public class CompteurEquipementDeleteCommandList : IRequest<ResponseBase<List<CompteurEquipementResponse>>>
    {
        public List<CompteurEquipementDeleteCommand> Commands { get; set; }

        public CompteurEquipementDeleteCommandList(List<CompteurEquipementDeleteCommand> commands)
        {
            Commands = commands ?? new List<CompteurEquipementDeleteCommand>();
        }
    }
}
