using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Responses;
using MediatR;

namespace DLMS_MODELS.FabricantDomain.Commands
{
    public class FabricantEditCommand : IRequest<ResponseBase<FabricantResponse>>
    {
        public int Id { get; set; }

        public string Libelle { get; set; }
        public string UpdatedBy { get; set; }
        public FabricantEditCommand()
        {
        }
    }
}
