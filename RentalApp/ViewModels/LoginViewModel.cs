/// @file LoginViewModel.cs
/// @brief Login page view model for user authentication
/// @author RentalApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Services;

namespace RentalApp.ViewModels;

/// @brief View model for the login page that handles user authentication
/// @details Manages login form data, validation, and authentication process
/// @extends BaseViewModel
public partial class LoginViewModel : BaseViewModel
{
    /// @brief The user's email address
    /// @details Observable property bound to the email input field
    [ObservableProperty]
    private string email = string.Empty;

    /// @brief The user's password
    /// @details Observable property bound to the password input field
    [ObservableProperty]
    private string password = string.Empty;

    /// @brief Whether to remember the user's login credentials
    /// @details Observable property bound to the remember me checkbox
    [ObservableProperty]
    private bool rememberMe;

    /// @brief Default constructor for design-time support
    public LoginViewModel()
        : base(null!, null!) // design-time only, not used at runtime
    {
        Title = "Login";
    }

    /// @brief Initializes a new instance of the LoginViewModel class
    /// @param authService The authentication service instance
    /// @param navigationService The navigation service instance
    public LoginViewModel(IAuthenticationService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        Title = "Login";
    }

    /// @brief Performs user login authentication
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

            var result = await _authService.LoginAsync(Email, Password);

            if (result.IsSuccess)
            {
                await _navigationService.NavigateToAsync("MainPage");
            }
            else
            {
                SetError(result.Message);
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

    /// @brief Navigates to the registration page
    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        await _navigationService.NavigateToAsync("RegisterPage");
    }

    /// @brief Handles forgot password functionality
    [RelayCommand]
    private async Task ForgotPasswordAsync()
    {
        await Application.Current.MainPage.DisplayAlert(
            "Info",
            "Forgot password functionality not implemented yet",
            "OK");
    }
}
