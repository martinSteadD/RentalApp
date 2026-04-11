using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

namespace RentalApp.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev/")
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task SetTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        var raw = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API RAW RESPONSE (GET): " + raw);
        System.Diagnostics.Debug.WriteLine("API RAW RESPONSE (GET): " + raw);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"GET {endpoint} failed: {response.StatusCode} - {raw}");

        return JsonSerializer.Deserialize<T>(raw, _jsonOptions);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        // Create the request manually so we can add headers
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(data)
        };

        // Add a custom User-Agent header to help Cloudflare accept the request
        request.Headers.UserAgent.ParseAdd("RentalApp/1.0");

        var response = await _httpClient.SendAsync(request);

        // Read raw response BEFORE doing anything else
        var raw = await response.Content.ReadAsStringAsync();

        // Log raw response ALWAYS
        Console.WriteLine("API RAW RESPONSE (POST): " + raw);
        System.Diagnostics.Debug.WriteLine("API RAW RESPONSE (POST): " + raw);

        // If HTTP status code is not success, throw with raw content
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"POST {endpoint} failed: {response.StatusCode} - {raw}");
        }

        // Try to deserialize — if it fails, you'll still have the raw response above
        return JsonSerializer.Deserialize<TResponse>(raw, _jsonOptions);
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(data, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PutAsync(endpoint, jsonContent);
        var raw = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API RAW RESPONSE (PUT): " + raw);
        System.Diagnostics.Debug.WriteLine("API RAW RESPONSE (PUT): " + raw);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"PUT {endpoint} failed: {response.StatusCode} - {raw}");

        return JsonSerializer.Deserialize<T>(raw, _jsonOptions);
    }

    public async Task<T?> PatchAsync<T>(string endpoint, object data)
    {
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(data, _jsonOptions),
            Encoding.UTF8,
            "application/json");

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
        {
            Content = jsonContent
        };

        var response = await _httpClient.SendAsync(request);
        var raw = await response.Content.ReadAsStringAsync();

        Console.WriteLine("API RAW RESPONSE (PATCH): " + raw);
        System.Diagnostics.Debug.WriteLine("API RAW RESPONSE (PATCH): " + raw);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"PATCH {endpoint} failed: {response.StatusCode} - {raw}");

        return JsonSerializer.Deserialize<T>(raw, _jsonOptions);
    }
}
