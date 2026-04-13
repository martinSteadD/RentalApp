/// @file LoginViewModel.cs
/// @brief Login page view model for user authentication
/// @author RentalApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Services;
using RentalApp.Models;

namespace RentalApp.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool rememberMe;

    public LoginViewModel()
        : base(null!, null!)
    {
        Title = "Login";
    }

    public LoginViewModel(IAuthenticationService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            SetError("Please enter both email and password");
            return;
        }

        try
        {
            IsBusy = true;
            ClearError();

            var request = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var result = await _authService.LoginAsync(request);

            if (result != null && !string.IsNullOrWhiteSpace(result.Token))
            {
                await SecureStorage.SetAsync("auth_token", result.Token);
                await _authService.LoadSavedTokenAsync();
                await _navigationService.NavigateToAsync("MainPage");
            }
            else
            {
                SetError("Invalid email or password");
            }
        }
        catch (Exception ex)
        {
            SetError($"Login failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        await _navigationService.NavigateToAsync("RegisterPage");
    }

    [RelayCommand]
    private async Task ForgotPasswordAsync()
    {
        await Shell.Current.DisplayAlert(
            "Info",
            "Forgot password functionality not implemented yet",
            "OK");
    }
}
