using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Models;
using System.Reflection.Emit;

namespace PracticeProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PaymentMethod>()
                .Property(p => p.AvailableBalance)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Transaction>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Transaction>(ms =>
            {
                ms.HasOne(t => t.Client)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.ClientId)
                .HasConstraintName("Transaction_Client")
                .OnDelete(DeleteBehavior.Cascade);

                ms.HasOne(t => t.PaymentMethod)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.PaymentMethodId)
                .HasConstraintName("Transaction_PaymentMethod")
                .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<Client>(d =>
            {
                d.HasOne(u => u.User)
                .WithOne(c => c.Client)
                .HasForeignKey<Client>(c => c.ClientId)
                .HasConstraintName("Client_User")
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Admin>(ms =>
            {
                ms.HasOne(d => d.User)
               .WithOne(p => p.Admin)
               .HasForeignKey<Admin>(d => d.AdminId)
               .HasConstraintName("Admin_User")
               .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
