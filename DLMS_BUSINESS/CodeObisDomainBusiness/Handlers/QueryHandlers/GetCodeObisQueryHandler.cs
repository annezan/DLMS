using DLMS_BUSINESS.CodeObisDomainBusiness.Mappers;
using DLMS_DAL.CodeObisDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Queries;
using DLMS_MODELS.CodeObisDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CodeObisDomainBusiness.Handlers.QueryHandlers
{
    public class GetCodeObisQueryHandler : IRequestHandler<GetCodeObisQuery, ResponseBase<List<CodeObisResponse>>>
    {
        private readonly ICodeObisQueryRepository _CodeObisQueryRepository;

        public GetCodeObisQueryHandler(ICodeObisQueryRepository CodeObisQueryRepository)
        {
            _CodeObisQueryRepository = CodeObisQueryRepository;
        }

        public async Task<ResponseBase<List<CodeObisResponse>>> Handle(GetCodeObisQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CodeObisResponse>> responseBase = new ResponseBase<List<CodeObisResponse>>();
            try
            {
                var CodeObis = await _CodeObisQueryRepository.GetCodeObis();
                responseBase.Data = CodeObisMapper.Mapper.Map<List<CodeObisResponse>>(CodeObis);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "CodeObis Introuvable ou désactivé " ;
                return responseBase;
            }
        }
    }
}
