using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class PermissionAddCommand : IRequest<ResponseBase<PermissionResponse>>
    {
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        
        private DateTime CreatedAt { get; set; }
        public PermissionAddCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
