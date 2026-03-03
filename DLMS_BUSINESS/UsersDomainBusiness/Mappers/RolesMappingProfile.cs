using AutoMapper;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Responses;

namespace DLMS_BUSINESS.UsersDomainBusiness.Mappers
{
    public class RolesMappingProfile : Profile
    {
        public RolesMappingProfile()
        {
            // Mapping pour Permission -> PermissionResponse (nécessaire pour le mapping des Roles)
            CreateMap<Permission, PermissionResponse>();
            
            CreateMap<Role, RoleResponse>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => 
                    src.RolePermissions.Select(rp => rp.Permissions).ToList()))
                .ReverseMap();
            CreateMap<Role, RoleAddCommand>().ReverseMap();
            CreateMap<Role, RoleEditCommand>().ReverseMap();
            CreateMap<Role, RoleDeleteCommand>().ReverseMap();
        }
    }
}
