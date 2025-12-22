using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using System.Text.Json;

namespace FireForce.Application.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogService _auditLogService;

        public IncidentService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
        {
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }

        public async Task<IncidentDTO?> GetByIdAsync(int id)
        {
            var incident = await _unitOfWork.Incidents.GetByIdAsync(id);
            return incident != null ? await MapToDTO(incident) : null;
        }

        public async Task<IEnumerable<IncidentDTO>> GetAllAsync()
        {
            var incidents = await _unitOfWork.Incidents.GetAllAsync();
            var dtoList = new List<IncidentDTO>();

            foreach (var incident in incidents)
            {
                dtoList.Add(await MapToDTO(incident));
            }

            return dtoList;
        }

        public async Task<int> CreateAsync(IncidentDTO dto, string currentUser)
        {
            var incident = MapToEntity(dto);
            incident.CreatedBy = currentUser;

            var id = await _unitOfWork.Incidents.AddAsync(incident);

            await _auditLogService.LogChangeAsync(
                "Incidents", "INSERT", id,
                "", JsonSerializer.Serialize(dto), currentUser);

            return id;
        }

        public async Task<bool> UpdateAsync(IncidentDTO dto, string currentUser)
        {
            var existing = await _unitOfWork.Incidents.GetByIdAsync(dto.Id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(await MapToDTO(existing));

            var incident = MapToEntity(dto);
            incident.CreatedAt = existing.CreatedAt;
            incident.CreatedBy = existing.CreatedBy;
            incident.UpdatedBy = currentUser;

            var result = await _unitOfWork.Incidents.UpdateAsync(incident);

            if (result)
            {
                await _auditLogService.LogChangeAsync(
                    "Incidents", "UPDATE", dto.Id,
                    oldValue, JsonSerializer.Serialize(dto), currentUser);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(int id, string currentUser)
        {
            var existing = await _unitOfWork.Incidents.GetByIdAsync(id);
            if (existing == null)
                return false;

            var oldValue = JsonSerializer.Serialize(await MapToDTO(existing));

            var result = await _unitOfWork.Incidents.SoftDeleteAsync(id);

            if (result)
            {
                await _auditLogService.LogChangeAsync(
                    "Incidents", "DELETE", id,
                    oldValue, "", currentUser);
            }

            return result;
        }

        /* ================= INTERFACE-SPECIFIC METHODS ================= */

        public async Task<IncidentDTO?> GetByIncidentNumberAsync(string incidentNumber)
        {
            var incident = await _unitOfWork.Incidents.GetByIncidentNumberAsync(incidentNumber);
            return incident != null ? await MapToDTO(incident) : null;
        }

        public async Task<IEnumerable<IncidentDTO>> GetByStationIdAsync(int stationId)
        {
            var incidents = await _unitOfWork.Incidents.GetByStationIdAsync(stationId);
            var dtoList = new List<IncidentDTO>();

            foreach (var incident in incidents)
            {
                dtoList.Add(await MapToDTO(incident));
            }

            return dtoList;
        }

        public async Task<IEnumerable<IncidentDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var incidents = await _unitOfWork.Incidents.GetByDateRangeAsync(startDate, endDate);
            var dtoList = new List<IncidentDTO>();

            foreach (var incident in incidents)
            {
                dtoList.Add(await MapToDTO(incident));
            }

            return dtoList;
        }

        /* ================= MAPPING ================= */

        private async Task<IncidentDTO> MapToDTO(Incident entity)
        {
            var dto = new IncidentDTO
            {
                Id = entity.Id,
                IncidentNumber = entity.IncidentNumber,
                IncidentType = entity.IncidentType,
                IncidentDate = entity.IncidentDate,
                Location = entity.Location,
                Description = entity.Description,
                Severity = entity.Severity,
                Status = entity.Status,
                StationId = entity.StationId,
                ResponseTime = entity.ResponseTime,
                ResolvedTime = entity.ResolvedTime
            };

            if (entity.StationId.HasValue)
            {
                var station = await _unitOfWork.Stations.GetByIdAsync(entity.StationId.Value);
                dto.StationName = station?.Name;
            }

            return dto;
        }

        private Incident MapToEntity(IncidentDTO dto)
        {
            return new Incident
            {
                Id = dto.Id,
                IncidentNumber = dto.IncidentNumber,
                IncidentType = dto.IncidentType,
                IncidentDate = dto.IncidentDate,
                Location = dto.Location,
                Description = dto.Description,
                Severity = dto.Severity,
                Status = dto.Status,
                StationId = dto.StationId,
                ResponseTime = dto.ResponseTime,
                ResolvedTime = dto.ResolvedTime
            };
        }
    }
}
