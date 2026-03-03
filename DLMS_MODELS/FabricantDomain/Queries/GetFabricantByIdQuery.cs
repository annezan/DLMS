using DLMS_MODELS.Bases;
using MediatR;
using DLMS_MODELS.FabricantDomain.Responses;

namespace DLMS_MODELS.FabricantDomain.Queries
{
    public class GetFabricantByIdQuery : IRequest<ResponseBase<FabricantResponse>>
    {
        public int Id { get; set; }
        public GetFabricantByIdQuery() { }
        public GetFabricantByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
