using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CompteurDomain.Commands
{
    public class CompteurEditCommand : IRequest<ResponseBase<CompteurResponse>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public string IdCompteur { get; set; }
        public string NumeroCompteur { get; set; } = null!;
        public string? MarqueCompteur { get; set; }

        public DateTime? DatePremierePose { get; set; }

        public DateTime? DatePoseActuelle { get; set; }

        public string? Typecompteur { get; set; }

        public int FabriquantId { get; set; }
        public int? Idposte { get; set; }

        public string? Etatcontacteur { get; set; }
        public string UpdatedBy { get; set; }

        public CompteurEditCommand()
        {
        }
    }
}
