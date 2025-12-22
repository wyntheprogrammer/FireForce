using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;
using System.Data;

namespace FireForce.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DatabaseContext _context;
        protected readonly string _tableName;

        protected Repository(DatabaseContext context, string tableName)
        {
            _context = context;
            _tableName = tableName;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE IsDeleted = 0 ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<T>(sql);
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsDeleted = false;

            using var connection = _context.CreateConnection();
            var properties = GetProperties(entity, excludeKey: true);
            var columns = string.Join(", ", properties.Keys);
            var values = string.Join(", ", properties.Keys.Select(k => $"@{k}"));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({values}); SELECT last_insert_rowid();";
            var id = await connection.ExecuteScalarAsync<int>(sql, entity);
            return id;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;

            using var connection = _context.CreateConnection();
            var properties = GetProperties(entity, excludeKey: true);
            var setClause = string.Join(", ", properties.Keys.Select(k => $"{k} = @{k}"));

            var sql = $"UPDATE {_tableName} SET {setClause} WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }

        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var sql = $"UPDATE {_tableName} SET IsDeleted = 1, UpdatedAt = @UpdatedAt WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id, UpdatedAt = DateTime.UtcNow });
            return result > 0;
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(string query, object parameters)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<T>(query, parameters);
        }

        protected Dictionary<string, object?> GetProperties(T entity, bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.Name != "Id")
                .ToDictionary(
                    p => p.Name,
                    p => p.GetValue(entity)
                );
            return properties;
        }
    }
}