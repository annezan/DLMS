using DLMS_BUSINESS.CelluleDomainBusiness.Mappers;
using DLMS_DAL.CelluleDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Queries;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CelluleDomainBusiness.Handlers.QueryHandlers
{
    public class GetCelluleQueryHandler : IRequestHandler<GetCelluleQuery, ResponseBase<List<CelluleResponse>>>
    {
        private readonly ICelluleQueryRepository _celluleQueryRepository;

        public GetCelluleQueryHandler(ICelluleQueryRepository celluleQueryRepository)
        {
            _celluleQueryRepository = celluleQueryRepository;
        }

        public async Task<ResponseBase<List<CelluleResponse>>> Handle(GetCelluleQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<CelluleResponse>> responseBase = new ResponseBase<List<CelluleResponse>>();
            try
            {
                var cellule = await _celluleQueryRepository.GetCellule();
                responseBase.Data = CelluleMapper.Mapper.Map<List<CelluleResponse>>(cellule);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Cellule Introuvable ";
                return responseBase;
            }
        }
    }
}

