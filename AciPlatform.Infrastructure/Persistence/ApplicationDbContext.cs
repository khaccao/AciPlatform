using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuRole> MenuRoles { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Configure UserRole entity
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // Configure Menu entity
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            
            // Self referencing relationship for Parent Menu
            entity.HasOne<Menu>()
                  .WithMany()
                  .HasForeignKey(d => d.CodeParent)
                  .HasPrincipalKey(p => p.Code)
                  .OnDelete(DeleteBehavior.Restrict); 

            // Note: Since CodeParent is string and not Id, we use HasPrincipalKey on Code. 
            // In the sample/db rules, CodeParent FK points to Menu.Code
        });

        // Configure MenuRole entity
        modelBuilder.Entity<MenuRole>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(d => d.Menu)
                  .WithMany()
                  .HasForeignKey(d => d.MenuId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.UserRole)
                  .WithMany()
                  .HasForeignKey(d => d.UserRoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Customer entity
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Phone).IsUnique();
        });
    }
}
