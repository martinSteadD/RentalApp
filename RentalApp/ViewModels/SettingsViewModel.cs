using CommunityToolkit.Mvvm.ComponentModel;
using RentalApp.Services;

namespace RentalApp.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    public SettingsViewModel(
        IAuthenticationService authService,
        INavigationService navigationService)
        : base(authService, navigationService)
    {
        Title = "Settings";
    }
}

