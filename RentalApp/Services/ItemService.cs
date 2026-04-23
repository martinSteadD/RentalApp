using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RentalApp.Models;
using RentalApp.Services;

public class ItemService : IItemService
{
    private readonly HttpClient _httpClient;
    private readonly TokenStore _tokenStore;
    private readonly ApiClient _apiClient;
    private readonly IAuthenticationService _authService;
    public ItemService(HttpClient httpClient, TokenStore tokenStore, ApiClient apiClient, IAuthenticationService authService)
    {
        _httpClient = httpClient;
        _tokenStore = tokenStore;
        _apiClient = apiClient;
        _authService = authService;
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

        public async Task<List<Item>> GetItemsAsync(int page = 1, int pageSize = 100)
    {
        await AddJwtHeaderAsync();

        var response = await _httpClient.GetAsync($"/items?page={page}&pageSize={pageSize}");

        Console.WriteLine($"DEBUG: GetItemsAsync response status: {response.StatusCode}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"DEBUG: Items JSON: {json}");

        var result = JsonSerializer.Deserialize<ItemsResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.Items ?? new List<Item>();
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

        // Use ApiClient so POST is logged and errors are visible
        return await _apiClient.PostAsync<Item, Item>("/items", item);
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

    public async Task<List<Item>> GetMyItemsAsync()
    {
        await AddJwtHeaderAsync();

        // Get ALL items from the API
        var allItems = await GetItemsAsync();

        var userId = _authService.CurrentUser!.Id;

        // Filter client-side
        return allItems
            .Where(i => i.OwnerId == userId)
            .ToList();
    }


}
