using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Commands
{
    public class GxdlmsprofilgenericdetailAddCommand : IRequest<ResponseBase<bool>>
    {
        public int Id { get; set; }

        public int Codeobisid { get; set; }

        public string Value { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public int? GxdlmsprofilgenericId { get; set; }
        public int IdCompteur { get; set; }
        public string Codeobislibelle { get; set; } = null!;
        public string Profilgenericlibelle { get; set; } = null!;
        public string Codeobisvalue { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }

        public GxdlmsprofilgenericdetailAddCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
