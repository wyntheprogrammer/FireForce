using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;

namespace FireForce.Infrastructure.Repositories
{
    public class IncidentRepository : Repository<Incident>, IIncidentRepository
    {
        public IncidentRepository(DatabaseContext context) : base(context, "Incidents")
        {
        }

        public async Task<Incident?> GetByIncidentNumberAsync(string incidentNumber)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Incidents WHERE IncidentNumber = @IncidentNumber AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<Incident>(sql, new { IncidentNumber = incidentNumber });
        }

        public async Task<IEnumerable<Incident>> GetByStationIdAsync(int stationId)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Incidents WHERE StationId = @StationId AND IsDeleted = 0 ORDER BY IncidentDate DESC";
            return await connection.QueryAsync<Incident>(sql, new { StationId = stationId });
        }

        public async Task<IEnumerable<Incident>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            using var connection = _context.CreateConnection();
            var sql = @"SELECT * FROM Incidents 
                       WHERE IncidentDate BETWEEN @StartDate AND @EndDate 
                       AND IsDeleted = 0 
                       ORDER BY IncidentDate DESC";
            return await connection.QueryAsync<Incident>(sql, new { StartDate = startDate, EndDate = endDate });
        }
    }

}