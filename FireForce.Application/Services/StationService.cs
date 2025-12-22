using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using System.Text.Json;

namespace FireForce.Application.Services
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;

        public StationService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }

        public async Task<StationDTO?> GetByIdAsync(int id)
        {
            var station = await _unitOfWork.Stations.GetByIdAsync(id);
            return station != null ? MapToDTO(station) : null;
        }

        public async Task<IEnumerable<StationDTO>> GetAllAsync()
        {
            var stations = await _unitOfWork.Stations.GetAllAsync();
            var dtoList = new List<StationDTO>();

            foreach (var station in stations)
            {
                dtoList.Add(MapToDTO(station));
            }

            return dtoList;
        }

        public async Task<int> CreateAsync(StationDTO dto, string currentUser)
        {
            var station = MapToEntity(dto);
            station.CreatedBy = currentUser;

            var id = await _unitOfWork.Stations.AddAsync(station);

            await _auditLogService.LogChangeAsync(
                "Stations", "INSERT", id,
                "", JsonSerializer.Serialize(dto), currentUser);

            return id;
        }

        public async Task<bool> UpdateAsync(StationDTO dto, string currentUser)
        {
            var existing = await _unitOfWork.Stations.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(MapToDTO(existing));

            var station = MapToEntity(dto);
            station.CreatedAt = existing.CreatedAt;
            station.CreatedBy = existing.CreatedBy;
            station.UpdatedBy = currentUser;

            var result = await _unitOfWork.Stations.UpdateAsync(station);

            if (result)
            {
                await _auditLogService.LogChangeAsync(
                    "Stations", "UPDATE", dto.Id,
                    oldValue, JsonSerializer.Serialize(dto), currentUser);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int id, string currentUser)
        {
            var existing = await _unitOfWork.Stations.GetByIdAsync(id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(MapToDTO(existing));

            var result = await _unitOfWork.Stations.SoftDeleteAsync(id);

            if (result)
            {
                await _auditLogService.LogChangeAsync(
                    "Stations", "DELETE", id,
                    oldValue, "", currentUser);
            }

            return result;
        }

        public async Task<StationDTO?> GetByStationNumberAsync(string stationNumber)
        {
            var station = await _unitOfWork.Stations.GetByStationNumberAsync(stationNumber);
            return station != null ? MapToDTO(station) : null;
        }

        /* ================= MAPPING ================= */

        private StationDTO MapToDTO(Station entity)
        {
            return new StationDTO
            {
                Id = entity.Id,
                StationNumber = entity.StationNumber,
                Name = entity.Name,
                Address = entity.Address,
                City = entity.City,
                PhoneNumber = entity.PhoneNumber,
                Capacity = entity.Capacity,
                Status = entity.Status
            };
        }

        private Station MapToEntity(StationDTO dto)
        {
            return new Station
            {
                Id = dto.Id,
                StationNumber = dto.StationNumber,
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                PhoneNumber = dto.PhoneNumber,
                Capacity = dto.Capacity,
                Status = dto.Status
            };
        }
    }
}
