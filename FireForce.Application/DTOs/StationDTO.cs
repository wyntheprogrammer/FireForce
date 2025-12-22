using System.ComponentModel.DataAnnotations;

namespace FireForce.Application.DTOs
{
    public class StationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Station number is required")]
        [StringLength(20)]
        public string StationNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Addres is required")]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;


        [Required(ErrorMessage = "City is required")]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } =  string.Empty;


        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 1000)]
        public int Capacity  { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;
    }
}