using RentalApp.Helpers;

namespace RentalApp.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetAsync(string endpoint)
    {
        await Task.Delay(100); // simulate network delay

        return $"Simulated GET {endpoint} with token: {TokenStore.JwtToken}";
    }
}
