using Microsoft.EntityFrameworkCore;
using Backend_Gym_Iris.Entities;

namespace Backend_Gym_Iris.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MembershipPrice> MembershipPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasConversion<string>();

            // Configuración Payment
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.MembershipType)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => new { p.UserId, p.Month, p.Year })
                .IsUnique();

            // Configuración MembershipPrice
            modelBuilder.Entity<MembershipPrice>()
                .Property(mp => mp.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MembershipPrice>()
                .Property(mp => mp.Type)
                .HasConversion<string>();

            modelBuilder.Entity<MembershipPrice>()
                .HasIndex(mp => mp.Type)
                .IsUnique();
        }
    }
}