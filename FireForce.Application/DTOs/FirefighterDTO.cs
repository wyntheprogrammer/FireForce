using System.ComponentModel.DataAnnotations;

namespace FireForce.Application.DTOs
{
    public class FirefighterDTO
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Badge number is required")]
        [StringLength(20)]
        public string BadgeNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Rank is required")]
        [StringLength(50)]
        public string Rank { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Hire date is required")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;
        
        public int? StationId { get; set; }
        public string? StationName { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}