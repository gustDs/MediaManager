using MediaManager.Domain.Entities;
using MediaManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MediaManager.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<MediaItem> MediaItems => Set<MediaItem>();
    public DbSet<ConsumptionRecord> ConsumptionRecords => Set<ConsumptionRecord>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
