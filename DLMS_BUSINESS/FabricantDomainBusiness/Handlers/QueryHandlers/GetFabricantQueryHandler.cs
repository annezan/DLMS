using DLMS_BUSINESS.FabricantDomainBusiness.Mappers;
using DLMS_DAL.FabricantDomainDal.Repositories.Queries;
using DLMS_DAL.FabricantsDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Queries;
using DLMS_MODELS.FabricantDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.FabricantDomainBusiness.Handlers.QueryHandlers
{
    public class GetFabricantQueryHandler : IRequestHandler<GetFabricantQuery, ResponseBase<List<FabricantResponse>>>
    {
        private readonly IFabricantQueryRepository _FabricantQueryRepository;

        public GetFabricantQueryHandler(IFabricantQueryRepository FabricantQueryRepository)
        {
            _FabricantQueryRepository = FabricantQueryRepository;
        }

        public async Task<ResponseBase<List<FabricantResponse>>> Handle(GetFabricantQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<FabricantResponse>> responseBase = new ResponseBase<List<FabricantResponse>>();
            try
            {
                var Fabricant = await _FabricantQueryRepository.GetFabricant();
                responseBase.Data = FabricantMapper.Mapper.Map<List<FabricantResponse>>(Fabricant);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Something went wrong! " + ex.Message;
                return responseBase;
            }
        }
    }
}
