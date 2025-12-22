using System.ComponentModel.DataAnnotations;

namespace FireForce.Application.DTOs
{
    public class EquipmentDTO
    {
        public int Id { get; set;}

        [Required(ErrorMessage = "Equipment number is required")]
        [StringLength(50)]
        public string EquipmentNumber { get; set; } = string.Empty;


        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Type is required")]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;


        [Required(ErrorMessage = "Manufacturer is required")]
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;


        [Required(ErrorMessage = "Purchase date is required")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime? LastMaintenanceDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime? NextMaintenenceDatae { get; set; }


        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;


        public int? StationId { get; set; }
        public string? StationName { get; set; }


    }
}