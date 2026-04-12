using RentalApp.Models;


namespace RentalApp.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password);
    Task<UserProfile?> GetCurrentUserAsync();
    Task LogoutAsync();
}
