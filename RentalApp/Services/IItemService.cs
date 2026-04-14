using RentalApp.Models;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<Item?> CreateItemAsync(Item item);
    Task<bool> UpdateItemAsync(Item item);
    Task<bool> DeleteItemAsync(int id);
}
