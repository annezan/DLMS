using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_MODELS.CelluleDomain.Commands
{
    public class CelluleAddCommand : IRequest<ResponseBase<CelluleResponse>>
    {
        public string? Type { get; set; }
        public int ValeurTension { get; set; }
        public string? Libelle { get; set; }
        public string Adresse { get; set; }
        public int PosteId { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
        public CelluleAddCommand()
        {
        }
    }
}

