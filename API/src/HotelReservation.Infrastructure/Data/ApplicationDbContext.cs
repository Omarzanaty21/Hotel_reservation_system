using HotelReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HotelReservation.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Room Configuration
        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Photo).HasMaxLength(500);
            
            // Map the Enum cleanly to its string representation
            entity.Property(e => e.Capacity)
                  .HasConversion<string>();

            // Precision for price
            entity.Property(e => e.PricePerNight)
                  .HasColumnType("decimal(18,2)");
        });

        // Reservation Configuration
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(e => e.GuestName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.GuestEmail).HasMaxLength(255).IsRequired();
            entity.Property(e => e.GuestNumber).HasMaxLength(50);
            
            // Setup relation
            entity.HasOne(r => r.Room)
                  .WithMany(room => room.Reservations)
                  .HasForeignKey(r => r.RoomId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
                entry.Entity.UpdatedAt = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = utcNow;
            }
        }
    }
}

