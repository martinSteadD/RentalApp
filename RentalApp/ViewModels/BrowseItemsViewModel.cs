using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RentalApp.Models;
using RentalApp.Services;
using RentalApp.Database.Data;
using RentalApp.Database.Models;
using System.Collections.ObjectModel;

namespace RentalApp.ViewModels;

public partial class BrowseItemsViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly AppDbContext _db;
    private readonly CategoryService _categoryService;


    public ObservableCollection<Item> Items { get; } = new();


    public BrowseItemsViewModel(
        IItemService itemService,
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore,
        AppDbContext db,
        CategoryService categoryService)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        _db = db;
        _categoryService = categoryService;

        Title = "Browse Items";

    }

    public async Task LoadItemsAsync()
    {
        try
        {
            Console.WriteLine("DEBUG: LoadItemsAsync started");

            IsBusy = true;

            // 1. Load API items
            var apiItems = await _itemService.GetItemsAsync();

            // 2. Load local DB items
            var localEntities = await _db.Items.ToListAsync();

            // 3. Load categories so we can map CategoryId -> CategoryName
            var categories = await _categoryService.GetCategoriesAsync();

            // 4. Convert ItemEntity -> Item
            var localItems = localEntities.Select(e =>
            {
                var cat = categories.FirstOrDefault(c => c.Id == e.CategoryId);

                return new Item
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    DailyRate = e.DailyRate,

                    CategoryId = e.CategoryId,
                    Category = cat?.Name ?? e.CategoryName ?? "Unknown",

                    OwnerId = e.OwnerId,
                    OwnerName = e.OwnerName,

                    IsAvailable = e.IsAvailable,
                    CreatedAt = e.CreatedAt,

                    // Local items don't have these fields — safe defaults
                    OwnerRating = 0,
                    AverageRating = 0,
                    Latitude = 0,
                    Longitude = 0
                };
            });

            // 5. Merge API + Local (UI FIX)
            Items.Clear();

            foreach (var item in apiItems.Concat(localItems))
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to load items: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SelectItem(Item item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync("ItemDetailPage", true, new Dictionary<string, object>
        {
            { "Item", item }
        });
    }
}
