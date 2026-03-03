using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Responses;
using MediatR;

namespace DLMS_MODELS.PosteDomain.Commands
{
    public class PosteAddCommand : IRequest<ResponseBase<PosteResponse>>
    {
        public string? Numero { get; set; }
        public string? Libelle { get; set; }
        public string Adresse { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
        public PosteAddCommand()
        {
        }
    }
}
