using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Enums;

namespace DLMS.API.Helpers
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

                // Liste des rôles à insérer
                var roles = new[]
                {
                new Role { Libelle = "Administrateur", Code = RoleEnum.Admin.ToString(), Description = "Gère tout le système" },
                new Role { Libelle = "Gestionnaire", Code = RoleEnum.Manager.ToString(), Description = "Gère les équipes et les projets" },
                new Role { Libelle = "Technicien", Code = RoleEnum.Technicien.ToString(), Description = "Intervient sur les aspects techniques" }
            };

                // Vérifier et insérer seulement les nouveaux rôles
                foreach (var role in roles)
                {
                    if (!context.Roles.Any(r => r.Code == role.Code))
                    {
                        await context.Roles.AddAsync(role);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }

}

