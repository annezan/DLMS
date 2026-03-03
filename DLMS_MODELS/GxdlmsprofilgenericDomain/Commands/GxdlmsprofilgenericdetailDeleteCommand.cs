using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Commands
{
    public class GxdlmsprofilgenericdetailDeleteCommand : IRequest<ResponseBase<GxdlmsprofilgenericdetailResponse>>
    {
        public int Id { get; set; }

        public DateTime? DeletedAt { get; set; }       
        public GxdlmsprofilgenericdetailDeleteCommand()
        {
            
        }
    }
}
