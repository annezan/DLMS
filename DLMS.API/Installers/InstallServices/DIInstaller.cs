using DLMS.Application;
using DLMS.Infrastructure;
using DLMS_SERVICE.Services;
using DLMS_MODELS.ServiceContracts;
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

            // Services pour l'exécution des commandes on-demand
            services.AddSingleton<IMeterLockService, MeterLockService>();
            services.AddSingleton<IDLMSGuruxSessionFactory, DLMSGuruxSessionFactory>();
            services.AddTransient<IDLMSHardwareService, DLMSHardwareService>();
            services.AddTransient<ICommandExecutor>(sp => sp.GetRequiredService<IDLMSHardwareService>());
        }
    }
}
