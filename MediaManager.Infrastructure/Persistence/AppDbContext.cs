using MediaManager.Domain.Entities;
using MediaManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MediaManager.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<MediaItem> MediaItems => Set<MediaItem>();
    public DbSet<ConsumptionRecord> ConsumptionRecords => Set<ConsumptionRecord>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.ClrType == typeof(Guid) || property.ClrType == typeof(Guid?))
                    property.SetColumnType("uuid");
                else if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    property.SetColumnType("timestamp with time zone");
                else if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    property.SetColumnType("numeric");
                else if (property.ClrType == typeof(string))
                    property.SetColumnType("text");
                else if (property.ClrType == typeof(int) || property.ClrType == typeof(int?))
                    property.SetColumnType("integer");
                else if (property.ClrType == typeof(bool) || property.ClrType == typeof(bool?))
                    property.SetColumnType("boolean");
            }
        }

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(256);

            entity.HasIndex(e => e.Email)
                  .IsUnique();

            entity.Property(e => e.PasswordHash)
                  .IsRequired();

            entity.Property(e => e.CriadoEm)
                  .IsRequired();
        });

        modelBuilder.Entity<MediaItem>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nome)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(e => e.Tipo)
                  .IsRequired()
                  .HasConversion<string>();

            entity.Property(e => e.CriadoEm)
                  .IsRequired();

            entity.Property(e => e.UserId)
                  .IsRequired();

            entity.HasOne(e => e.User)
                  .WithMany(e => e.MediaItems)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.ConsumptionRecords)
                  .WithOne(e => e.MediaItem)
                  .HasForeignKey(e => e.MediaItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConsumptionRecord>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MediaItemId)
                  .IsRequired();

            entity.Property(e => e.Status)
                  .IsRequired()
                  .HasConversion<string>();

            entity.Property(e => e.Nota)
                  .HasPrecision(3, 1);

            entity.Property(e => e.CriadoEm)
                  .IsRequired();
        });
    }
}
