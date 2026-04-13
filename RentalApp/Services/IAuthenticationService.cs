using RentalApp.Models;

namespace RentalApp.Services;

public interface IAuthenticationService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<RegisterResponse?> RegisterAsync(RegisterRequest request);
    Task<bool> LoadSavedTokenAsync();
    Task<UserProfile?> GetCurrentUserAsync(string token);
    Task LogoutAsync();
}
