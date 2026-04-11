using RentalApp.Helpers;

namespace RentalApp.Services;

public class ApiAuthService
{
    public async Task<bool> LoginAsync(string username, string password)
    {
        // TODO: Replace with real API call later
        await Task.Delay(100); // simulate network delay

        TokenStore.JwtToken = "dummy-token";
        return true;
    }

    public void Logout()
    {
        TokenStore.JwtToken = null;
    }
}
