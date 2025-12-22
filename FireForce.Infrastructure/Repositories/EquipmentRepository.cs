using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;

namespace FireForce.Infrastructure.Repositories
{   
    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(DatabaseContext context) : base(context, "Equipment")
        {
        }

        public async Task<Equipment?> GetByEquipmentNumberAsync(string equipmentNumber)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Equipment WHERE EquipmentNumber = @EquipmentNumber AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<Equipment>(sql, new { EquipmentNumber = equipmentNumber });
        }

        public async Task<IEnumerable<Equipment>> GetByStationIdAsync(int stationId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Equipment WHERE StationId = @StationId AND IsDeleted = 0";
            return await connection.QueryAsync<Equipment>(sql, new { StationId = stationId });
        }
    }


}