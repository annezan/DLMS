using DLMS_BUSINESS.CelluleDomainBusiness.Mappers;
using DLMS_DAL.CelluleDomainDal.Repositories.Commands;
using DLMS_DAL.CelluleDomainDal.Repositories.Queries;
using DLMS_MODELS.CelluleDomain.Commands;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_BUSINESS.CelluleDomainBusiness.Handlers.CommandHandlers
{
    public class CelluleEditCommandHandler : IRequestHandler<CelluleEditCommand, ResponseBase<CelluleResponse>>
    {
        private readonly ICelluleCommandRepository _celluleCommandRepository;
        private readonly IMediator _mediator;

        public CelluleEditCommandHandler(ICelluleCommandRepository celluleCommandRepository, IMediator mediator, ICelluleQueryRepository celluleQueryRepository)
        {
            _mediator = mediator;
            _celluleCommandRepository = celluleCommandRepository;
        }

        public async Task<ResponseBase<CelluleResponse>> Handle(CelluleEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CelluleResponse> responseBase = new ResponseBase<CelluleResponse>();

            // Appliquer les modifications
            try
            {
                var celluleEntity = CelluleMapper.Mapper.Map<Cellule>(request);

                var editCellule = await _celluleCommandRepository.EditCellule(celluleEntity);
                if (editCellule == null || celluleEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu : " ;
                    return responseBase;
                }

                responseBase.Data = CelluleMapper.Mapper.Map<CelluleResponse>(editCellule);

                return responseBase;
            }
            catch (Exception exp)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu : " + exp.Message;
                return responseBase;
            }

            
        }

    }

}

