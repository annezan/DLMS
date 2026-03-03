using DLMS_BUSINESS.ErrorDomainBusiness.Mappers;
using DLMS_DAL.ErrorDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.ErrorDomain.Queries;
using DLMS_MODELS.ErrorDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_BUSINESS.ErrorDomainBusiness.Mappers;
using DLMS_MODELS.ErrorDomain.Responses;

namespace DLMS_BUSINESS.ErrorDomainBusiness.Handlers.QueryHandlers
{
    public class GetErrorByValueQueryHandler : IRequestHandler<GetErrorByValueQuery, ResponseBase<ErrorResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IErrorQueryRepository _ErrorQueryRepository;

        public GetErrorByValueQueryHandler(IMediator mediator, IErrorQueryRepository ErrorQueryRepository)
        {
            _mediator = mediator;
            _ErrorQueryRepository = ErrorQueryRepository;
        }

        public async Task<ResponseBase<ErrorResponse>> Handle(GetErrorByValueQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<ErrorResponse> responseBase = new ResponseBase<ErrorResponse>();

            try
            {
                var Error = await _ErrorQueryRepository.GetErrorByValue(request.Value);
                responseBase.Data = ErrorMapper.Mapper.Map<ErrorResponse>(Error);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Error Introuvable ou désactivé";
                return responseBase;
            }

        }
    }

}
