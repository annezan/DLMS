using DLMS_BUSINESS.FabricantDomainBusiness.Mappers;
using DLMS_DAL.FabricantDomainDal.Repositories.Commands;
using DLMS_DAL.FabricantDomainDal.Repositories.Queries;
using DLMS_MODELS.FabricantDomain.Commands;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_MODELS.FabricantDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;
using DLMS_DAL.FabricantsDomainDal.Repositories.Queries;

namespace DLMS_BUSINESS.FabricantDomainBusiness.Handlers.CommandHandlers
{
    public class FabricantDeleteCommandHandler : IRequestHandler<FabricantDeleteCommand, ResponseBase<FabricantResponse>>
    {
        private readonly IFabricantCommandRepository _FabricantCommandRepository;
        private readonly IMediator _mediator;

        public FabricantDeleteCommandHandler(IFabricantCommandRepository FabricantCommandRepository, IMediator mediator, IFabricantQueryRepository FabricantQueryRepository)
        {
            _mediator = mediator;
            _FabricantCommandRepository = FabricantCommandRepository;
        }

        public async Task<ResponseBase<FabricantResponse>> Handle(FabricantDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<FabricantResponse> responseBase = new ResponseBase<FabricantResponse>();
            try
            {
                // Appliquer les modifications
                var FabricantEntity = FabricantMapper.Mapper.Map<Fabricant>(request);
                var deleteFabricant = await _FabricantCommandRepository.DeleteFabricant(FabricantEntity);

                if (deleteFabricant == null || FabricantEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = FabricantMapper.Mapper.Map<FabricantResponse>(deleteFabricant);

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
