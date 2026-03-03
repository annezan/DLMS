using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurDomain.Commands
{
    public class CompteurMAJCommand : IRequest<ResponseBase<bool>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public CompteurMAJCommand() { }
    }
}
