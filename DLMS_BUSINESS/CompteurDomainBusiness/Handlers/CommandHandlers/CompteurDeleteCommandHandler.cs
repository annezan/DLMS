using DLMS_BUSINESS.CompteurDomainBusiness.Mappers;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;
using DLMS_MODELS.CompteurDomain.Commands;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;
using DLMS_DAL.CompteurDomainDal.Repositories.Queries;

namespace DLMS_BUSINESS.CompteurDomainBusiness.Handlers.CommandHandlers
{
    public class CompteurDeleteCommandHandler : IRequestHandler<CompteurDeleteCommand, ResponseBase<CompteurResponse>>
    {
        private readonly ICompteurCommandRepository _CompteurCommandRepository;
        private readonly IMediator _mediator;

        public CompteurDeleteCommandHandler(ICompteurCommandRepository CompteurCommandRepository, IMediator mediator, ICompteurQueryRepository CompteurQueryRepository)
        {
            _mediator = mediator;
            _CompteurCommandRepository = CompteurCommandRepository;
        }

        public async Task<ResponseBase<CompteurResponse>> Handle(CompteurDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CompteurResponse> responseBase = new ResponseBase<CompteurResponse>();            
          
            try
            {
                // Appliquer les modifications
                var CompteurEntity = CompteurMapper.Mapper.Map<Compteur>(request);
                
                var DeleteCompteur = await _CompteurCommandRepository.DeleteCompteur(CompteurEntity);

                if (DeleteCompteur == null || CompteurEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = CompteurMapper.Mapper.Map<CompteurResponse>(DeleteCompteur);

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
