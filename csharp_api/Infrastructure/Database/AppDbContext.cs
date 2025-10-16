using csharp_api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace csharp_api.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Users> Users { get; set; }
    public DbSet<Court> Courts { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração de Users
        modelBuilder.Entity<Users>(entity =>
        {
            entity.ToTable("users");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Telefone).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            
            entity.HasIndex(e => e.Email).IsUnique();
            
            entity.HasMany(e => e.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração de Court
        modelBuilder.Entity<Court>(entity =>
        {
            entity.ToTable("courts");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PricePerHour).IsRequired().HasPrecision(10, 2);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.IsCovered).IsRequired();
            entity.Property(e => e.Capacity).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasMany(e => e.Reservations)
                .WithOne(r => r.Court)
                .HasForeignKey(r => r.CourtId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração de Reservation
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("reservations");
            
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CourtId).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.TotalPrice).IsRequired().HasPrecision(10, 2);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.CancellationReason).HasMaxLength(500);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Court)
                .WithMany(c => c.Reservations)
                .HasForeignKey(e => e.CourtId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasIndex(e => new { e.CourtId, e.StartTime, e.EndTime });
        });
    }
}