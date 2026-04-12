using System.Text.Json;
using RentalApp.Models;


namespace RentalApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ApiAuthService _apiAuthService;
    private readonly TokenStore _tokenStore;

    public AuthenticationService(ApiAuthService apiAuthService, TokenStore tokenStore)
    {
        _apiAuthService = apiAuthService;
        _tokenStore = tokenStore;
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var response = await _apiAuthService.LoginAsync(request);

        if (response == null)
            return new AuthenticationResult(false, "No response from server");

        if (string.IsNullOrWhiteSpace(response.Token))
            return new AuthenticationResult(false, "Invalid email or password");

        // Save token
        await _tokenStore.SaveTokenAsync(response.Token);

        return new AuthenticationResult(true, "Login successful");
    }

    public async Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        var request = new RegisterRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        var response = await _apiAuthService.RegisterAsync(request);

        // If API returned nothing
        if (response == null)
            return new AuthenticationResult(false, "No response from server");

        // Registration succeeded — API returns the created user object
        return new AuthenticationResult(true, "Registration successful");
    }

    public async Task<UserProfile?> GetCurrentUserAsync()
    {
        var token = await _tokenStore.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return null;

        return await _apiAuthService.GetCurrentUserAsync(token);
    }



    public Task LogoutAsync()
    {
        _tokenStore.ClearToken();
        return Task.CompletedTask;
    }
}
