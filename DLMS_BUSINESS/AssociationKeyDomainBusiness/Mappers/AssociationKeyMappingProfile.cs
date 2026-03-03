using AutoMapper;
using DLMS_MODELS.AssociationKeyDomain.Commands;
using DLMS_MODELS.AssociationKeyDomain.Entities;
using DLMS_MODELS.AssociationKeyDomain.Responses;

namespace DLMS_BUSINESS.AssociationKeyDomainBusiness.Mappers
{
    public class AssociationKeyMappingProfile : Profile
    {
        public AssociationKeyMappingProfile()
        {
            CreateMap<AssociationKey, AssociationKeyResponse>().ReverseMap();
            CreateMap<AssociationKey, AssociationKeyAddCommand>().ReverseMap();
            CreateMap<AssociationKey, AssociationKeyEditCommand>().ReverseMap();
        }
    }
}
