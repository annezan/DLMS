using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;

namespace DLMS_MODELS.EquipementDomain.Commands
{
    public class EquipementDeleteCommand : IRequest<ResponseBase<EquipementResponse>>
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; }
        public EquipementDeleteCommand()
        {
        }
    }
}
