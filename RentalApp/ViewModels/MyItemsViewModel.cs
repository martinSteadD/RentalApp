using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RentalApp.Database.Data;
using RentalApp.Database.Models;
using RentalApp.Services;
using System.Collections.ObjectModel;
using RentalApp.Views;


namespace RentalApp.ViewModels;

public partial class MyItemsViewModel : BaseViewModel
{
    private readonly AppDbContext _db;
    private readonly CategoryService _categoryService;

    [ObservableProperty]
    private ObservableCollection<ItemEntity> myItems = new();

    [ObservableProperty]
    private ItemEntity? selectedItem;

    private readonly IItemService _itemService;

    public MyItemsViewModel(
        AppDbContext db,
        IAuthenticationService authService,
        INavigationService navigationService,
        CategoryService categoryService,
        IItemService itemService)
        : base(authService, navigationService)
    {
        _db = db;
        _categoryService = categoryService;
        _itemService = itemService;

        Title = "My Items";

    }

    [RelayCommand]
    public async Task LoadMyItemsAsync()
    {
        try
        {
            IsBusy = true;

            var userId = CurrentUser!.Id;

            // Try API first
            var allItems = await _itemService.GetItemsAsync(); // GET /items
            var apiItems = allItems.Where(i => i.OwnerId == userId).ToList();


            if (apiItems != null && apiItems.Any())
            {
                // Clear local DB
                var localItems = _db.Items.Where(i => i.OwnerId == userId);
                _db.Items.RemoveRange(localItems);
                await _db.SaveChangesAsync();

                // Insert API items into SQLite
                foreach (var apiItem in apiItems)
                {
                    var entity = new ItemEntity
                    {
                        Id = apiItem.Id,
                        Title = apiItem.Title ?? string.Empty,
                        Description = apiItem.Description ?? string.Empty,
                        DailyRate = apiItem.DailyRate ?? 0,
                        CategoryId = apiItem.CategoryId ?? 0,
                        CategoryName = apiItem.Category ?? "Unknown",
                        OwnerId = apiItem.OwnerId ?? 0,
                        OwnerName = apiItem.OwnerName ?? "Unknown",
                        CreatedAt = apiItem.CreatedAt ?? DateTime.UtcNow,
                        IsAvailable = apiItem.IsAvailable ?? true
                    };

                    _db.Items.Add(entity);
                }

                await _db.SaveChangesAsync();
            }

            // Load from SQLite (API or offline)
            var items = await _db.Items
                .Where(i => i.OwnerId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            MyItems = new ObservableCollection<ItemEntity>(items);
        }
        catch (Exception ex)
        {
            SetError($"Failed to load your items: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }


    [RelayCommand]
    private async Task SelectItemAsync(ItemEntity item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync("ItemDetailPage", true, new Dictionary<string, object>
        {
            { "Item", item }
        });
    }

    [RelayCommand]
    private async Task GoToCreateItemAsync()
    {
        await Shell.Current.GoToAsync($"/{nameof(CreateItemPage)}");
    }






}
