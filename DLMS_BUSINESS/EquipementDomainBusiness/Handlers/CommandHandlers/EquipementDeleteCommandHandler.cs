using DLMS_BUSINESS.EquipementDomainBusiness.Mappers;
using DLMS_DAL.EquipementDomainDal.Repositories.Commands;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_MODELS.EquipementDomain.Commands;
using DLMS_MODELS.EquipementDomain.Entities;
using DLMS_MODELS.EquipementDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;

namespace DLMS_BUSINESS.EquipementDomainBusiness.Handlers.CommandHandlers
{
    public class EquipementDeleteCommandHandler : IRequestHandler<EquipementDeleteCommand, ResponseBase<EquipementResponse>>
    {
        private readonly IEquipementCommandRepository _EquipementCommandRepository;
        private readonly IMediator _mediator;

        public EquipementDeleteCommandHandler(IEquipementCommandRepository EquipementCommandRepository, IMediator mediator, IEquipementQueryRepository EquipementQueryRepository)
        {
            _mediator = mediator;
            _EquipementCommandRepository = EquipementCommandRepository;
        }

        public async Task<ResponseBase<EquipementResponse>> Handle(EquipementDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<EquipementResponse> responseBase = new ResponseBase<EquipementResponse>();

            

            // Appliquer les modifications
            try
            {
                var EquipementEntity = EquipementMapper.Mapper.Map<Equipement>(request);

                var deleteEquipement = await _EquipementCommandRepository.DeleteEquipement(EquipementEntity);
                if (deleteEquipement == null || EquipementEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu ";
                    return responseBase;
                }

                responseBase.Data = EquipementMapper.Mapper.Map<EquipementResponse>(deleteEquipement);

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
