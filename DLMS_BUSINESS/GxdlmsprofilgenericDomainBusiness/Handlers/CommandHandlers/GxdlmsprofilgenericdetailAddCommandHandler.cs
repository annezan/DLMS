using DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Mappers;
using DLMS_DAL.GxdlmsprofilgenericDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Commands;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Entities;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Queries;
using DLMS_MODELS.GxdlmsprofilgenericDomain.Responses;
using MediatR;
using DLMS_DAL.GxdlmsprofilgenericdetailDomainDal.Repositories;

namespace DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Handlers.CommandHandlers
{
    public class GxdlmsprofilgenericdetailAddCommandHandler : IRequestHandler<GxdlmsprofilgenericdetailAddCommand, ResponseBase<bool>>
    {
        private readonly IGxdlmsprofilgenericdetailCommandRepository _GxdlmsprofilgenericdetailCommandRepository;
        private readonly IMediator _mediator;

        public GxdlmsprofilgenericdetailAddCommandHandler(IGxdlmsprofilgenericdetailCommandRepository GxdlmsprofilgenericdetailCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _GxdlmsprofilgenericdetailCommandRepository = GxdlmsprofilgenericdetailCommandRepository;
        }

        public async Task<ResponseBase<bool>> Handle(GxdlmsprofilgenericdetailAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<bool> responseBase = new ResponseBase<bool>();

            var GxdlmsprofilgenericdetailEntity = GxdlmsprofilgenericdetailMapper.Mapper.Map<Gxdlmsprofilgenericdetail>(request);

            if (GxdlmsprofilgenericdetailEntity is null)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Un problème technique est survenu";
                return responseBase;
            }

            //GxdlmsprofilgenericdetailEntity.CreatedAt = DateTime.Now;
            var newGxdlmsprofilgenericdetail = await _GxdlmsprofilgenericdetailCommandRepository.AddGxdlmsprofilgenericdetail(GxdlmsprofilgenericdetailEntity);

            //responseBase.Data = GxdlmsprofilgenericdetailMapper.Mapper.Map<GxdlmsprofilgenericdetailResponse>(newGxdlmsprofilgenericdetail);

            return responseBase;
        }

    }

}
