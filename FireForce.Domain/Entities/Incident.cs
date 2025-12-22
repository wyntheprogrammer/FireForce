namespace FireForce.Domain.Entities
{
    public class Incident : BaseEntity
    {
        public string IncidentNumber { get; set; } = string.Empty;
        public string IncidentType { get; set; } = string.Empty; // Fire, Medical, Rescue, Hazmat
        public DateTime IncidentDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty; // Low, Medium, High, Critical
        public string Status { get; set; } = string.Empty; // Reported, InProgress, Resolved, Closed
        public int? StationId { get; set; }
        public DateTime? ResponseTime { get; set; }
        public DateTime? ResolvedTime { get; set; }
    }
}