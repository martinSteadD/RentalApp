using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RentalApp.Models;
using RentalApp.Services;

public class ItemService : IItemService
{
    private readonly HttpClient _httpClient;
    private readonly TokenStore _tokenStore;

    public ItemService(HttpClient httpClient, TokenStore tokenStore)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
    }

    private async Task AddJwtHeaderAsync()
    {
        var token = await _tokenStore.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        await AddJwtHeaderAsync();

        var response = await _httpClient.GetAsync("/items");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Item>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        await AddJwtHeaderAsync();

        var response = await _httpClient.GetAsync($"/items/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Item>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<Item?> CreateItemAsync(Item item)
    {
        await AddJwtHeaderAsync();

        var json = JsonSerializer.Serialize(item);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/items", content);
        if (!response.IsSuccessStatusCode) return null;

        var resultJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Item>(resultJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<bool> UpdateItemAsync(Item item)
    {
        await AddJwtHeaderAsync();

        var json = JsonSerializer.Serialize(item);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"/items/{item.Id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        await AddJwtHeaderAsync();

        var response = await _httpClient.DeleteAsync($"/items/{id}");
        return response.IsSuccessStatusCode;
    }
}
