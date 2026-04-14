using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Services;
using System.Collections.ObjectModel;

namespace RentalApp.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        public ObservableCollection<MenuBarItem> DynamicMenuBarItems { get; } = new();

        // Constructor for design-time support (optional)
        public AppShellViewModel()
            : base(null!, null!) // Not used at runtime
        {
            Title = "RentalApp";
        }

        // Runtime constructor
        public AppShellViewModel(IAuthenticationService authService, INavigationService navigationService)
            : base(authService, navigationService)
        {
            Title = "RentalApp";
        }

        // Always allow actions until real role logic is added
        private bool CanExecuteGuestAction() => true;
        private bool CanExecuteUserAction() => true;
        private bool CanExecuteAdminAction() => true;

        // TEMPORARY: Always allow authenticated actions until token-based state is added
        private bool CanExecuteAuthenticatedAction() => true;

        [RelayCommand]
        private async Task NavigateToProfileAsync()
        {
            await _navigationService.NavigateToAsync("TempPage");
        }

        [RelayCommand]
        private async Task NavigateToSettingsAsync()
        {
            await _navigationService.NavigateToAsync("SettingsPage");

        }

    }
}
