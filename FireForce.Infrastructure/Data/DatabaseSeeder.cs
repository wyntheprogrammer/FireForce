using Dapper;
using FireForce.Domain.Interfaces;

namespace FireForce.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly DatabaseContext _context;

        public DatabaseSeeder(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            using var connection = _context.CreateConnection();

            // Check if admin user exists
            var adminExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Users WHERE Username = 'admin'");

            if (adminExists == 0)
            {
                // Create default admin user
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                
                await connection.ExecuteAsync(@"
                    INSERT INTO Users (Username, PasswordHash, Email, FullName, Role, IsActive, CreatedAt, CreatedBy, IsDeleted)
                    VALUES (@Username, @PasswordHash, @Email, @FullName, @Role, @IsActive, @CreatedAt, @CreatedBy, @IsDeleted)",
                    new
                    {
                        Username = "admin",
                        PasswordHash = passwordHash,
                        Email = "admin@fireforce.com",
                        FullName = "System Administrator",
                        Role = "Admin",
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow.ToString("o"),
                        CreatedBy = "System",
                        IsDeleted = 0
                    });

                Console.WriteLine("Default admin user created:");
                Console.WriteLine("Username: admin");
                Console.WriteLine("Password: admin123");
            }
        }
    }
}