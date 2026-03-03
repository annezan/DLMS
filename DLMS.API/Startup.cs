using System;
using DLMS.API.Helpers;
using DLMS.API.Installers.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DLMS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseCors("AllowReact");


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            // ❌ Désactivé au profit du système d'attributs
            // app.UseMiddleware<PermissionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // 🚀 Exécuter PermissionSeeder après le mappage des routes
            // 🔥 Exécuter les seeders séquentiellement avec `async`
            Task.Run(async () =>
            {
                await PermissionSeeder.SeedPermissions(app.ApplicationServices);
                await RoleSeeder.SeedRoles(app.ApplicationServices);
                await RolePermissionSeeder.SeedAdminPermissions(app.ApplicationServices);
                await SeedUser.SeedUsers(app.ApplicationServices);                
            }).Wait(); // Attendre que les tâches soient terminées avant de continuer

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
        }

    }
}
