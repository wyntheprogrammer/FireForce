using System.ComponentModel.DataAnnotations;

namespace FireForce.Application.DTOs
{
    
    public class IncidentDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Incident number is required")]
        [StringLength(50)]
        public string IncidentNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Incident type is required")]
        public string IncidentType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Incident date is required")]
        [DataType(DataType.DateTime)]
        public DateTime IncidentDate { get; set; }
        
        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Severity is required")]
        public string Severity { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;
        
        public int? StationId { get; set; }
        public string? StationName { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? ResponseTime { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? ResolvedTime { get; set; }
    }
}