using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private UserProfile? currentUser;

    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    [ObservableProperty]
    private bool isAdmin;

    public MainViewModel(IAuthenticationService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        Title = "Dashboard";
        LoadUserData();
    }

    private async void LoadUserData()
    {
        try
        {
            UserProfile? user = await _authService.GetCurrentUserAsync();

            if (user is null)
            {
                WelcomeMessage = "Welcome!";
                CurrentUser = null;
                IsAdmin = false;
                return;
            }

            CurrentUser = user;
            WelcomeMessage = $"Welcome, {user.FirstName}!";

            // TODO: Add role support when API includes it
            IsAdmin = false;
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
        await _navigationService.NavigateToAsync("TempPage");
    }

    [RelayCommand]
    private async Task NavigateToUserListAsync()
    {
        if (!IsAdmin)
        {
            await Shell.Current.DisplayAlert(
                "Access Denied",
                "You don't have permission to access admin features.",
                "OK");
            return;
        }

        await _navigationService.NavigateToAsync("UserListPage");
    }

    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        try
        {
            IsBusy = true;
            LoadUserData();
            await Task.Delay(1000);
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
