using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly TokenStore _tokenStore;

    [ObservableProperty]
    private UserProfile? currentUser;

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [ObservableProperty]
    private bool isAdmin;

    public MainViewModel(
        IItemService itemService,
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore)
        : base(authService, navigationService)
    {
        _itemService = itemService;
        _tokenStore = tokenStore;

        Title = "Dashboard";

        // Fire-and-forget safely
        _ = LoadUserDataAsync();
    }

    private async Task LoadUserDataAsync()
    {
        try
        {
            var token = await _tokenStore.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                WelcomeMessage = "Welcome!";
                CurrentUser = null;
                IsAdmin = false;
                return;
            }

            var user = await _authService.GetCurrentUserAsync(token);

            if (user is null)
            {
                WelcomeMessage = "Welcome!";
                CurrentUser = null;
                IsAdmin = false;
                return;
            }

            CurrentUser = user;
            WelcomeMessage = $"Welcome, {user.FirstName}!";

            IsAdmin = user.Email.Equals("xxxxx@rentalapp.com", StringComparison.OrdinalIgnoreCase);

        }
        catch (Exception ex)
        {
            SetError($"Failed to load user data: {ex.Message}");
            WelcomeMessage = "Welcome!";
            CurrentUser = null;
            IsAdmin = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToProfileAsync()
    {
        await Shell.Current.GoToAsync("ProfilePage");
    }

    [RelayCommand]
    private async Task NavigateToSettingsAsync()
    {
        await Shell.Current.GoToAsync("//SettingsPage");
    }

    [RelayCommand]
    private async Task NavigateToBrowseItemsAsync()
    {
        await Shell.Current.GoToAsync("BrowseItemsPage");
    }
    [RelayCommand]
    private async Task NavigateToMyItemsAsync()
    {
        await Shell.Current.GoToAsync("MyItemsPage");
    }



    [RelayCommand]
    private async Task NavigateToUserListAsync()
    {
        if (!IsAdmin)
        {
            await Shell.Current.DisplayAlertAsync(
                "Access Denied",
                "You don't have permission to access admin features.",
                "OK");
            return;
        }

        await Shell.Current.GoToAsync("UserListPage");
    }

    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        try
        {
            IsBusy = true;
            await LoadUserDataAsync();
        }
        catch (Exception ex)
        {
            SetError($"Failed to refresh data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
