using System.Collections.ObjectModel;
using System.Windows.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels
{
    public class UserListViewModel : BaseViewModel
    {
        private readonly ApiClient _apiClient;

        private const string AdminEmail = "xxxxx@rentalapp.com";

        public ObservableCollection<UserDisplayModel> Users { get; set; } = new();
        public ObservableCollection<UserDisplayModel> FilteredUsers { get; set; } = new();

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    ApplyFilter();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand UserSelectedCommand { get; }

        public UserListViewModel(
            ApiClient apiClient,
            IAuthenticationService authService,
            INavigationService navigationService
        ) : base(authService, navigationService)
        {
            _apiClient = apiClient;

            RefreshCommand = new Command(async () => await LoadUsers());
            UserSelectedCommand = new Command<UserDisplayModel>(OnUserSelected);
        }


        public async Task LoadUsers()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var apiUsers = await _apiClient.GetAsync<List<User>>("users");

                Users.Clear();

                foreach (var u in apiUsers)
                {
                    // Split full name into first + last
                    var parts = u.Name?.Split(' ', 2) ?? new string[] { "" };
                    var firstName = parts.Length > 0 ? parts[0] : "";
                    var lastName = parts.Length > 1 ? parts[1] : "";

                    Users.Add(new UserDisplayModel
                    {
                        Id = u.Id,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = u.Email,
                        Role = u.Email == AdminEmail ? "Admin" : "User"
                    });
                }

                ApplyFilter();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredUsers = new ObservableCollection<UserDisplayModel>(Users);
            }
            else
            {
                var lower = SearchText.ToLower();

                FilteredUsers = new ObservableCollection<UserDisplayModel>(
                    Users.Where(u =>
                        u.FirstName.ToLower().Contains(lower) ||
                        u.LastName.ToLower().Contains(lower) ||
                        u.Email.ToLower().Contains(lower))
                );
            }

            OnPropertyChanged(nameof(FilteredUsers));
        }

        private async void OnUserSelected(UserDisplayModel user)
        {
            if (user == null) return;

            // Navigate to user detail page (you already have this set up)
            await Shell.Current.GoToAsync($"UserDetailPage?userId={user.Id}");
        }
    }

    public class UserDisplayModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string DisplayName => $"{FirstName} {LastName}";
        public string DisplayEmail => Email;
    }
}
