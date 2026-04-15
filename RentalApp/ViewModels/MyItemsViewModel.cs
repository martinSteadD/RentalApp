using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RentalApp.Database.Data;
using RentalApp.Database.Models;
using RentalApp.Services;
using System.Collections.ObjectModel;

namespace RentalApp.ViewModels;

public partial class MyItemsViewModel : BaseViewModel
{
    private readonly AppDbContext _db;
    private readonly CategoryService _categoryService;

    [ObservableProperty]
    private ObservableCollection<ItemEntity> myItems = new();

    public MyItemsViewModel(
        AppDbContext db,
        IAuthenticationService authService,
        INavigationService navigationService,
        CategoryService categoryService)
        : base(authService, navigationService)
    {
        _db = db;
        _categoryService = categoryService;

        Title = "My Items";

        _ = LoadMyItemsAsync();
    }

    [RelayCommand]
    public async Task LoadMyItemsAsync()
    {
        try
        {
            IsBusy = true;

            var userId = CurrentUser!.Id;

            // Load items from SQLite
            var items = await _db.Items
                .Where(i => i.OwnerId == userId)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();

            // Load categories from API (or local if you later store them)
            var categories = await _categoryService.GetCategoriesAsync();

            // Map CategoryId -> CategoryName
            foreach (var item in items)
            {
                var cat = categories.FirstOrDefault(c => c.Id == item.CategoryId);
                item.CategoryName = cat?.Name ?? "Unknown";
            }

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
}
