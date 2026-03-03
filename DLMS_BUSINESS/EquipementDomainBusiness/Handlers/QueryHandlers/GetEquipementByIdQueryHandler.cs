using DLMS_BUSINESS.EquipementDomainBusiness.Mappers;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Queries;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;

namespace DLMS_BUSINESS.EquipementDomainBusiness.Handlers.QueryHandlers
{
    public class GetEquipementByTypeQueryHandler : IRequestHandler<GetEquipementByIdQuery, ResponseBase<EquipementResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IEquipementQueryRepository _EquipementQueryRepository;

        public GetEquipementByTypeQueryHandler(IMediator mediator, IEquipementQueryRepository EquipementQueryRepository)
        {
            _mediator = mediator;
            _EquipementQueryRepository = EquipementQueryRepository;
        }

        public async Task<ResponseBase<EquipementResponse>> Handle(GetEquipementByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<EquipementResponse> responseBase = new ResponseBase<EquipementResponse>();

            try
            {
                var Equipement = await _EquipementQueryRepository.GetEquipementById(request.Id);
                if (Equipement==null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Equipement introuvable";
                    return responseBase;
                }
                responseBase.Data = EquipementMapper.Mapper.Map<EquipementResponse>(Equipement);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Equipement introuvable";
                return responseBase;
            }

        }
    }

}
