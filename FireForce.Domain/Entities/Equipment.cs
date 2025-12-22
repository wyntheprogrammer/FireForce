namespace FireForce.Domain.Entities
{
    public class Equipment : BaseEntity
    {
        public string EquipmentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Truck, Hose, Ladder, Protective Gear
        public string Manufacturer { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string Status { get; set; } = string.Empty; // Available, InUse, Maintenance, Decommissioned
        public int? StationId { get; set; }
    }
}