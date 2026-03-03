using DLMS_BUSINESS.EquipementDomainBusiness.Mappers;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_DAL.EquipementDomainDal.Repositories.Queries;
using DLMS_MODELS.AssociationKeyDomain.Queries;
using DLMS_MODELS.Bases;
using DLMS_MODELS.EquipementDomain.Queries;
using DLMS_MODELS.EquipementDomain.Responses;
using MediatR;

namespace DLMS_BUSINESS.EquipementDomainBusiness.Handlers.QueryHandlers
{
    public class GetEquipementQueryHandler : IRequestHandler<GetEquipementQuery, ResponseBase<List<EquipementResponse>>>
    {
        private readonly IEquipementQueryRepository _EquipementQueryRepository;

        public GetEquipementQueryHandler(IEquipementQueryRepository EquipementQueryRepository)
        {
            _EquipementQueryRepository = EquipementQueryRepository;
        }

        public async Task<ResponseBase<List<EquipementResponse>>> Handle(GetEquipementQuery request, CancellationToken cancellationToken)
        {
            ResponseBase<List<EquipementResponse>> responseBase = new ResponseBase<List<EquipementResponse>>();
            try
            {
                var Equipement = await _EquipementQueryRepository.GetEquipement();
                responseBase.Data = EquipementMapper.Mapper.Map<List<EquipementResponse>>(Equipement);
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.IsSuccess = false;
                responseBase.Message = "Equipement introuvable";
                return responseBase;
            }
        }
    }
}
