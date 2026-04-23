using RentalApp.Database.Models;

namespace RentalApp.Database.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(int id);
    Task<Item> AddAsync(Item item);
    Task<Item> UpdateAsync(Item item);
    Task<bool> DeleteAsync(int id);
}
