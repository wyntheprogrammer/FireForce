using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;

namespace FireForce.Infrastructure.Repositories
{
    // StationRepository.cs
    public class StationRepository : Repository<Station>, IStationRepository
    {
        public StationRepository(DatabaseContext context) : base(context, "Stations")
        {
        }

        public async Task<Station?> GetByStationNumberAsync(string stationNumber)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Stations WHERE StationNumber = @StationNumber AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<Station>(sql, new { StationNumber = stationNumber });
        }
    }

}