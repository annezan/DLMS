using DLMS_BUSINESS.FabricantDomainBusiness.Mappers;
using DLMS_DAL.FabricantDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Queries;
using DLMS_MODELS.FabricantDomain.Responses;
using MediatR;
using DLMS_DAL.FabricantsDomainDal.Repositories.Queries;

namespace DLMS_BUSINESS.FabricantDomainBusiness.Handlers.QueryHandlers
{
    public class GetFabricantByIdQueryHandler : IRequestHandler<GetFabricantByIdQuery, ResponseBase<FabricantResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IFabricantQueryRepository _FabricantQueryRepository;

        public GetFabricantByIdQueryHandler(IMediator mediator, IFabricantQueryRepository FabricantQueryRepository)
        {
            _mediator = mediator;
            _FabricantQueryRepository = FabricantQueryRepository;
        }

        public async Task<ResponseBase<FabricantResponse>> Handle(GetFabricantByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<FabricantResponse> responseBase = new ResponseBase<FabricantResponse>();

            try
            {
                var Fabricant = await _FabricantQueryRepository.GetFabricantById(request.Id);
                if (Fabricant == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Poste Introuvable ";
                    return responseBase;
                }
                responseBase.Data = FabricantMapper.Mapper.Map<FabricantResponse>(Fabricant);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Poste Introuvable ";
                return responseBase;
            }
        }
    }

}
