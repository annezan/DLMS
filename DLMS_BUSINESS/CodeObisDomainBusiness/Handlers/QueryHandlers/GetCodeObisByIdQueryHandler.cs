using DLMS_BUSINESS.CodeObisDomainBusiness.Mappers;
using DLMS_DAL.CodeObisDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CodeObisDomain.Queries;
using DLMS_MODELS.CodeObisDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_BUSINESS.CodeObisDomainBusiness.Mappers;
using DLMS_MODELS.CodeObisDomain.Responses;

namespace DLMS_BUSINESS.CodeObisDomainBusiness.Handlers.QueryHandlers
{
    public class GetCodeObisByIdQueryHandler : IRequestHandler<GetCodeObisByIdQuery, ResponseBase<CodeObisResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ICodeObisQueryRepository _CodeObisQueryRepository;

        public GetCodeObisByIdQueryHandler(IMediator mediator, ICodeObisQueryRepository CodeObisQueryRepository)
        {
            _mediator = mediator;
            _CodeObisQueryRepository = CodeObisQueryRepository;
        }

        public async Task<ResponseBase<CodeObisResponse>> Handle(GetCodeObisByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<CodeObisResponse> responseBase = new ResponseBase<CodeObisResponse>();
            try
            {
                var CodeObis = await _CodeObisQueryRepository.GetCodeObisById(request.Id);
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
