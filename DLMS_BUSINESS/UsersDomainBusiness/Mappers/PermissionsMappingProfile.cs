using AutoMapper;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Responses;

namespace DLMS_BUSINESS.UsersDomainBusiness.Mappers
{
    public class PermissionsMappingProfile : Profile
    {
        public PermissionsMappingProfile()
        {
            CreateMap<Permission, PermissionResponse>().ReverseMap();
            // CreateMap<Role, RoleAddCommand>().ReverseMap();
        }
    }
}
