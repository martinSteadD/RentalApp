/// @file RegisterViewModel.cs
/// @brief User registration view model
/// @author RentalApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Services;
using System.Text.RegularExpressions;

namespace RentalApp.ViewModels;

/// @brief View model for the user registration page
/// @details Manages user registration form data, validation, and registration process
/// @extends BaseViewModel
public partial class RegisterViewModel : BaseViewModel
{
    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private bool acceptTerms;

    /// @brief Default constructor for design-time support
    public RegisterViewModel()
        : base(null!, null!) // design-time only
    {
        Title = "Register";
    }

    /// @brief Runtime constructor
    public RegisterViewModel(IAuthenticationService authService, INavigationService navigationService)
        : base(authService, navigationService)
    {
        Title = "Register";
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateForm())
            return;

        try
        {
            IsBusy = true;
            ClearError();

            var result = await _authService.RegisterAsync(FirstName, LastName, Email, Password);

            if (result.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Success",
                    "Registration successful! Please login.",
                    "OK");

                await _navigationService.NavigateBackAsync();
            }
            else
            {
                SetError(result.Message);
            }
        }
        catch (Exception ex)
        {
            SetError($"Registration failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateBackToLoginAsync()
    {
        await _navigationService.NavigateBackAsync();
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            SetError("First name is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            SetError("Last name is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            SetError("Email is required");
            return false;
        }

        if (!IsValidEmail(Email))
        {
            SetError("Please enter a valid email address");
            return false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            SetError("Password is required");
            return false;
        }

        if (Password.Length < 6)
        {
            SetError("Password must be at least 6 characters long");
            return false;
        }

        if (Password != ConfirmPassword)
        {
            SetError("Passwords do not match");
            return false;
        }

        if (!AcceptTerms)
        {
            SetError("Please accept the terms and conditions");
            return false;
        }

        return true;
    }

    private static bool IsValidEmail(string email)
    {
        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
    }
}
