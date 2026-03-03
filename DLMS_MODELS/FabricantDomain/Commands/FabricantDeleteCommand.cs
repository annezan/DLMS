using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Responses;
using MediatR;

namespace DLMS_MODELS.FabricantDomain.Commands
{
    public class FabricantDeleteCommand : IRequest<ResponseBase<FabricantResponse>>
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; }
        public FabricantDeleteCommand()
        {
            
        }
    }
}
