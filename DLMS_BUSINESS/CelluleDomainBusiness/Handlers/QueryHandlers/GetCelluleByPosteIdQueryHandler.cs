using DLMS_BUSINESS.CelluleDomainBusiness.Mappers;
using DLMS_DAL.CelluleDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Queries;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CelluleDomainBusiness.Handlers.QueryHandlers
{
    public class GetCelluleByPosteIdQueryHandler : IRequestHandler<GetCelluleByPosteIdQuery, ResponseBase<List<CelluleResponse>>>
    {
        private readonly IMediator _mediator;
        private readonly ICelluleQueryRepository _celluleQueryRepository;

        public GetCelluleByPosteIdQueryHandler(IMediator mediator, ICelluleQueryRepository celluleQueryRepository)
        {
            _mediator = mediator;
            _celluleQueryRepository = celluleQueryRepository;
        }

        public async Task<ResponseBase<List<CelluleResponse>>> Handle(GetCelluleByPosteIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CelluleResponse>> responseBase = new ResponseBase<List<CelluleResponse>>();

            try
            {
                var cellules = await _celluleQueryRepository.GetCelluleByPosteId(request.PosteId);
                if (cellules == null || !cellules.Any())
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Aucune cellule trouvée pour ce poste";
                    return responseBase;
                }
                responseBase.Data = CelluleMapper.Mapper.Map<List<CelluleResponse>>(cellules);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Une erreur est survenue lors de la récupération des cellules";
                return responseBase;
            }

        }
    }

}

