using RentalApp.Database.Models;

namespace RentalApp.Database.Repositories;

// This repository is now a NO-OP placeholder.
// It compiles, but does not use EF Core or SQLite.
// You can keep it for reference without breaking the build.

public class ItemRepository : IItemRepository
{
    public Task<List<Item>> GetAllAsync()
    {
        // Return an empty list so the app can build
        return Task.FromResult(new List<Item>());
    }

    public Task<Item?> GetByIdAsync(int id)
    {
        // Return null placeholder
        return Task.FromResult<Item?>(null);
    }

    public Task<Item> AddAsync(Item item)
    {
        // Return the same item so the app compiles
        return Task.FromResult(item);
    }

    public Task<Item> UpdateAsync(Item item)
    {
        // Return the same item so the app compiles
        return Task.FromResult(item);
    }

    public Task<bool> DeleteAsync(int id)
    {
        // Pretend delete succeeded
        return Task.FromResult(true);
    }
}
