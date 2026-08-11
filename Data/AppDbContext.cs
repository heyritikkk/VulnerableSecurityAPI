namespace VulnerableSecurityAPI.Data;

using Microsoft.EntityFrameworkCore;
using VulnerableSecurityAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@example.com", Password = "adminpassword123", Role = "Admin" },
            new User { Id = 2, Username = "testuser", Email = "user@example.com", Password = "userpassword123", Role = "User" }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Secure Laptop", Description = "A laptop for secure coding.", Price = 1500.00m, Category = "Electronics" },
            new Product { Id = 2, Name = "Security Book", Description = "Learn AppSec.", Price = 45.00m, Category = "Books" }
        );
    }
}
