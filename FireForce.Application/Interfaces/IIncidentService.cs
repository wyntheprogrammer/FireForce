using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
    public interface IIncidentService : IGenericService<IncidentDTO>
    {
        Task<IncidentDTO?> GetByIncidentNumberAsync(string incidentNumber);
        Task<IEnumerable<IncidentDTO>> GetByStationIdAsync(int stationId);
        Task<IEnumerable<IncidentDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}