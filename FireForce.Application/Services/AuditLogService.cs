using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;

namespace FireForce.Application.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogChangeAsync(string tableName, string action, int recordId, 
            string oldValue, string newValue, string changedBy)
        {
            var log = new AuditLog
            {
                TableName = tableName,
                Action = action,
                RecordId = recordId,
                OldValue = oldValue,
                NewValue = newValue,
                ChangedBy = changedBy,
                ChangedAt = DateTime.UtcNow
            };

            await _unitOfWork.AuditLogs.AddAsync(log);
        }

        public async Task<IEnumerable<AuditLogDTO>> GetByTableNameAsync(string tableName)
        {
            var logs = await _unitOfWork.AuditLogs.GetByTableNameAsync(tableName);
            return logs.Select(MapToDTO);
        }

        public async Task<IEnumerable<AuditLogDTO>> GetByUserAsync(string username)
        {
            var logs = await _unitOfWork.AuditLogs.GetByUserAsync(username);
            return logs.Select(MapToDTO);
        }

        public async Task<IEnumerable<AuditLogDTO>> GetAllAsync()
        {
            var logs = await _unitOfWork.AuditLogs.GetAllAsync();
            return logs.Select(MapToDTO);
        }

        private AuditLogDTO MapToDTO(AuditLog entity)
        {
            return new AuditLogDTO
            {
                Id = entity.Id,
                TableName = entity.TableName,
                Action = entity.Action,
                RecordId = entity.RecordId,
                OldValue = entity.OldValue,
                NewValue = entity.NewValue,
                ChangedBy = entity.ChangedBy,
                ChangedAt = entity.ChangedAt
            };
        }
    }
}