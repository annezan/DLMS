using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CommandeCompteurDomain.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.CommandHandlers
{
    public class CommandeCompteurAddCommandHandler : IRequestHandler<CommandeCompteurAddCommand, ResponseBase<CommandeCompteurResponse>>
    {
        private readonly ICommandeCompteurCommandRepository _CommandeCompteurCommandRepository;
        private readonly IMediator _mediator;

        public CommandeCompteurAddCommandHandler(ICommandeCompteurCommandRepository CommandeCompteurCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _CommandeCompteurCommandRepository = CommandeCompteurCommandRepository;
        }

        public async Task<ResponseBase<CommandeCompteurResponse>> Handle(CommandeCompteurAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CommandeCompteurResponse> responseBase = new ResponseBase<CommandeCompteurResponse>();

            var CommandeCompteurEntity = CommandeCompteurMapper.Mapper.Map<CommandeCompteur>(request);

            if (CommandeCompteurEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            //CommandeCompteurEntity.CreatedAt = DateTime.Now;
            var newCommandeCompteur = await _CommandeCompteurCommandRepository.AddCommandeCompteur(CommandeCompteurEntity);

            responseBase.Data = CommandeCompteurMapper.Mapper.Map<CommandeCompteurResponse>(newCommandeCompteur);

            return responseBase;
        }

    }

}
