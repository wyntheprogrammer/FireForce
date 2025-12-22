using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
    public interface IFirefighterService : IGenericService<FirefighterDTO>
    {
        Task<FirefighterDTO?> GetByBadgeNumberAsync(string badgeNumber);
        Task<IEnumerable<FirefighterDTO>> GetByStationIdAsync(int stationId);
    }
}