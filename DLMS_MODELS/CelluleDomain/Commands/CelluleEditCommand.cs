using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CelluleDomain.Commands
{
    public class CelluleEditCommand : IRequest<ResponseBase<CelluleResponse>>
    {
        public int Id { get; set; }

        public string? Type { get; set; }
        public int ValeurTension { get; set; }
        public string? Libelle { get; set; }
        public string Adresse { get; set; }
        public int PosteId { get; set; }
        public string UpdatedBy { get; set; }
        public CelluleEditCommand()
        {
        }
    }
}

