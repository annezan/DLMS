using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Commands
{
    public class GxdlmsprofilgenericdetailseventDeleteCommand : IRequest<ResponseBase<GxdlmsprofilgenericdetailseventResponse>>
    {
        public int Id { get; set; }

        public DateTime? DeletedAt { get; set; }
        public GxdlmsprofilgenericdetailseventDeleteCommand()
        {
            
        }
    }
}
