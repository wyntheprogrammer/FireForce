using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using System.Text.Json;

namespace FireForce.Application.Services
{
    public class FirefighterService : IFirefighterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;

        public FirefighterService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }

        public async Task<FirefighterDTO?> GetByIdAsync(int id)
        {
            var firefighter = await _unitOfWork.Firefighters.GetByIdAsync(id);
            return firefighter != null ? await MapToDTO(firefighter) : null;
        }

        public async Task<IEnumerable<FirefighterDTO>> GetAllAsync()
        {
            var firefighters = await _unitOfWork.Firefighters.GetAllAsync();
            var dtoList = new List<FirefighterDTO>();
            foreach (var firefighter in firefighters)
            {
                dtoList.Add(await MapToDTO(firefighter));
            }
            return dtoList;
        }

        public async Task<int> CreateAsync(FirefighterDTO dto, string currentUser)
        {
            var firefighter = MapToEntity(dto);
            firefighter.CreatedBy = currentUser;
            
            var id = await _unitOfWork.Firefighters.AddAsync(firefighter);
            
            await _auditLogService.LogChangeAsync("Firefighters", "INSERT", id, 
                "", JsonSerializer.Serialize(dto), currentUser);
            
            return id;
        }

        public async Task<bool> UpdateAsync(FirefighterDTO dto, string currentUser)
        {
            var existing = await _unitOfWork.Firefighters.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(await MapToDTO(existing));
            
            var firefighter = MapToEntity(dto);
            firefighter.UpdatedBy = currentUser;
            firefighter.CreatedAt = existing.CreatedAt;
            firefighter.CreatedBy = existing.CreatedBy;
            
            var result = await _unitOfWork.Firefighters.UpdateAsync(firefighter);
            
            if (result)
            {
                await _auditLogService.LogChangeAsync("Firefighters", "UPDATE", dto.Id, 
                    oldValue, JsonSerializer.Serialize(dto), currentUser);
            }
            
            return result;
        }

        public async Task<bool> DeleteAsync(int id, string currentUser)
        {
            var existing = await _unitOfWork.Firefighters.GetByIdAsync(id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(await MapToDTO(existing));
            
            var result = await _unitOfWork.Firefighters.SoftDeleteAsync(id);
            
            if (result)
            {
                await _auditLogService.LogChangeAsync("Firefighters", "DELETE", id, 
                    oldValue, "", currentUser);
            }
            
            return result;
        }

        public async Task<FirefighterDTO?> GetByBadgeNumberAsync(string badgeNumber)
        {
            var firefighter = await _unitOfWork.Firefighters.GetByBadgeNumberAsync(badgeNumber);
            return firefighter != null ? await MapToDTO(firefighter) : null;
        }

        public async Task<IEnumerable<FirefighterDTO>> GetByStationIdAsync(int stationId)
        {
            var firefighters = await _unitOfWork.Firefighters.GetByStationIdAsync(stationId);
            var dtoList = new List<FirefighterDTO>();
            foreach (var firefighter in firefighters)
            {
                dtoList.Add(await MapToDTO(firefighter));
            }
            return dtoList;
        }

        private async Task<FirefighterDTO> MapToDTO(Firefighter entity)
        {
            var dto = new FirefighterDTO
            {
                Id = entity.Id,
                BadgeNumber = entity.BadgeNumber,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Rank = entity.Rank,
                DateOfBirth = entity.DateOfBirth,
                PhoneNumber = entity.PhoneNumber,
                Address = entity.Address,
                HireDate = entity.HireDate,
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

        private Firefighter MapToEntity(FirefighterDTO dto)
        {
            return new Firefighter
            {
                Id = dto.Id,
                BadgeNumber = dto.BadgeNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Rank = dto.Rank,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                HireDate = dto.HireDate,
                Status = dto.Status,
                StationId = dto.StationId
            };
        }
    }
}