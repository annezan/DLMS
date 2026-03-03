using DLMS_DAL.GxDLMSDomainDal.Repositories.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.ReadObjectCommandeDomain.Queries;
using MediatR;

namespace DLMS_BUSINESS.ReadObjectCommandeDomainBusiness.Handlers.QueryHandlers
{
    public class GetReadObjectCommandeQueryHandler : IRequestHandler<GetReadObjectCommandeQuery, ResponseBase<string>>
    {
        private readonly IReadObjectCommandeQueryRepository _readObjectCommandeQueryRepository;

        public GetReadObjectCommandeQueryHandler(
            IReadObjectCommandeQueryRepository readObjectCommandeQueryRepository)
        {
            _readObjectCommandeQueryRepository = readObjectCommandeQueryRepository;
        }

        public async Task<ResponseBase<string>> Handle(GetReadObjectCommandeQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<string> responseBase = new ResponseBase<string>();

            try
            {
                // Exécuter la commande
                var result = await _readObjectCommandeQueryRepository.GetReadObjectCommande(request.CommandeId);

                if (result == null)
                {
                    responseBase.IsSuccess = false;
                    responseBase.Message = "Erreur lors de l'exécution de la commande";
                    return responseBase;
                }

                responseBase.Data = result;
                responseBase.IsSuccess = true;
                responseBase.Message = "Commande exécutée avec succès";

                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = $"Erreur lors de l'exécution de la commande : {ex.Message}";
                return responseBase;
            }
        }
    }
}
