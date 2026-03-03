using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class RoleAddCommand : IRequest<ResponseBase<RoleResponse>>
    {
        public string Libelle { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        private DateTime CreatedAt { get; set; }
        public RoleAddCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
