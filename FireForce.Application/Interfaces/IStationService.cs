using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
    public interface IStationService : IGenericService<StationDTO>
    {
        Task<StationDTO?> GetByStationNumberAsync(string stationNumber);
    }
}