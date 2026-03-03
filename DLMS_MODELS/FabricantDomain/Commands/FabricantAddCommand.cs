using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Responses;
using MediatR;

namespace DLMS_MODELS.FabricantDomain.Commands
{
    public class FabricantAddCommand : IRequest<ResponseBase<FabricantResponse>>
    {
        public string Libelle { get; set; }
        public string CreatedBy { get; set; } = String.Empty;

        public FabricantAddCommand()
        {
        }
    }
}
