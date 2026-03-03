using DLMS_BUSINESS.CelluleDomainBusiness.Mappers;
using DLMS_DAL.CelluleDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Queries;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CelluleDomainBusiness.Handlers.QueryHandlers
{
    public class GetCelluleByIdQueryHandler : IRequestHandler<GetCelluleByIdQuery, ResponseBase<CelluleResponse>>
    {
        private readonly IMediator _mediator;
        private readonly ICelluleQueryRepository _celluleQueryRepository;

        public GetCelluleByIdQueryHandler(IMediator mediator, ICelluleQueryRepository celluleQueryRepository)
        {
            _mediator = mediator;
            _celluleQueryRepository = celluleQueryRepository;
        }

        public async Task<ResponseBase<CelluleResponse>> Handle(GetCelluleByIdQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<CelluleResponse> responseBase = new ResponseBase<CelluleResponse>();

            try
            {
                var cellule = await _celluleQueryRepository.GetCelluleById(request.IdCellule);
                if (cellule == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Cellule Introuvable";
                    return responseBase;
                }
                responseBase.Data = CelluleMapper.Mapper.Map<CelluleResponse>(cellule);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Cellule Introuvable";
                return responseBase;
            }

        }
    }

}

