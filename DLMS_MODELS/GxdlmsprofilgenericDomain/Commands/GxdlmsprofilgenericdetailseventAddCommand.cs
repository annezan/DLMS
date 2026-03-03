using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Commands
{
    public class GxdlmsprofilgenericdetailseventAddCommand : IRequest<ResponseBase<bool>>
    {
        public int Id { get; set; }

        public int Codeobisid { get; set; }

        public string Value { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int GxdlmsprofilgenericId { get; set; }
        public int IdCompteur { get; set; }
        public DateTime DateEnr { get; set; }
        public string Codeobislibelle { get; set; } = null!;
        public string Profilgenericlibelle { get; set; } = null!;
        public string Codeobisvalue { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public GxdlmsprofilgenericdetailseventAddCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
