using System.ComponentModel;
using System.Windows.Input;
using RentalApp.Models;
using RentalApp.Services;

namespace RentalApp.ViewModels;

[QueryProperty(nameof(UserId), "userId")]
public partial class UserDetailViewModel : BaseViewModel

{
    private readonly ApiClient _apiClient;

    private const string AdminEmail = "xxxxx@rentalapp.com";

    private int _userId;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    private string _role = string.Empty;
    private bool _isLoading;

    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnPropertyChanged();
            _ = LoadUserAsync();
        }
    }

    public string FirstName
    {
        get => _firstName;
        set { _firstName = value; OnPropertyChanged(); }
    }

    public string LastName
    {
        get => _lastName;
        set { _lastName = value; OnPropertyChanged(); }
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public string Role
    {
        get => _role;
        set { _role = value; OnPropertyChanged(); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ICommand BackCommand { get; }

    public UserDetailViewModel(
        ApiClient apiClient,
        IAuthenticationService authService,
        INavigationService navigationService
    ) : base(authService, navigationService)
    {
        _apiClient = apiClient;

        BackCommand = new Command(async () => await _navigationService.NavigateToAsync("UserListPage"));

    }

    private async Task LoadUserAsync()
    {
        if (UserId <= 0) return;

        IsLoading = true;

        try
        {
            var user = await _apiClient.GetAsync<User>($"users/{UserId}");

            if (user == null)
                return;

            // Split full name
            var parts = user.Name?.Split(' ', 2) ?? new string[] { "" };
            FirstName = parts.Length > 0 ? parts[0] : "";
            LastName = parts.Length > 1 ? parts[1] : "";

            Email = user.Email;

            // Client-side role assignment
            Role = user.Email == AdminEmail ? "Admin" : "User";
        }
        catch (Exception ex)
        {
            // Optional: add error message property if needed
            Console.WriteLine($"Error loading user: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}
