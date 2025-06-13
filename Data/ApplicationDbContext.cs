using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações das entidades
            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
                entity.Property(w => w.Position).IsRequired().HasMaxLength(50);
                entity.Property(w => w.Gender).IsRequired().HasMaxLength(1);
            });

            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Status).IsRequired().HasMaxLength(20);
                entity.Property(t => t.Type).IsRequired().HasMaxLength(30);
                entity.Property(t => t.Deadline).IsRequired();
                
                entity.HasOne(t => t.Worker)
                      .WithMany(w => w.Tasks)
                      .HasForeignKey(t => t.WorkerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(30);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
                
                entity.HasOne(u => u.Worker)
                      .WithMany(w => w.Users)
                      .HasForeignKey(u => u.WorkerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}