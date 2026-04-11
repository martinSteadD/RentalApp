using Microsoft.Maui.Storage;

namespace RentalApp.Services;

public class ApiAuthService
{
    private readonly ApiClient _apiClient;

    public ApiAuthService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // LOGIN
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        return await _apiClient.PostAsync<LoginRequest, LoginResponse>("auth/token", request);
    }


    // REGISTER
    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        return await _apiClient.PostAsync<RegisterRequest, RegisterResponse>("auth/register", request);
    }


    // LOAD TOKEN ON APP START
    public async Task<bool> LoadSavedTokenAsync()
    {
        var token = await SecureStorage.GetAsync("auth_token");

        if (string.IsNullOrWhiteSpace(token))
            return false;

        await _apiClient.SetTokenAsync(token);
        return true;
    }

    // LOGOUT
    public async Task LogoutAsync()
    {
        SecureStorage.Remove("auth_token");
        await _apiClient.SetTokenAsync(null);
    }
}
