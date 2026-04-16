using System.Text.Json.Serialization;

namespace RentalApp.Models;

public class Item
{
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("dailyRate")]
    public decimal? DailyRate { get; set; }

    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("ownerId")]
    public int? OwnerId { get; set; }

    [JsonPropertyName("owner")]
    public string? OwnerName { get; set; }

    [JsonPropertyName("ownerRating")]
    public double? OwnerRating { get; set; }

    [JsonPropertyName("available")]
    public bool? IsAvailable { get; set; }

    [JsonPropertyName("rating")]
    public double? AverageRating { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}
