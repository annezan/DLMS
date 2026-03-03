using AutoMapper;
using DLMS_MODELS.PosteDomain.Entities;
using DLMS_MODELS.PosteDomain.Responses;
using DLMS_MODELS.UsersDomain.Commands;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Responses;

namespace DLMS_BUSINESS.UsersDomainBusiness.Mappers
{
    public class UsersMappingProfile : Profile
    {
        public UsersMappingProfile()
        {
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<Poste, PosteResponse>().ReverseMap();
            CreateMap<User, UsersResponse>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role)).ReverseMap();
            CreateMap<User, UsersResponse>().ForMember(dest => dest.Poste, opt => opt.MapFrom(src => src.Poste)).ReverseMap();

            CreateMap<User, UsersRegisterCommand>().ReverseMap();
            CreateMap<User, UsersEditCommand>().ReverseMap();
            CreateMap<User, UsersAddCommand>().ReverseMap();
            CreateMap<User, UsersDeleteCommand>().ReverseMap();
        }
    }
}
