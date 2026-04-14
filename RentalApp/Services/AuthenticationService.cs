using RentalApp.Models;

namespace RentalApp.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ApiAuthService _apiAuthService;
    private readonly TokenStore _tokenStore;

    // ⭐ Forward the CurrentUser property to ApiAuthService
    public UserProfile? CurrentUser => _apiAuthService.CurrentUser;

    public AuthenticationService(ApiAuthService apiAuthService, TokenStore tokenStore)
    {
        _apiAuthService = apiAuthService;
        _tokenStore = tokenStore;
    }

    // LOGIN
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _apiAuthService.LoginAsync(request);

        if (response == null || string.IsNullOrWhiteSpace(response.Token))
            return null;

        await _tokenStore.SaveTokenAsync(response.Token);
        return response;
    }

    // REGISTER
    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        return await _apiAuthService.RegisterAsync(request);
    }

    // LOAD TOKEN ON APP START
    public async Task<bool> LoadSavedTokenAsync()
    {
        var token = await _tokenStore.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return false;

        // ApiAuthService will load CurrentUser internally
        return await _apiAuthService.LoadSavedTokenAsync();
    }

    // GET CURRENT USER
    public async Task<UserProfile?> GetCurrentUserAsync(string token)
    {
        return await _apiAuthService.GetCurrentUserAsync(token);
    }

    // LOGOUT
    public async Task LogoutAsync()
    {
        _tokenStore.ClearToken();
        await _apiAuthService.LogoutAsync();
    }
}
