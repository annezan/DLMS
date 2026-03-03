using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Responses;
using DLMS_MODELS.EquipementDomain.Entities;
using MediatR;

namespace DLMS_MODELS.CompteurEquipementDomain.Commands
{   
    public class CompteurEquipementDeleteCommand : IRequest<ResponseBase<CompteurEquipementResponse>>
    {
        public int CompteurId { get; set; }

        public int EquipementId { get; set; }

        public Guid UserId { get; set; } // Added for access checks

        public string DeletedBy { get; set; }

    }
}
