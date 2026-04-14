using Microsoft.Maui.Storage;
using RentalApp.Models;

namespace RentalApp.Services;

public class ApiAuthService : IAuthenticationService
{
    private readonly ApiClient _apiClient;

    // ⭐ This is the logged-in user available to all ViewModels
    public UserProfile? CurrentUser { get; private set; }

    public ApiAuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // ⭐ LOGIN
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _apiClient.PostAsync<LoginRequest, LoginResponse>("auth/token", request);

        if (response == null || string.IsNullOrWhiteSpace(response.Token))
            return null;

        // Save token
        await SecureStorage.SetAsync("auth_token", response.Token);
        await _apiClient.SetTokenAsync(response.Token);

        // Load user profile from /users/me
        CurrentUser = await GetCurrentUserAsync(response.Token);

        return response;
    }

    // ⭐ REGISTER
    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        return await _apiClient.PostAsync<RegisterRequest, RegisterResponse>("auth/register", request);
    }

    // ⭐ LOAD TOKEN ON APP START
    public async Task<bool> LoadSavedTokenAsync()
    {
        var token = await SecureStorage.GetAsync("auth_token");

        if (string.IsNullOrWhiteSpace(token))
            return false;

        await _apiClient.SetTokenAsync(token);

        // Load user profile
        CurrentUser = await GetCurrentUserAsync(token);

        return CurrentUser != null;
    }

    // ⭐ GET CURRENT USER FROM /users/me
    public async Task<UserProfile?> GetCurrentUserAsync(string token)
    {
        await _apiClient.SetTokenAsync(token);

        try
        {
            return await _apiClient.GetAsync<UserProfile>("users/me");
        }
        catch
        {
            return null;
        }
    }

    // ⭐ LOGOUT
    public async Task LogoutAsync()
    {
        SecureStorage.Remove("auth_token");
        await _apiClient.SetTokenAsync(null);

        CurrentUser = null;
    }
}
