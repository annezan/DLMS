using AutoMapper;
using DLMS_BUSINESS.GxDLMSDomainDomainBusiness.Mappers;

namespace DLMS_BUSINESS.GxDLMSDomainBusiness.Mappers
{
    public class ReadRowsByEntryMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ReadRowsByEntryMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
