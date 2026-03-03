using DLMS_BUSINESS.AssociationKeyDomainBusiness.Mappers;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Commands;
using DLMS_DAL.AssociationKeyDomainDal.Repositories.Queries;
using DLMS_MODELS.AssociationKeyDomain.Commands;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.AssociationKeyDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_BUSINESS.AssociationKeyDomainBusiness.Handlers.CommandHandlers
{
    public class AssociationKeyEditCommandHandler : IRequestHandler<AssociationKeyEditCommand, ResponseBase<AssociationKeyResponse>>
    {
        private readonly IAssociationKeyCommandRepository _associationKeyCommandRepository;
        private readonly IMediator _mediator;

        public AssociationKeyEditCommandHandler(IAssociationKeyCommandRepository associationKeyCommandRepository, IMediator mediator, IAssociationKeyQueryRepository AssociationKeyQueryRepository)
        {
            _mediator = mediator;
            _associationKeyCommandRepository = associationKeyCommandRepository;
        }

        public async Task<ResponseBase<AssociationKeyResponse>> Handle(AssociationKeyEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<AssociationKeyResponse> responseBase = new ResponseBase<AssociationKeyResponse>();

            

            // Appliquer les modifications
            var AssociationKeyEntity = AssociationKeyMapper.Mapper.Map<AssociationKey>(request);

            if (AssociationKeyEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            try
            {
                var newAssociationKey = await _associationKeyCommandRepository.EditAssociationKey(AssociationKeyEntity);

                responseBase.Data = AssociationKeyMapper.Mapper.Map<AssociationKeyResponse>(newAssociationKey);

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
