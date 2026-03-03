using DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Mappers;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Commands;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;

namespace DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Handlers.CommandHandlers
{
    public class GxdlmsprofilgenericdetailDeleteCommandHandler : IRequestHandler<GxdlmsprofilgenericdetailDeleteCommand, ResponseBase<GxdlmsprofilgenericdetailResponse>>
    {
        private readonly IGxdlmsprofilgenericdetailCommandRepository _GxdlmsprofilgenericdetailCommandRepository;
        private readonly IMediator _mediator;

        public GxdlmsprofilgenericdetailDeleteCommandHandler(IGxdlmsprofilgenericdetailCommandRepository GxdlmsprofilgenericdetailCommandRepository, IMediator mediator, IGxdlmsprofilgenericdetailQueryRepository GxdlmsprofilgenericdetailQueryRepository)
        {
            _mediator = mediator;
            _GxdlmsprofilgenericdetailCommandRepository = GxdlmsprofilgenericdetailCommandRepository;
        }

        public async Task<ResponseBase<GxdlmsprofilgenericdetailResponse>> Handle(GxdlmsprofilgenericdetailDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<GxdlmsprofilgenericdetailResponse> responseBase = new ResponseBase<GxdlmsprofilgenericdetailResponse>();

            

            // Appliquer les modifications
            var GxdlmsprofilgenericdetailEntity = GxdlmsprofilgenericdetailMapper.Mapper.Map<Gxdlmsprofilgenericdetail>(request);

            if (GxdlmsprofilgenericdetailEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            try
            {
                var newGxdlmsprofilgenericdetail = await _GxdlmsprofilgenericdetailCommandRepository.DeleteGxdlmsprofilgenericdetail(GxdlmsprofilgenericdetailEntity);

                responseBase.Data = GxdlmsprofilgenericdetailMapper.Mapper.Map<GxdlmsprofilgenericdetailResponse>(newGxdlmsprofilgenericdetail);

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
