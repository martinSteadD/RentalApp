using RentalApp.Models;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync(int page = 1, int pageSize = 100);
    Task<Item?> GetItemByIdAsync(int id);
    Task<Item?> CreateItemAsync(Item item);
    Task<bool> UpdateItemAsync(Item item);
    Task<bool> DeleteItemAsync(int id);
    Task<List<Item>> GetMyItemsAsync();


}
