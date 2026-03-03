using DLMS_DAL.Datas;
using DLMS_MODELS;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS.API.Installers.InstallServices
{
    public class DataBaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            asc_connection.connection ascuser = new asc_connection.connection();
            var env = configuration.GetSection("env").Value;
            var serveur = configuration.GetSection("Serveur").Value;
            var DB = configuration.GetSection("DB").Value;
            var user = "";
            var pass = "";

            user = ascuser.asc_user;
            pass = ascuser.asc_pass;



            //var constr = String.Format("Server = {0}; Database = {1}; Trusted_Connection = true; TrustServerCertificate = true;", serveur, DB);
            var constr= String.Format("Server = {0}; Database = {1}; Trusted_Connection = false;TrustServerCertificate=true;MultipleActiveResultSets=true; user id = {2};password = {3};", serveur, DB, user, pass);
            //var connectionString = configuration.GetSection("ConnectionStrings").GetSection("DefaultConnexion").Value;
            GlobalVariable.ConString = constr;
            //services.AddDbContext<DLMSDBContext>(opts => opts.UseSqlServer(configuration["ConnectionString:DefaultConnection"]));
            //services.AddDbContext<DLMSDBContext>(opts => opts.UseSqlServer(GlobalVariable.ConString));
            services.AddDbContext<DLMSDBContext>(opts =>
                    opts.UseSqlServer(GlobalVariable.ConString, options =>
                    {
                        options.CommandTimeout(120);
                    }));

        }
    }
}
