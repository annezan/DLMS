using DLMS_BUSINESS.ErrorDomainBusiness.Mappers;
using DLMS_DAL.ErrorDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.ErrorDomain.Queries;
using DLMS_MODELS.ErrorDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.ErrorDomainBusiness.Handlers.QueryHandlers
{
    public class GetErrorQueryHandler : IRequestHandler<GetErrorQuery, ResponseBase<List<ErrorResponse>>>
    {
        private readonly IErrorQueryRepository _ErrorQueryRepository;

        public GetErrorQueryHandler(IErrorQueryRepository ErrorQueryRepository)
        {
            _ErrorQueryRepository = ErrorQueryRepository;
        }

        public async Task<ResponseBase<List<ErrorResponse>>> Handle(GetErrorQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<ErrorResponse>> responseBase = new ResponseBase<List<ErrorResponse>>();
            try
            {
                var Error = await _ErrorQueryRepository.GetError();
                responseBase.Data = ErrorMapper.Mapper.Map<List<ErrorResponse>>(Error);
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
