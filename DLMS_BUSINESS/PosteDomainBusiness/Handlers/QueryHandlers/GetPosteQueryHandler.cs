using DLMS_BUSINESS.PosteDomainBusiness.Mappers;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_DAL.PosteDomainDal.Repositories.Queries;
using DLMS_MODELS.AssociationKeyDomain.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.PosteDomain.Queries;
using DLMS_MODELS.PosteDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.PosteDomainBusiness.Handlers.QueryHandlers
{
    public class GetPosteQueryHandler : IRequestHandler<GetPosteQuery, ResponseBase<List<PosteResponse>>>
    {
        private readonly IPosteQueryRepository _PosteQueryRepository;

        public GetPosteQueryHandler(IPosteQueryRepository PosteQueryRepository)
        {
            _PosteQueryRepository = PosteQueryRepository;
        }

        public async Task<ResponseBase<List<PosteResponse>>> Handle(GetPosteQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<PosteResponse>> responseBase = new ResponseBase<List<PosteResponse>>();
            try
            {
                var Poste = await _PosteQueryRepository.GetPoste();
                
                if (Poste == null || Poste.Count == 0)
                {
                    responseBase.IsSuccess = true;
                    responseBase.Message = "Aucun poste trouvé";
                    responseBase.Data = new List<PosteResponse>();
                    return responseBase;
                }
                
                responseBase.Data = PosteMapper.Mapper.Map<List<PosteResponse>>(Poste);
                responseBase.IsSuccess = true;
                responseBase.Message = "Postes récupérés avec succès";
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de la récupération des postes: {ex.Message}";
                return responseBase;
            }
        }
    }
}
