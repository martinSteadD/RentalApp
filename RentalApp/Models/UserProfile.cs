namespace RentalApp.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public double? AverageRating { get; set; }
        public int ItemsListed { get; set; }
        public int RentalsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
