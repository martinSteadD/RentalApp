using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

[ObservableProperty] private string dailyRate = string.Empty;
[ObservableProperty] private string categoryId = string.Empty;
[ObservableProperty] private string title = string.Empty;
[ObservableProperty] private string description = string.Empty;
[ObservableProperty] private string category = string.Empty;


    public CreateItemViewModel(
        IItemService itemService,
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        Title = "Create Item";
    }

    [RelayCommand]
    private async Task CreateItemAsync()
    {
        if (string.IsNullOrWhiteSpace(Title) ||
            string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(DailyRate) ||
            string.IsNullOrWhiteSpace(CategoryId))
        {
            await Shell.Current.DisplayAlert("Error", "Please fill in all required fields.", "OK");
            return;
        }

        var item = new Item
        {
            Title = Title,
            Description = Description,
            DailyRate = decimal.Parse(DailyRate),
            CategoryId = int.Parse(CategoryId),
            Category = Category,
            OwnerId = CurrentUser.Id,
            OwnerName = $"{CurrentUser!.FirstName} {CurrentUser!.LastName}",
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _itemService.CreateItemAsync(item);

        if (result == null)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to create item.", "OK");
            return;
        }

        await Shell.Current.DisplayAlert("Success", "Item created successfully!", "OK");

        // Navigate back to My Items
        await Shell.Current.GoToAsync("..");
    }
}
