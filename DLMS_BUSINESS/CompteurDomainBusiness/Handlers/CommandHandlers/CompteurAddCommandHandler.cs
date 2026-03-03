using DLMS_BUSINESS.CompteurDomainBusiness.Mappers;
using DLMS_DAL.CompteurDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.CompteurDomain.Commands;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurDomain.Queries;
using DLMS_MODELS.CompteurDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_DAL.CompteurDomainDal.Repositories;

namespace DLMS_BUSINESS.CompteurDomainBusiness.Handlers.CommandHandlers
{
    public class CompteurAddCommandHandler : IRequestHandler<CompteurAddCommand, ResponseBase<CompteurResponse>>
    {
        private readonly ICompteurCommandRepository _CompteurCommandRepository;
        private readonly IMediator _mediator;

        public CompteurAddCommandHandler(ICompteurCommandRepository CompteurCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _CompteurCommandRepository = CompteurCommandRepository;
        }

        public async Task<ResponseBase<CompteurResponse>> Handle(CompteurAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<CompteurResponse> responseBase = new ResponseBase<CompteurResponse>();

            try
            {
                var CompteurEntity = CompteurMapper.Mapper.Map<Compteur>(request);

                //CompteurEntity.CreatedAt = DateTime.Now;
                var newCompteur = await _CompteurCommandRepository.AddCompteur(CompteurEntity);
                if (newCompteur == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }
                responseBase.Data = CompteurMapper.Mapper.Map<CompteurResponse>(newCompteur);

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
