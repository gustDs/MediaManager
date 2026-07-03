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
