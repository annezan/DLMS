using DLMS_MODELS.Bases;
using DLMS_MODELS.UsersDomain.Responses;
using MediatR;

namespace DLMS_MODELS.UsersDomain.Commands
{
    public class RoleEditCommand : IRequest<ResponseBase<RoleResponse>>
    {
        public Guid Id { get; set; }
        public string Libelle { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        private DateTime UpdatedAt { get; set; }
        public RoleEditCommand()
        {
            this.UpdatedAt = DateTime.Now;
        }
    }
}
