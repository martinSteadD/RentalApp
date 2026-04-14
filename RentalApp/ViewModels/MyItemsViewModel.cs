using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

public partial class MyItemsViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private List<Item> myItems = new();

    public MyItemsViewModel(
        IItemService itemService,
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        Title = "My Items";

        _ = LoadMyItemsAsync();
    }

    [RelayCommand]
    public async Task LoadMyItemsAsync()
    {
        try
        {
            IsBusy = true;

            var allItems = await _itemService.GetItemsAsync();
            var userId = CurrentUser.Id;

            MyItems = allItems
                .Where(i => i.OwnerId == userId)
                .ToList();
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
    private async Task CreateItemAsync()
    {
        await Shell.Current.GoToAsync("CreateItemPage");
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
