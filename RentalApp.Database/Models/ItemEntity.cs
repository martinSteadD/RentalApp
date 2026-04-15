using System.ComponentModel.DataAnnotations;

namespace RentalApp.Database.Models;

public class ItemEntity
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DailyRate { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    
    public bool IsAvailable { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
}
