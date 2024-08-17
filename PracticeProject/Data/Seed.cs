using Microsoft.AspNetCore.Identity;
using PracticeProject.Enums;
using PracticeProject.Models;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PracticeProject.Data
{
    public class Seed
    {
        public static async Task SeedData(ApplicationDbContext context, UserManager<User> userManager)
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
                await context.SaveChangesAsync();
            }
            if (!userManager.Users.Any())
            {
                var clientUser = new User
                {
                    UserName = "client",
                    Email = "client@example.com",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var clientResult = await userManager.CreateAsync(clientUser, "Client_1");
                var adminResult = await userManager.CreateAsync(adminUser, "Admin_1");

                if (clientResult.Succeeded && adminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(clientUser, "Client");
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    var paymentMethod = new PaymentMethod
                    {
                        Name = "Credit Card",
                        AvailableBalance = 200,
                    };
                    context.PaymentMethods.Add(paymentMethod);

                    var client = new Client
                    {
                        User = clientUser,
                        ClientId = Guid.NewGuid()
                    };
                    context.Clients.Add(client);

                    var transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Amount = 50.00m,
                        Currency = "USD",
                        Status = TransactionStatus.Pending,
                        Type = TransactionType.Deposit,
                        ClientId = client.ClientId,
                        PaymentMethod = paymentMethod,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Transaction
                    {
                        Amount = 75.00m,
                        Currency = "USD",
                        Status = TransactionStatus.Pending,
                        Type = TransactionType.Withdrawal,
                        ClientId = client.ClientId,
                        PaymentMethod = paymentMethod,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                };

                    context.Transactions.AddRange(transactions);

                    await context.SaveChangesAsync();
                }
                else
                {
                    var errors = string.Join("; ", clientResult.Errors.Select(e => e.Description).Concat(adminResult.Errors.Select(e => e.Description)));
                    throw new Exception($"Failed to create users: {errors}");
                }
            }
        }

    }
}
