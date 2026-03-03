using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;

namespace DLMS_MODELS.EquipementDomain.Commands
{
    public class EquipementAddCommand : IRequest<ResponseBase<EquipementResponse>>
    {
        public string? NumeroSerie { get; set; }
        public string? Libelle { get; set; }
        public string? AdresseIp { get; set; }
        public string Type { get; set; }
        public string? Marque { get; set; }
        public string? Port { get; set; }
        public string? SerialPort { get; set; }
        public DateTime? DatePremierePose { get; set; }

        public DateTime? DatePoseActuelle { get; set; }
        public string CreatedBy { get; set; }
        public EquipementAddCommand()
        {
        }
    }
}

