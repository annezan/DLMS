using DLMS_BUSINESS.PosteDomainBusiness.Mappers;
using DLMS_DAL.PosteDomainDal.Repositories.Commands;
using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Commands;
using DLMS_MODELS.PosteDomain.Entities;
using DLMS_MODELS.PosteDomain.Queries;
using DLMS_MODELS.PosteDomain.Responses;
using MediatR;
using DLMS_MODELS.AssocationKeyDomain.Queries;
using DLMS_DAL.PosteDomainDal.Repositories;

namespace DLMS_BUSINESS.PosteDomainBusiness.Handlers.CommandHandlers
{
    public class PosteAddCommandHandler : IRequestHandler<PosteAddCommand, ResponseBase<PosteResponse>>
    {
        private readonly IPosteCommandRepository _PosteCommandRepository;
        private readonly IMediator _mediator;

        public PosteAddCommandHandler(IPosteCommandRepository PosteCommandRepository, IMediator mediator)
        {
            _mediator = mediator;
            _PosteCommandRepository = PosteCommandRepository;
        }

        public async Task<ResponseBase<PosteResponse>> Handle(PosteAddCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<PosteResponse> responseBase = new ResponseBase<PosteResponse>();
            try
            {
                var PosteEntity = PosteMapper.Mapper.Map<Poste>(request);

                //PosteEntity.CreatedAt = DateTime.Now;
                var newPoste = await _PosteCommandRepository.AddPoste(PosteEntity);
                if (newPoste == null || PosteEntity==null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu";
                    return responseBase;
                }
                responseBase.Data = PosteMapper.Mapper.Map<PosteResponse>(newPoste);

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
