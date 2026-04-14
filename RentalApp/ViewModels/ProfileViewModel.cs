/// @file ProfileViewModel.cs
/// @brief User profile management view model
/// @author RentalApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Services;
using RentalApp.Models;

namespace RentalApp.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly TokenStore _tokenStore;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    [NotifyPropertyChangedFor(nameof(DisplayRating))]
    private UserProfile? currentUser;

    [ObservableProperty]
    private string currentPassword = string.Empty;

    [ObservableProperty]
    private string newPassword = string.Empty;

    [ObservableProperty]
    private string confirmNewPassword = string.Empty;

    [ObservableProperty]
    private bool isChangingPassword;

    public string FullName =>
        $"{CurrentUser?.FirstName} {CurrentUser?.LastName}".Trim();

    public string DisplayRating =>
        CurrentUser?.AverageRating.HasValue == true
            ? $"Rating: {CurrentUser.AverageRating:0.0}"
            : "Rating: N/A";

    public ProfileViewModel(
        IAuthenticationService authService,
        INavigationService navigationService,
        TokenStore tokenStore)
        : base(authService, navigationService)
    {
        _tokenStore = tokenStore;
        Title = "Profile";

        _ = LoadUserDataAsync();
    }


    private async Task LoadUserDataAsync()
    {
        try
        {
            var token = await _tokenStore.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "You are not logged in.",
                    "OK");
                return;
            }

            var user = await _authService.GetCurrentUserAsync(token);

            if (user is null)
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "Unable to load your profile information.",
                    "OK");
                return;
            }

            CurrentUser = user;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Error",
                $"Failed to load profile: {ex.Message}",
                "OK");
        }
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        await Shell.Current.DisplayAlert(
            "Not Available",
            "Password change is not supported in this version of the app.",
            "OK");
    }

    [RelayCommand]
    private void TogglePasswordChangeMode()
    {
        IsChangingPassword = !IsChangingPassword;
        if (!IsChangingPassword)
        {
            ClearPasswordFields();
            ClearError();
        }
    }

    [RelayCommand]
    private async Task NavigateBackAsync()
    {
        await Shell.Current.GoToAsync("..");

    }

    private bool ValidatePasswordChange()
    {
        if (string.IsNullOrWhiteSpace(CurrentPassword))
        {
            SetError("Current password is required");
            return false;
        }

        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            SetError("New password is required");
            return false;
        }

        if (NewPassword.Length < 6)
        {
            SetError("New password must be at least 6 characters long");
            return false;
        }

        if (NewPassword != ConfirmNewPassword)
        {
            SetError("New passwords do not match");
            return false;
        }

        if (CurrentPassword == NewPassword)
        {
            SetError("New password must be different from current password");
            return false;
        }

        return true;
    }

    private void ClearPasswordFields()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmNewPassword = string.Empty;
    }
}
