using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentalApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RentalApp.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly IAuthenticationService _authService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<MenuBarItem> DynamicMenuBarItems { get; } = new();

        public AppShellViewModel()
        {
            Title = "RentalApp";
        }

        public AppShellViewModel(IAuthenticationService authService, INavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            Title = "RentalApp";
        }

        // TEMPORARY: Always allow actions until real role logic is added
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
            await _navigationService.NavigateToAsync("TempPage");
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAuthenticatedAction))]
        private async Task LogoutAsync()
        {
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync("LoginPage");

            LogoutCommand.NotifyCanExecuteChanged();
            NavigateToProfileCommand.NotifyCanExecuteChanged();
            NavigateToSettingsCommand.NotifyCanExecuteChanged();
        }
    }
}
