using DLMS_MODELS.Bases;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using DLMS_MODELS.EquipementDomain.Entities;
using MediatR;

namespace DLMS_MODELS.AssociationKeyDomain.Commands
{
    public class AssociationKeyAddCommandList : IRequest<ResponseBase<List<AssociationKeyResponse>>>
    {
        public List<AssociationKeyAddCommand> Commands { get; set; }

        public AssociationKeyAddCommandList(List<AssociationKeyAddCommand> commands)
        {
            Commands = commands ?? new List<AssociationKeyAddCommand>();
        }
    }
}
