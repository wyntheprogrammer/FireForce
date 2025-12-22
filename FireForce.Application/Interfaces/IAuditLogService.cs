using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task LogChangeAsync(string tableName, string action, int recordId, string oldValue, string newValue, string changeBy);
        Task<IEnumerable<AuditLogDTO>> GetByTableNameAsync(string tableName);
        Task<IEnumerable<AuditLogDTO>> GetByUserAsync(string username);
        Task<IEnumerable<AuditLogDTO>> GetAllAsync();
    }
}