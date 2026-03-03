using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeDomain.Queries
{
    public class GetCommandeByIdQuery : IRequest<ResponseBase<CommandeByIdResponse>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public GetCommandeByIdQuery(){ }

        public GetCommandeByIdQuery(int id)
        {
            this.Id = id;
        }

        public GetCommandeByIdQuery(int id, Guid userId)
        {
            this.Id = id;
            this.UserId = userId;
        }
    }
}
