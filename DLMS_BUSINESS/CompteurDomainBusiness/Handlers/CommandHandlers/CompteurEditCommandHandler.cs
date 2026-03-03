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
    public class CompteurEditCommandHandler : IRequestHandler<CompteurEditCommand, ResponseBase<CompteurResponse>>
    {
        private readonly ICompteurCommandRepository _CompteurCommandRepository;
        private readonly IMediator _mediator;

        public CompteurEditCommandHandler(ICompteurCommandRepository CompteurCommandRepository, IMediator mediator, ICompteurQueryRepository CompteurQueryRepository)
        {
            _mediator = mediator;
            _CompteurCommandRepository = CompteurCommandRepository;
        }

        public async Task<ResponseBase<CompteurResponse>> Handle(CompteurEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CompteurResponse> responseBase = new ResponseBase<CompteurResponse>();            
          
            try
            {
                // Appliquer les modifications
                var CompteurEntity = CompteurMapper.Mapper.Map<Compteur>(request);
                
                var editCompteur = await _CompteurCommandRepository.EditCompteur(CompteurEntity);

                if (editCompteur == null || CompteurEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = CompteurMapper.Mapper.Map<CompteurResponse>(editCompteur);

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
