using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;

namespace FireForce.Infrastructure.Repositories
{
    public class FirefighterRepository : Repository<Firefighter>, IFirefighterRepository
    {
        public FirefighterRepository(DatabaseContext context) : base(context, "Firefighters")
        {  
        }

        public async Task<Firefighter?> GetByBadgeNumberAsync(string badgeNumber)
        {
            using var connection =_context.CreateConnection();
            var sql ="SELECT * FROM Firefighters WHERE BadgeNumber = @BadgeNumber AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<Firefighter>(sql, new { BadgeNumber = badgeNumber });
        }
        

        public async Task<IEnumerable<Firefighter>> GetByStationIdAsync(int stationId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Firefighters WHERE StationId = @StationId AND IsDeleted = 0";
            return await connection.QueryAsync<Firefighter>(sql, new { StationId = stationId });
        }
    }
}