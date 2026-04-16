using System.Text.Json.Serialization;

namespace RentalApp.Models;

public class ItemsResponse
{
    [JsonPropertyName("items")]
    public List<Item> Items { get; set; } = new();

    [JsonPropertyName("total")]
    public int TotalItems { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
