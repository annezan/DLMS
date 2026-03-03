using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeDomain.Queries
{
    public class GetCommandesQuery : IRequest<ResponseBase<List<CommandeResponse>>>
    {
        public Guid UserId { get; set; }

        public GetCommandesQuery()
        {
        }

        public GetCommandesQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
