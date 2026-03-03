using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeDomain.Commands
{
    public class CommandeEditCommand : IRequest<ResponseBase<CommandeResponse>>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public string Libellecommande { get; set; } = null!;

        public DateTime? Dateexec { get; set; }

        public DateTime? Datefin { get; set; }
        public int? Numeroprofile { get; set; }
        public int? Nombreentree { get; set; }
        public DateTime? Datedebut { get; set; }
        public DateTime? Dateexp { get; set; }
        public int TypecommandeId { get; set; }
        public List<int> CompteurId { get; set; }
        public string UpdatedBy { get; set; }

        public CommandeEditCommand()
        {
        }
    }
}
