using DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Mappers;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;

namespace DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Handlers.QueryHandlers
{
    public class GetGxdlmsprofilgenericByLNQueryHandler : IRequestHandler<GetGxdlmsprofilgenericByLNQuery, ResponseBase<GxdlmsprofilgenericResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IGxdlmsprofilgenericQueryRepository _GxdlmsprofilgenericQueryRepository;

        public GetGxdlmsprofilgenericByLNQueryHandler(IMediator mediator, IGxdlmsprofilgenericQueryRepository GxdlmsprofilgenericQueryRepository)
        {
            _mediator = mediator;
            _GxdlmsprofilgenericQueryRepository = GxdlmsprofilgenericQueryRepository;
        }

        public async Task<ResponseBase<GxdlmsprofilgenericResponse>> Handle(GetGxdlmsprofilgenericByLNQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<GxdlmsprofilgenericResponse> responseBase = new ResponseBase<GxdlmsprofilgenericResponse>();

            var Gxdlmsprofilgeneric = await _GxdlmsprofilgenericQueryRepository.GetProfilgenericByLN(request.LN);

            if (Gxdlmsprofilgeneric == null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Gxdlmsprofilgeneric Introuvable ou désactivé";
                return responseBase;
            }

            responseBase.Data = GxdlmsprofilgenericMapper.Mapper.Map<GxdlmsprofilgenericResponse>(Gxdlmsprofilgeneric); ;

            return responseBase;
        }
    }

}
