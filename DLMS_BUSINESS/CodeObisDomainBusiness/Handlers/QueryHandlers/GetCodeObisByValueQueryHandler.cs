using DLMS_BUSINESS.CodeObisDomainBusiness.Mappers;
using DLMS_DAL.CodeObisDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Queries;
using DLMS_MODELS.CodeObisDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;

namespace DLMS_BUSINESS.CodeObisDomainBusiness.Handlers.QueryHandlers
{
    public class GetCodeObisByValueQueryHandler : IRequestHandler<GetCodeObisByValueQuery, ResponseBase<CodeObisResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ICodeObisQueryRepository _CodeObisQueryRepository;

        public GetCodeObisByValueQueryHandler(IMediator mediator, ICodeObisQueryRepository CodeObisQueryRepository)
        {
            _mediator = mediator;
            _CodeObisQueryRepository = CodeObisQueryRepository;
        }

        public async Task<ResponseBase<CodeObisResponse>> Handle(GetCodeObisByValueQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<CodeObisResponse> responseBase = new ResponseBase<CodeObisResponse>();

            try
            {
                var CodeObis = await _CodeObisQueryRepository.GetCodeObisByValue(request.Value);
                responseBase.Data = CodeObisMapper.Mapper.Map<CodeObisResponse>(CodeObis);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "CodeObis Introuvable ou désactivé";
                return responseBase;
            }

        }
    }

}
