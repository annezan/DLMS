using DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Mappers;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;

namespace DLMS_BUSINESS.GxdlmsprofilgenericDomainBusiness.Handlers.QueryHandlers
{
    public class GetGxdlmsprofilgenericByStatusQueryHandler : IRequestHandler<GetGxdlmsprofilgenericByStatusQuery, ResponseBase<List<GxdlmsprofilgenericResponse>>>
    {
        private readonly IMediator _mediator;
        private readonly IGxdlmsprofilgenericQueryRepository _GxdlmsprofilgenericQueryRepository;

        public GetGxdlmsprofilgenericByStatusQueryHandler(IMediator mediator, IGxdlmsprofilgenericQueryRepository GxdlmsprofilgenericQueryRepository)
        {
            _mediator = mediator;
            _GxdlmsprofilgenericQueryRepository = GxdlmsprofilgenericQueryRepository;
        }

        public async Task<ResponseBase<List<GxdlmsprofilgenericResponse>>> Handle(GetGxdlmsprofilgenericByStatusQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<GxdlmsprofilgenericResponse>> responseBase = new ResponseBase<List<GxdlmsprofilgenericResponse>>();
            try
            {
                var Gxdlmsprofilgeneric = await _GxdlmsprofilgenericQueryRepository.GetProfilgenericByStatus(request.IsArchive);

                if (Gxdlmsprofilgeneric == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Gxdlmsprofilgeneric Introuvable ou désactivé";
                    return responseBase;
                }

                responseBase.Data = GxdlmsprofilgenericMapper.Mapper.Map<List<GxdlmsprofilgenericResponse>>(Gxdlmsprofilgeneric);

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Gxdlmsprofilgeneric Introuvable ou désactivé";
                return responseBase;
            }
           
        }
    }

}
