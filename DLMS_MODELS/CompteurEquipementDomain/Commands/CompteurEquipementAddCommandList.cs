using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using DLMS_MODELS.EquipementDomain.Entities;
using MediatR;

namespace DLMS_MODELS.CompteurEquipementDomain.Commands
{
    public class CompteurEquipementAddCommandList : IRequest<ResponseBase<List<CompteurEquipementResponse>>>
    {
        public List<CompteurEquipementAddCommand> Commands { get; set; }

        public CompteurEquipementAddCommandList(List<CompteurEquipementAddCommand> commands)
        {
            Commands = commands ?? new List<CompteurEquipementAddCommand>();
        }
    }
}
