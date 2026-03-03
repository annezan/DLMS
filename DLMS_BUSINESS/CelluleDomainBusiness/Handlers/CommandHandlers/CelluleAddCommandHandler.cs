using DLMS_BUSINESS.CelluleDomainBusiness.Mappers;
using DLMS_DAL.CelluleDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CelluleDomain.Commands;
using DLMS_MODELS.CelluleDomain.Entities;
using DLMS_MODELS.CelluleDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.CelluleDomainBusiness.Handlers.CommandHandlers
{
    public class CelluleAddCommandHandler : IRequestHandler<CelluleAddCommand, ResponseBase<CelluleResponse>>
    {
        private readonly ICelluleCommandRepository _celluleCommandRepository;
        private readonly IMediator _mediator;

        public CelluleAddCommandHandler(ICelluleCommandRepository celluleCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _celluleCommandRepository = celluleCommandRepository;
        }

        public async Task<ResponseBase<CelluleResponse>> Handle(CelluleAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CelluleResponse> responseBase = new ResponseBase<CelluleResponse>();
            try
            {
                var celluleEntity = CelluleMapper.Mapper.Map<Cellule>(request);

                var newCellule = await _celluleCommandRepository.AddCellule(celluleEntity);
                if (newCellule == null || celluleEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "La cellule existe déjà.Veuillez renseigner une nouvelle cellule";
                    return responseBase;
                }
                responseBase.Data = CelluleMapper.Mapper.Map<CelluleResponse>(newCellule);

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }
            
        }

    }

}

