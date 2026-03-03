using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Responses;
using MediatR;

namespace DLMS_MODELS.PosteDomain.Commands
{
    public class PosteEditCommand : IRequest<ResponseBase<PosteResponse>>
    {
        public int Id { get; set; }

        public string? Numero { get; set; }
        public string? Libelle { get; set; }
        public string Adresse { get; set; }
        public string UpdatedBy { get; set; }
        public PosteEditCommand()
        {
        }
    }
}
