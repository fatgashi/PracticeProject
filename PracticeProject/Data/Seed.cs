using Microsoft.AspNetCore.Identity;

namespace PracticeProject.Data
{
    public class Seed
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole<Guid>>
                {
                    new IdentityRole<Guid>
                    {
                        Name = "Client",
                        NormalizedName = "CLIENT"
                    },
                    new IdentityRole<Guid>
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    }
                };
                context.Roles.AddRange(roles);
            }
            await context.SaveChangesAsync();
        }
    }
}
