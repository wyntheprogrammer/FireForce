using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;

namespace FireForce.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DatabaseContext _context;

        public AuditLogRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(AuditLog log)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO AuditLogs (TableName, Action, RecordId, OldValue, NewValue, ChangedBy, ChangedAt) 
                       VALUES (@TableName, @Action, @RecordId, @OldValue, @NewValue, @ChangedBy, @ChangedAt);
                       SELECT last_insert_rowid();";
            return await connection.ExecuteScalarAsync<int>(sql, log);
        }

        public async Task<IEnumerable<AuditLog>> GetByTableNameAsync(string tableName)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM AuditLogs WHERE TableName = @TableName ORDER BY ChangedAt DESC";
            return await connection.QueryAsync<AuditLog>(sql, new { TableName = tableName });
        }

        public async Task<IEnumerable<AuditLog>> GetByUserAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM AuditLogs WHERE ChangedBy = @Username ORDER BY ChangedAt DESC";
            return await connection.QueryAsync<AuditLog>(sql, new { Username = username });
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM AuditLogs ORDER BY ChangedAt DESC LIMIT 1000";
            return await connection.QueryAsync<AuditLog>(sql);
        }
    }
}