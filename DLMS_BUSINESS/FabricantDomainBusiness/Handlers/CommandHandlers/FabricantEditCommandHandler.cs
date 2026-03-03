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
    public class FabricantEditCommandHandler : IRequestHandler<FabricantEditCommand, ResponseBase<FabricantResponse>>
    {
        private readonly IFabricantCommandRepository _FabricantCommandRepository;
        private readonly IMediator _mediator;

        public FabricantEditCommandHandler(IFabricantCommandRepository FabricantCommandRepository, IMediator mediator, IFabricantQueryRepository FabricantQueryRepository)
        {
            _mediator = mediator;
            _FabricantCommandRepository = FabricantCommandRepository;
        }

        public async Task<ResponseBase<FabricantResponse>> Handle(FabricantEditCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<FabricantResponse> responseBase = new ResponseBase<FabricantResponse>();
            try
            {
                // Appliquer les modifications
                var FabricantEntity = FabricantMapper.Mapper.Map<Fabricant>(request);
                var editFabricant = await _FabricantCommandRepository.EditFabricant(FabricantEntity);

                if (editFabricant == null || FabricantEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = FabricantMapper.Mapper.Map<FabricantResponse>(editFabricant);

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
