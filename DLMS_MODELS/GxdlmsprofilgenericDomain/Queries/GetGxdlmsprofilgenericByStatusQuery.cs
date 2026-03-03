using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Queries
{
    public class GetGxdlmsprofilgenericByStatusQuery : IRequest<ResponseBase<List<GxdlmsprofilgenericResponse>>>
    {
        public bool IsArchive { get; set; }
        public GetGxdlmsprofilgenericByStatusQuery(){}
        public GetGxdlmsprofilgenericByStatusQuery(bool IsArchive)
        {
            this.IsArchive = IsArchive;
        }
    }
}
