using DLMS.Application;
using DLMS.Infrastructure;
using System.Text.Json.Serialization;

namespace DLMS.API.Installers.InstallServices
{
    public class DIInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions
                .ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddApplication();
            services.AddInfrastructure();
        }
    }
}
