using DLMS_DAL.Datas;
using DLMS_MODELS.UsersDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLMS.API.Helpers
{
    public static class SeedUser
    {
        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DLMSDBContext>();

                // 🔥 1. Récupérer tous les rôles existants
                var roles = await context.Roles.ToListAsync();
                if (!roles.Any())
                {
                    Console.WriteLine("⛔ Aucun rôle trouvé en base !");
                    return;
                }

                var defaultPassword = "Operating0"; // Mot de passe commun hashé
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(defaultPassword);

                // 🔥 2. Liste des utilisateurs à créer par rôle
                var usersToCreate = new List<User>();

                foreach (var role in roles)
                {
                    var email = $"{role.Code.ToLower()}@gmail.com"; // Un email unique par rôle

                    if (!await context.Users.AnyAsync(u => u.Email == email))
                    {
                        usersToCreate.Add(new User
                        {
                            Nom = $"{role.Code}",
                            Prenoms = $"Utilisateur",
                            Mobile = $"0101010101",
                            Email = email,
                            MotDePasse = hashedPassword, // 🔥 Mot de passe hashé
                            CodeValidation = null,
                            DateNaissance = null,
                            RoleId = role.Id,
                            Role = role
                        });
                    }
                }

                // 🔥 3. Ajouter uniquement les nouveaux utilisateurs
                if (usersToCreate.Any())
                {
                    await context.Users.AddRangeAsync(usersToCreate);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"✅ {usersToCreate.Count} utilisateurs créés !");
                }
                else
                {
                    Console.WriteLine("🔹 Tous les utilisateurs existent déjà.");
                }
            }
        }
    }
}

