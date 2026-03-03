using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_MODELS.ReadObjectCommandeDomain.Queries
{
    public class GetReadObjectCommandeQuery : IRequest<ResponseBase<string>>
    {
        public int CommandeId { get; set; }

        public GetReadObjectCommandeQuery() { }

        public GetReadObjectCommandeQuery(int commandeId)
        {
            CommandeId = commandeId;
        }
    }
}
