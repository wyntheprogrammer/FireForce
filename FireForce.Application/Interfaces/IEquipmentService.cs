using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
    public interface IEquipmentService : IGenericService<EquipmentDTO>
    {
        Task<EquipmentDTO?> GetByEquipmentNumberAsync(string equipmentNumber);
        Task<IEnumerable<EquipmentDTO>> GetByStationIdAsync(int stationId);
    }
}