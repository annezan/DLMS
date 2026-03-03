using DLMS_BUSINESS.FabricantDomainBusiness.Mappers;
using DLMS_DAL.FabricantDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.FabricantDomain.Commands;
using DLMS_MODELS.FabricantDomain.Entities;
using DLMS_MODELS.FabricantDomain.Queries;
using DLMS_MODELS.FabricantDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_DAL.FabricantDomainDal.Repositories;

namespace DLMS_BUSINESS.FabricantDomainBusiness.Handlers.CommandHandlers
{
    public class FabricantAddCommandHandler : IRequestHandler<FabricantAddCommand, ResponseBase<FabricantResponse>>
    {
        private readonly IFabricantCommandRepository _FabricantCommandRepository;
        private readonly IMediator _mediator;

        public FabricantAddCommandHandler(IFabricantCommandRepository FabricantCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _FabricantCommandRepository = FabricantCommandRepository;
        }

        public async Task<ResponseBase<FabricantResponse>> Handle(FabricantAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<FabricantResponse> responseBase = new ResponseBase<FabricantResponse>();
            try
            {
                var FabricantEntity = FabricantMapper.Mapper.Map<Fabricant>(request);
                //FabricantEntity.CreatedAt = DateTime.Now;
                var newFabricant = await _FabricantCommandRepository.AddFabricant(FabricantEntity);
                if (newFabricant == null || FabricantEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }

                responseBase.Data = FabricantMapper.Mapper.Map<FabricantResponse>(newFabricant);

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu" + ex.Message;
                return responseBase;
            }
            
        }

    }

}
