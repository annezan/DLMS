using DLMS_BUSINESS.TypecommandeDomainBusiness.Mappers;
using DLMS_DAL.TypecommandeDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.TypecommandeDomain.Queries;
using DLMS_MODELS.TypecommandeDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.TypecommandeDomainBusiness.Handlers.QueryHandlers
{
    public class GetTypecommandeQueryHandler : IRequestHandler<GetTypecommandeQuery, ResponseBase<List<TypecommandeResponse>>>
    {
        private readonly ITypecommandeQueryRepository _TypecommandeQueryRepository;

        public GetTypecommandeQueryHandler(ITypecommandeQueryRepository TypecommandeQueryRepository)
        {
            _TypecommandeQueryRepository = TypecommandeQueryRepository;
        }

        public async Task<ResponseBase<List<TypecommandeResponse>>> Handle(GetTypecommandeQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<TypecommandeResponse>> responseBase = new ResponseBase<List<TypecommandeResponse>>();
            try
            {
                var Typecommande = await _TypecommandeQueryRepository.GetTypecommande();
                responseBase.Data = TypecommandeMapper.Mapper.Map<List<TypecommandeResponse>>(Typecommande);
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
