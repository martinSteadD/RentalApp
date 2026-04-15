using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using RentalApp.Models;
using RentalApp.Services;
using RentalApp.Database.Data;
using System.Collections.ObjectModel;
using RentalApp.Database.Models;


namespace RentalApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly CategoryService _categoryService;
    private readonly AppDbContext _db;

    // Form fields
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string dailyRate = string.Empty;

    // Category Picker
    public ObservableCollection<Category> Categories { get; } = new();

    [ObservableProperty]
    private Category? selectedCategory;

    public CreateItemViewModel(
        IItemService itemService,
        CategoryService categoryService,
        IAuthenticationService authService,
        INavigationService navigationService,
        AppDbContext db)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        _categoryService = categoryService;
        _db = db;

        Title = "Create Item";

        LoadCategories();
    }

    private async void LoadCategories()
    {
        try
        {
            var list = await _categoryService.GetCategoriesAsync();

            Categories.Clear();
            foreach (var c in list)
                Categories.Add(c);
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync("Error", "Failed to load categories.", "OK");
        }
    }

    [RelayCommand]
    private async Task CreateItemAsync()
    {
        if (string.IsNullOrWhiteSpace(Title) ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(DailyRate) ||
            SelectedCategory == null)
        {
            await Shell.Current.DisplayAlertAsync("Error", "Please fill in all required fields.", "OK");
            return;
        }

        // Build the item for API POST
        var item = new Item
        {
            Title = Title,
            Description = Description,
            DailyRate = decimal.Parse(DailyRate),
            CategoryId = SelectedCategory.Id,
            Category = SelectedCategory.Name,
            OwnerId = CurrentUser!.Id,
            OwnerName = $"{CurrentUser.FirstName} {CurrentUser.LastName}",
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };

        // Send to API (mock API returns success but does not persist)
        var result = await _itemService.CreateItemAsync(item);

        if (result == null)
        {
            await Shell.Current.DisplayAlertAsync("Error", "Failed to create item.", "OK");
            return;
        }

        // Save to local SQLite DB for real persistence
        var entity = new ItemEntity
        {
            Title = item.Title,
            Description = item.Description,
            DailyRate = item.DailyRate,
            CategoryId = item.CategoryId,
            OwnerId = item.OwnerId,
            OwnerName = item.OwnerName,
            CreatedAt = item.CreatedAt,
            IsAvailable = true
        };

        _db.Items.Add(entity);
        await _db.SaveChangesAsync();

        await Shell.Current.DisplayAlertAsync("Success", "Item created and saved locally!", "OK");

        // Navigate back to My Items
        await Shell.Current.GoToAsync("..");
    }
}
