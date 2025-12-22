namespace FireForce.Domain.Entities
{
    public class Station : BaseEntity
    {
        public string StationNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = string.Empty; // Active, Maintenance, Closed
    }
}