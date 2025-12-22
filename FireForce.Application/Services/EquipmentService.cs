using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using System.Text.Json;

namespace FireForce.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;

        public EquipmentService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }

        public async Task<EquipmentDTO?> GetByIdAsync(int id)
        {
            var equipment = await _unitOfWork.Equipment.GetByIdAsync(id);
            return equipment != null ? await MapToDTO(equipment) : null;
        }

        public async Task<IEnumerable<EquipmentDTO>> GetAllAsync()
        {
            var equipments = await _unitOfWork.Equipment.GetAllAsync();
            var dtoList = new List<EquipmentDTO>();

            foreach (var equipment in equipments)
            {
                dtoList.Add(await MapToDTO(equipment));
            }

            return dtoList;
        }

        public async Task<int> CreateAsync(EquipmentDTO dto, string currentUser)
        {
            var equipment = MapToEntity(dto);
            equipment.CreatedBy = currentUser;

            var id = await _unitOfWork.Equipment.AddAsync(equipment);

            await _auditLogService.LogChangeAsync(
                "Equipments", "INSERT", id,
                "", JsonSerializer.Serialize(dto), currentUser);

            return id;
        }

        public async Task<bool> UpdateAsync(EquipmentDTO dto, string currentUser)
        {
            var existing = await _unitOfWork.Equipment.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(await MapToDTO(existing));

            var equipment = MapToEntity(dto);
            equipment.CreatedAt = existing.CreatedAt;
            equipment.CreatedBy = existing.CreatedBy;
            equipment.UpdatedBy = currentUser;

            var result = await _unitOfWork.Equipment.UpdateAsync(equipment);

            if (result)
            {
                await _auditLogService.LogChangeAsync(
                    "Equipments", "UPDATE", dto.Id,
                    oldValue, JsonSerializer.Serialize(dto), currentUser);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int id, string currentUser)
        {
            var existing = await _unitOfWork.Equipment.GetByIdAsync(id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(await MapToDTO(existing));

            var result = await _unitOfWork.Equipment.SoftDeleteAsync(id);

            if (result)
            {
                await _auditLogService.LogChangeAsync(
                    "Equipments", "DELETE", id,
                    oldValue, "", currentUser);
            }

            return result;
        }

        /* ================= INTERFACE-SPECIFIC METHODS ================= */

        public async Task<EquipmentDTO?> GetByEquipmentNumberAsync(string equipmentNumber)
        {
            var equipment = await _unitOfWork.Equipment.GetByEquipmentNumberAsync(equipmentNumber);
            return equipment != null ? await MapToDTO(equipment) : null;
        }

        public async Task<IEnumerable<EquipmentDTO>> GetByStationIdAsync(int stationId)
        {
            var equipments = await _unitOfWork.Equipment.GetByStationIdAsync(stationId);
            var dtoList = new List<EquipmentDTO>();

            foreach (var equipment in equipments)
            {
                dtoList.Add(await MapToDTO(equipment));
            }

            return dtoList;
        }

        /* ================= MAPPING ================= */

        private async Task<EquipmentDTO> MapToDTO(Equipment entity)
        {
            var dto = new EquipmentDTO
            {
                Id = entity.Id,
                EquipmentNumber = entity.EquipmentNumber,
                Name = entity.Name,
                Type = entity.Type,
                Manufacturer = entity.Manufacturer,
                PurchaseDate = entity.PurchaseDate,
                LastMaintenanceDate = entity.LastMaintenanceDate,
                Status = entity.Status,
                StationId = entity.StationId
            };

            if (entity.StationId.HasValue)
            {
                var station = await _unitOfWork.Stations.GetByIdAsync(entity.StationId.Value);
                dto.StationName = station?.Name;
            }

            return dto;
        }

        private Equipment MapToEntity(EquipmentDTO dto)
        {
            return new Equipment
            {
                Id = dto.Id,
                EquipmentNumber = dto.EquipmentNumber,
                Name = dto.Name,
                Type = dto.Type,
                Manufacturer = dto.Manufacturer,
                PurchaseDate = dto.PurchaseDate,
                LastMaintenanceDate = dto.LastMaintenanceDate,
                Status = dto.Status,
                StationId = dto.StationId
            };
        }
    }
}
