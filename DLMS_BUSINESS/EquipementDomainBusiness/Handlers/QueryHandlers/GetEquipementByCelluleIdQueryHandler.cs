using DLMS_BUSINESS.EquipementDomainBusiness.Mappers;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Queries;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.EquipementDomainBusiness.Handlers.QueryHandlers
{
    public class GetEquipementByCelluleIdQueryHandler : IRequestHandler<GetEquipementByCelluleIdQuery, ResponseBase<List<EquipementResponse>>>
    {
        private readonly IMediator _mediator;
        private readonly IEquipementQueryRepository _equipementQueryRepository;

        public GetEquipementByCelluleIdQueryHandler(IMediator mediator, IEquipementQueryRepository equipementQueryRepository)
        {
            _mediator = mediator;
            _equipementQueryRepository = equipementQueryRepository;
        }

        public async Task<ResponseBase<List<EquipementResponse>>> Handle(GetEquipementByCelluleIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<EquipementResponse>> responseBase = new ResponseBase<List<EquipementResponse>>();

            try
            {
                var equipements = await _equipementQueryRepository.GetEquipementByCelluleId(request.CelluleId);
                if (equipements == null || !equipements.Any())
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Aucun équipement trouvé pour cette cellule";
                    return responseBase;
                }
                responseBase.Data = EquipementMapper.Mapper.Map<List<EquipementResponse>>(equipements);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Une erreur est survenue lors de la récupération des équipements";
                return responseBase;
            }
        }
    }
}

