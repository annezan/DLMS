using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Responses;
using MediatR;

namespace DLMS_MODELS.PosteDomain.Commands
{
    public class PosteDeleteCommand : IRequest<ResponseBase<PosteResponse>>
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; }
        public PosteDeleteCommand()
        {
        }
    }
}
