using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;

namespace DLMS_MODELS.GxdlmsprofilgenericDomain.Queries
{
    public class GetGxdlmsprofilgenericByLNQuery : IRequest<ResponseBase<GxdlmsprofilgenericResponse>>
    {
        public string LN { get; private set; }

        public GetGxdlmsprofilgenericByLNQuery(string ln)
        {
            this.LN = ln;
        }
    }
}
