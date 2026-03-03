namespace DLMS.API.Installers.InstallServices
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

                options.AddPolicy("AllowReact",
                policy => policy
                    .WithOrigins("http://localhost:5174")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
        }
    }
}
