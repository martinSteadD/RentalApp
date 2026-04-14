using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

public partial class BrowseItemsViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private List<Item> items = new();

    public BrowseItemsViewModel(
        IItemService itemService,
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        Title = "Browse Items";

        _ = LoadItemsAsync();
    }

    private async Task LoadItemsAsync()
    {
        try
        {
            IsBusy = true;
            Items = await _itemService.GetItemsAsync();
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
    private async Task SelectItemAsync(Item item)
    {
        if (item == null)
            return;

        await Shell.Current.GoToAsync("ItemDetailPage", true, new Dictionary<string, object>
        {
            { "Item", item }
        });
    }
}
