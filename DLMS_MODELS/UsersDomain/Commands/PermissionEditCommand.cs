using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class PermissionEditCommand : IRequest<ResponseBase<PermissionResponse>>
    {
        public Guid Id { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        
        private DateTime UpdatedAt { get; set; }

        public PermissionEditCommand()
        {
            this.UpdatedAt = DateTime.Now;
        }
    }
}
