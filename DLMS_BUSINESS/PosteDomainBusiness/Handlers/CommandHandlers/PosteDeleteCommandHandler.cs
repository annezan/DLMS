using DLMS_BUSINESS.PosteDomainBusiness.Mappers;
using DLMS_DAL.PosteDomainDal.Repositories.Commands;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_MODELS.PosteDomain.Commands;
using DLMS_MODELS.PosteDomain.Entities;
using DLMS_MODELS.PosteDomain.Responses;
using DLMS_MODELS.Bases;
using MediatR;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;

namespace DLMS_BUSINESS.PosteDomainBusiness.Handlers.CommandHandlers
{
    public class PosteDeleteCommandHandler : IRequestHandler<PosteDeleteCommand, ResponseBase<PosteResponse>>
    {
        private readonly IPosteCommandRepository _PosteCommandRepository;
        private readonly IMediator _mediator;

        public PosteDeleteCommandHandler(IPosteCommandRepository PosteCommandRepository, IMediator mediator, IPosteQueryRepository PosteQueryRepository)
        {
            _mediator = mediator;
            _PosteCommandRepository = PosteCommandRepository;
        }

        public async Task<ResponseBase<PosteResponse>> Handle(PosteDeleteCommand request, CancellationToken cancellationToken)
        {
            ResponseBase<PosteResponse> responseBase = new ResponseBase<PosteResponse>();

            // Appliquer les modifications
            try
            {
                var PosteEntity = PosteMapper.Mapper.Map<Poste>(request);

                var newPoste = await _PosteCommandRepository.DeletePoste(PosteEntity);
                if (newPoste == null || PosteEntity == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Un problème technique est survenu ";
                    return responseBase;
                }
                responseBase.Data = PosteMapper.Mapper.Map<PosteResponse>(newPoste);

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
