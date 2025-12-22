using FireForce.Domain.Entities;

namespace FireForce.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
    }

    public interface IFirefighterRepository : IRepository<Firefighter>
    {
        Task<Firefighter?> GetByBadgeNumberAsync(string badgeNumber);
        Task<IEnumerable<Firefighter>> GetByStationIdAsync(int stationId);
    }

    public interface IStationRepository : IRepository<Station>
    {
        Task<Station?> GetByStationNumberAsync(string stationNumber);
    }

    public interface IIncidentRepository : IRepository<Incident>
    {
        Task<Incident?> GetByIncidentNumberAsync(string incidentNumber);
        Task<IEnumerable<Incident>> GetByStationIdAsync(int stationId);
        Task<IEnumerable<Incident>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }

    public interface IEquipmentRepository : IRepository<Equipment>
    {
        Task<Equipment?> GetByEquipmentNumberAsync(string equipmentNumber);
        Task<IEnumerable<Equipment>> GetByStationIdAsync(int stationId);
    }

    public interface IAuditLogRepository
    {
        Task<int> AddAsync(AuditLog log);
        Task<IEnumerable<AuditLog>> GetByTableNameAsync(string tableName);
        Task<IEnumerable<AuditLog>> GetByUserAsync(string username);
        Task<IEnumerable<AuditLog>> GetAllAsync();
    }
}