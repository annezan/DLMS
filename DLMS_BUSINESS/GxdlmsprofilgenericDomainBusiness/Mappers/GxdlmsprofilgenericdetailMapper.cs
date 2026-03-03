using AutoMapper;

namespace DLMS_BUSINESS.GxdlmsprofilgenericdetailDomainBusiness.Mappers
{
    public class GxdlmsprofilgenericdetailMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<GxdlmsprofilgenericdetailMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
