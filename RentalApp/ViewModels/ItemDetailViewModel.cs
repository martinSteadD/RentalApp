using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

[QueryProperty(nameof(Item), "Item")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private Item item;

    partial void OnItemChanged(Item value)
    {
        if (value != null)
            Title = value.Title; // Update page title dynamically
    }

    public ItemDetailViewModel(
        IItemService itemService,
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        Title = "Item Details";
    }

    [RelayCommand]
    private async Task RentItemAsync()
    {
        await Shell.Current.DisplayAlert("Rent", "Renting is not implemented yet.", "OK");
    }
}
