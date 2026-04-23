using System.ComponentModel.DataAnnotations;

namespace RentalApp.Database.Models;

public class Item
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public string ImageUrl { get; set; }

    [Range(0, 9999)]
    public decimal PricePerDay { get; set; }

    public bool IsAvailable { get; set; } = true;
}
