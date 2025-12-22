namespace FireForce.Domain.Entities
{
    public class Firefighter : BaseEntity
    {
        public string BadgeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string Status { get; set; } = string.Empty; // Active, OnLeave, Retired
        public int? StationId { get; set; }
    }
}