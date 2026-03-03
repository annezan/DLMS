using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;
using DLMS_MODELS.CommandeCompteurDomain.Responses;

namespace DLMS_MODELS.CommandeCompteurDomain.Commands
{
    public class CommandeCompteurAddCommand : IRequest<ResponseBase<CommandeCompteurResponse>>
    {
        public string Libellecommande { get; set; } = null!;

        public DateTime? Dateexec { get; set; }

        public DateTime? Datefin { get; set; }

        public string Statut { get; set; }

        public int Idtype { get; set; }
        public int? Numeroprofile { get; set; }
        public int? Nombreentree { get; set; }
        public DateTime? Datedebut { get; set; }
        public DateTime? Dateexp { get; set; }
        private DateTime CreatedAt { get; set; }
        public CommandeCompteurAddCommand()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
