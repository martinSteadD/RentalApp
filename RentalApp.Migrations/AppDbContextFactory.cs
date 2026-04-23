using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RentalApp.Database.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // IMPORTANT: This connection string is ONLY for design-time (migrations)
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=appdb;Username=postgres;Password=postgres");

        return new AppDbContext(optionsBuilder.Options);
    }
}
