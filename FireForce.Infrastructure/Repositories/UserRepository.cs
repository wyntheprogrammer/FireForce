using Dapper;
using FireForce.Domain.Entities;
using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Data;

namespace FireForce.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context, "Users")
        {
        }


        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT * FROM Users Where Username = @Username AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = _context.CreateConnection();
            var sql ="SELECT * FROM Users WHERE Email = @Email AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }
    }
}