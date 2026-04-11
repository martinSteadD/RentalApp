using Microsoft.Maui.Storage;

namespace RentalApp.Services;

public class TokenStore
{
    private const string TokenKey = "auth_token";

    public async Task SaveTokenAsync(string token)
    {
        await SecureStorage.SetAsync(TokenKey, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public void ClearToken()
    {
        SecureStorage.Remove(TokenKey);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }
}
