using csharp_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace csharp_api.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Users> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>(entity =>
        {
            entity.ToTable("users");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome);
            entity.Property(e => e.Telefone);
            
            entity.HasIndex(e => e.Id).IsUnique();
        });
    }

}