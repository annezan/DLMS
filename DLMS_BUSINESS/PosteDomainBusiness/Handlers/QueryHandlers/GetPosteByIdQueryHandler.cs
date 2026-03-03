using DLMS_BUSINESS.PosteDomainBusiness.Mappers;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Queries;
using DLMS_MODELS.PosteDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_BUSINESS.PosteDomainBusiness.Mappers;
using DLMS_MODELS.PosteDomain.Responses;

namespace DLMS_BUSINESS.PosteDomainBusiness.Handlers.QueryHandlers
{
    public class GetPosteByTypeQueryHandler : IRequestHandler<GetPosteByIdQuery, ResponseBase<PosteResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IPosteQueryRepository _PosteQueryRepository;

        public GetPosteByTypeQueryHandler(IMediator mediator, IPosteQueryRepository PosteQueryRepository)
        {
            _mediator = mediator;
            _PosteQueryRepository = PosteQueryRepository;
        }

        public async Task<ResponseBase<PosteResponse>> Handle(GetPosteByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<PosteResponse> responseBase = new ResponseBase<PosteResponse>();

            try
            {
                var Poste = await _PosteQueryRepository.GetPosteById(request.IdPoste);
                if (Poste == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = $"Poste avec ID {request.IdPoste} introuvable";
                    return responseBase;
                }
                
                responseBase.Data = PosteMapper.Mapper.Map<PosteResponse>(Poste);
                responseBase.IsSuccess = true;
                responseBase.Message = "Poste récupéré avec succès";
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération du poste: {ex.Message}";
                return responseBase;
            }
        }
    }

}
