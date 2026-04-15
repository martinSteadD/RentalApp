using Microsoft.EntityFrameworkCore;
using RentalApp.Database.Models;
using System.IO;

namespace RentalApp.Database.Data;

public class AppDbContext : DbContext
{
    public DbSet<ItemEntity> Items { get; set; }

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "rentalapp.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ItemEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.DailyRate).IsRequired();
            entity.Property(e => e.CategoryId).IsRequired();
            entity.Property(e => e.OwnerId).IsRequired();
            entity.Property(e => e.OwnerName).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
