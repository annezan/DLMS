using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using DLMS_MODELS.CommandeDomain.Responses;
using DLMS_MODELS.TypecommandeDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CommandeDomain.Responses
{
    public class CommandeByIdResponse : IRequest<ResponseBase<CommandeByIdResponse>>
    {
        public int Id { get; set; }

        public string Libellecommande { get; set; } = null!;

        public DateTime? Dateexec { get; set; }

        public DateTime? Datefin { get; set; }

        public string Statut { get; set; }

        public int Idtype { get; set; }
        public int? Numeroprofile { get; set; }
        public int? Nombreentree { get; set; }
        public DateTime? Datedebut { get; set; }
        public DateTime? Dateexp { get; set; }
        public ICollection<CommandeCompteurResponse>? CommandeCompteur { get; set; }
        public TypecommandeResponse? Typecommande { get; set; } = null!;



    }
}
