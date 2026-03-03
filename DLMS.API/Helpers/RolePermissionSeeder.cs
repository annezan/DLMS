using DLMS_MODELS.UsersDomain.Entities;
using DLMS_MODELS.UsersDomain.Enums;
using DLMS_DAL.Datas;

namespace DLMS.API.Helpers
{
    public static class RolePermissionSeeder
    {
        public static async Task SeedAdminPermissions(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

                // 🔥 1. Récupérer le rôle Admin
                var adminRole = context.Roles.FirstOrDefault(r => r.Code == RoleEnum.Admin.ToString());
                if (adminRole == null)
                {
                    Console.WriteLine("⛔ Le rôle Admin n'existe pas !");
                    return;
                }

                // 🔥 2. Récupérer toutes les permissions existantes
                var allPermissions = context.Permissions.ToList();
                if (!allPermissions.Any())
                {
                    Console.WriteLine("⛔ Aucune permission trouvée en base !");
                    return;
                }

                // 🔥 3. Récupérer les permissions déjà associées à Admin
                var existingPermissions = context.RolePermissions
                    .Where(rp => rp.RoleId == adminRole.Id)
                    .Select(rp => rp.PermissionId)
                    .ToHashSet();

                // 🔥 4. Ajouter uniquement les nouvelles permissions
                var newPermissions = new List<RolePermission>();
                foreach (var permission in allPermissions)
                {
                    if (!existingPermissions.Contains(permission.Id))
                    {
                        newPermissions.Add(new RolePermission
                        {
                            RoleId = adminRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }

                if (newPermissions.Any())
                {
                    await context.RolePermissions.AddRangeAsync(newPermissions);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"✅ {newPermissions.Count} permissions ajoutées au rôle Admin !");
                }
                else
                {
                    Console.WriteLine("🔹 Le rôle Admin a déjà toutes les permissions !");
                }
            }
        }
    }

}

