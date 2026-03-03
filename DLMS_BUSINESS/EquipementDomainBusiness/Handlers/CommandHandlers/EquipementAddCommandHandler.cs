using DLMS_BUSINESS.EquipementDomainBusiness.Mappers;
using DLMS_DAL.EquipementDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Commands;
using DLMS_MODELS.EquipementDomain.Entities;
using DLMS_MODELS.EquipementDomain.Queries;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;
using DLMS_DAL.EquipementDomainDal.Repositories;

namespace DLMS_BUSINESS.EquipementDomainBusiness.Handlers.CommandHandlers
{
    public class EquipementAddCommandHandler : IRequestHandler<EquipementAddCommand, ResponseBase<EquipementResponse>>
    {
        private readonly IEquipementCommandRepository _EquipementCommandRepository;
        private readonly IMediator _mediator;

        public EquipementAddCommandHandler(IEquipementCommandRepository EquipementCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _EquipementCommandRepository = EquipementCommandRepository;
        }

        public async Task<ResponseBase<EquipementResponse>> Handle(EquipementAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<EquipementResponse> responseBase = new ResponseBase<EquipementResponse>();
            try
            {
                var EquipementEntity = EquipementMapper.Mapper.Map<Equipement>(request);

                //EquipementEntity.CreatedAt = DateTime.Now;
                var newEquipement = await _EquipementCommandRepository.AddEquipement(EquipementEntity);
                if (newEquipement == null || EquipementEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                
                // Recharger l'équipement avec ses associations
                var equipementWithAssociations = await _mediator.Send(new GetEquipementByIdQuery(newEquipement.Id));
                responseBase.Data = EquipementMapper.Mapper.Map<EquipementResponse>(equipementWithAssociations.Data);

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
