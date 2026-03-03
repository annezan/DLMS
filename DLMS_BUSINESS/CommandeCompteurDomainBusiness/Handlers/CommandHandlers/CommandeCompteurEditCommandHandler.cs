using DLMS_BUSINESS.CommandeCompteurDomainBusiness.Mappers;
using DLMS_DAL.CommandeCompteurDomainDal.Repositories.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Commands;
using DLMS_MODELS.CommandeCompteurDomain.Entities;
using DLMS_MODELS.CommandeCompteurDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_BUSINESS.CommandeCompteurDomainBusiness.Handlers.CommandHandlers
{
    public class CommandeCompteurEditCommandHandler : IRequestHandler<CommandeCompteurEditCommand, ResponseBase<CommandeCompteurResponse>>
    {
        private readonly ICommandeCompteurCommandRepository _CommandeCompteurCommandRepository;
        private readonly IMediator _mediator;

        public CommandeCompteurEditCommandHandler(ICommandeCompteurCommandRepository CommandeCompteurCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _CommandeCompteurCommandRepository = CommandeCompteurCommandRepository;
        }

        public async Task<ResponseBase<CommandeCompteurResponse>> Handle(CommandeCompteurEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CommandeCompteurResponse> responseBase = new ResponseBase<CommandeCompteurResponse>();

            

            // Appliquer les modifications
            var CommandeCompteurEntity = CommandeCompteurMapper.Mapper.Map<CommandeCompteur>(request);

            if (CommandeCompteurEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            try
            {
                var newCommandeCompteur = await _CommandeCompteurCommandRepository.EditCommandeCompteur(CommandeCompteurEntity);

                responseBase.Data = CommandeCompteurMapper.Mapper.Map<CommandeCompteurResponse>(newCommandeCompteur);

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
