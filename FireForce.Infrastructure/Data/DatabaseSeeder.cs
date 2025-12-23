using Microsoft.Data.Sqlite;
using System.Data.Common;
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
            //var adminExists = await connection.ExecuteScalarAsync<int>(
            //    "SELECT COUNT(*) FROM Users WHERE Username = 'admin'");

            //if (adminExists == 0)
            //{
            //    // Create default admin user
            //    var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                
            //    await connection.ExecuteAsync(@"
            //        INSERT INTO Users (Username, PasswordHash, Email, FullName, Role, IsActive, CreatedAt, CreatedBy, IsDeleted)
            //        VALUES (@Username, @PasswordHash, @Email, @FullName, @Role, @IsActive, @CreatedAt, @CreatedBy, @IsDeleted)",
            //        new
            //        {
            //            Username = "admin",
            //            PasswordHash = passwordHash,
            //            Email = "admin@fireforce.com",
            //            FullName = "System Administrator",
            //            Role = "Admin",
            //            IsActive = 1,
            //            CreatedAt = DateTime.UtcNow.ToString("o"),
            //            CreatedBy = "System",
            //            IsDeleted = 0
            //        });

            //    Console.WriteLine("Default admin user created:");
            //    Console.WriteLine("Username: admin");
            //    Console.WriteLine("Password: admin123");
            //}

            var dataExists = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Users");

            if (dataExists > 0)
            {
                Console.WriteLine("Database already contains data. Skipping seed.");
                return;
            }

            Console.WriteLine("Seeding database sample data....");

            await SeedUsers(connection);

            await SeedStations(connection);

            await SeedFirefighters(connection);

            await SeedIncidents(connection);

            await SeedEquipment(connection);

            Console.WriteLine("Database seeding completed successfully!");
        }


        private async Task SeedUsers(DbConnection connection)
        {
            var users = new[]
            {
                new
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Email = "admin@fireforce.ph",
                    FullName ="Wyn Bacolod",
                    Role = "Admin",
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    Username = "captain",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("captain123"),
                    Email = "ethan@fireforce.ph",
                    FullName ="Ethan Corona",
                    Role = "Captain",
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    Username = "firefighter",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("firefighter123"),
                    Email = "firefighter@fireforce.ph",
                    FullName ="Cyrus Ben",
                    Role = "Firefighter",
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                }
            };

            foreach (var user in users)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO Users (Username, PasswordHash, Email, FullName, Role, IsActive, CreatedAt, CreatedBy, IsDeleted)
                    VALUES (@Username, @PasswordHash, @Email, @FullName, @Role, @IsActive, @CreatedAt, @CreatedBy, @IsDeleted)", user);
            }

            Console.WriteLine("✓ Users seeded (4 users)");
        }


        private async Task SeedStations(DbConnection connection)
        {
            var stations = new[]
            {
                new
                {
                    StationNumber = "FS-MNL-001",
                    Name = "Manila Central Fire Station",
                    Address = "Plaza Lawton, Ermita",
                    City = "Manila",
                    PhoneNumber = "+63-2-8310-3333",
                    Capacity = 45,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    StationNumber = "FS-QC-001",
                    Name = "Quezon City Fire Station",
                    Address = "EDSA corner Timog Avenue",
                    City = "Quezon City",
                    PhoneNumber = "+63-2-8921-0001",
                    Capacity = 50,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    StationNumber = "FS-MAK-001",
                    Name = "Makati Fire Station",
                    Address = "Ayala Avenue, Makati CBD",
                    City = "Makati",
                    PhoneNumber = "+63-2-8870-5555",
                    Capacity = 40,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    StationNumber = "FS-PAS-001",
                    Name = "Pasig Fire Station",
                    Address = "Ortigas Center",
                    City = "Pasig",
                    PhoneNumber = "+63-2-8631-2222",
                    Capacity = 35,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    StationNumber = "FS-TAY-001",
                    Name = "Taguig Fire Station",
                    Address = "BGC, Taguig",
                    City = "Taguig",
                    PhoneNumber = "+63-2-8789-4444",
                    Capacity = 30,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new
                {
                    StationNumber = "FS-CAL-001",
                    Name = "Caloocan Fire Station",
                    Address = "10th Avenue, Caloocan",
                    City = "Caloocan",
                    PhoneNumber = "+63-2-8361-7777",
                    Capacity = 38,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                }
            };

            foreach (var station in stations)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO Stations (StationNumber, Name, Address, City, PhoneNumber, Capacity, Status, CreatedAt, CreatedBy, IsDeleted)
                                 VALUES (@StationNumber, @Name, @Address, @City, @PhoneNumber, @Capacity, @Status, @CreatedAt, @CreatedBy, @IsDeleted)", station);
            }

            Console.WriteLine("✓ Stations seeded (6 stations)");
        } 


        private async Task SeedFirefighters(DbConnection connection)
        {
            var firefighters = new[]
            {
                // Manila Station
                new {
                    BadgeNumber = "FF-2018-001",
                    FirstName = "Antonio",
                    LastName = "Luna",
                    Rank = "Fire Chief",
                    DateOfBirth = new DateTime(1975, 3, 15).ToString("o"),
                    PhoneNumber = "+63-917-123-4567",
                    Address = "123 Taft Avenue, Manila",
                    HireDate = new DateTime(2000, 1, 15).ToString("o"),
                    Status = "Active",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2019-002",
                    FirstName = "Andres",
                    LastName = "Bonifacio",
                    Rank = "Battalion Chief",
                    DateOfBirth = new DateTime(1980, 11, 30).ToString("o"),
                    PhoneNumber = "+63-917-234-5678",
                    Address = "456 Roxas Boulevard, Manila",
                    HireDate = new DateTime(2005, 6, 1).ToString("o"),
                    Status = "Active",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2020-003",
                    FirstName = "Emilio",
                    LastName = "Aguinaldo",
                    Rank = "Captain",
                    DateOfBirth = new DateTime(1985, 7, 22).ToString("o"),
                    PhoneNumber = "+63-917-345-6789",
                    Address = "789 United Nations Avenue, Manila",
                    HireDate = new DateTime(2010, 3, 10).ToString("o"),
                    Status = "Active",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2021-004",
                    FirstName = "Apolinario",
                    LastName = "Mabini",
                    Rank = "Lieutenant",
                    DateOfBirth = new DateTime(1988, 5, 10).ToString("o"),
                    PhoneNumber = "+63-917-456-7890",
                    Address = "321 Pedro Gil Street, Manila",
                    HireDate = new DateTime(2015, 8, 20).ToString("o"),
                    Status = "Active",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                
                // Quezon City Station
                new {
                    BadgeNumber = "FF-2020-005",
                    FirstName = "Gabriela",
                    LastName = "Silang",
                    Rank = "Captain",
                    DateOfBirth = new DateTime(1987, 3, 19).ToString("o"),
                    PhoneNumber = "+63-918-567-8901",
                    Address = "555 Commonwealth Avenue, QC",
                    HireDate = new DateTime(2012, 2, 14).ToString("o"),
                    Status = "Active",
                    StationId = 2,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2021-006",
                    FirstName = "Melchora",
                    LastName = "Aquino",
                    Rank = "Lieutenant",
                    DateOfBirth = new DateTime(1990, 1, 6).ToString("o"),
                    PhoneNumber = "+63-918-678-9012",
                    Address = "888 Quezon Avenue, QC",
                    HireDate = new DateTime(2016, 5, 1).ToString("o"),
                    Status = "Active",
                    StationId = 2,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2022-007",
                    FirstName = "Diego",
                    LastName = "Silang",
                    Rank = "Driver/Operator",
                    DateOfBirth = new DateTime(1992, 12, 16).ToString("o"),
                    PhoneNumber = "+63-918-789-0123",
                    Address = "777 East Avenue, QC",
                    HireDate = new DateTime(2018, 9, 15).ToString("o"),
                    Status = "Active",
                    StationId = 2,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                
                // Makati Station
                new {
                    BadgeNumber = "FF-2019-008",
                    FirstName = "Manuel",
                    LastName = "Quezon",
                    Rank = "Captain",
                    DateOfBirth = new DateTime(1983, 8, 19).ToString("o"),
                    PhoneNumber = "+63-919-890-1234",
                    Address = "999 Makati Avenue, Makati",
                    HireDate = new DateTime(2008, 4, 10).ToString("o"),
                    Status = "Active",
                    StationId = 3,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2022-009",
                    FirstName = "Corazon",
                    LastName = "Aquino",
                    Rank = "Firefighter",
                    DateOfBirth = new DateTime(1995, 1, 25).ToString("o"),
                    PhoneNumber = "+63-919-901-2345",
                    Address = "222 Buendia Avenue, Makati",
                    HireDate = new DateTime(2019, 11, 5).ToString("o"),
                    Status = "Active",
                    StationId = 3,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                
                // Pasig Station
                new {
                    BadgeNumber = "FF-2021-010",
                    FirstName = "Lapu",
                    LastName = "Lapu",
                    Rank = "Lieutenant",
                    DateOfBirth = new DateTime(1989, 4, 27).ToString("o"),
                    PhoneNumber = "+63-920-012-3456",
                    Address = "111 Ortigas Avenue, Pasig",
                    HireDate = new DateTime(2017, 7, 22).ToString("o"),
                    Status = "Active",
                    StationId = 4,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2023-011",
                    FirstName = "Rajah",
                    LastName = "Humabon",
                    Rank = "Firefighter",
                    DateOfBirth = new DateTime(1996, 6, 12).ToString("o"),
                    PhoneNumber = "+63-920-123-4567",
                    Address = "333 Meralco Avenue, Pasig",
                    HireDate = new DateTime(2020, 2, 10).ToString("o"),
                    Status = "Active",
                    StationId = 4,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                
                // Taguig Station
                new {
                    BadgeNumber = "FF-2022-012",
                    FirstName = "Fernando",
                    LastName = "Poe",
                    Rank = "Firefighter",
                    DateOfBirth = new DateTime(1994, 8, 20).ToString("o"),
                    PhoneNumber = "+63-921-234-5678",
                    Address = "444 McKinley Road, BGC",
                    HireDate = new DateTime(2021, 1, 15).ToString("o"),
                    Status = "Active",
                    StationId = 5,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                
                // Caloocan Station
                new {
                    BadgeNumber = "FF-2020-013",
                    FirstName = "Ninoy",
                    LastName = "Aquino",
                    Rank = "Lieutenant",
                    DateOfBirth = new DateTime(1986, 11, 27).ToString("o"),
                    PhoneNumber = "+63-921-345-6789",
                    Address = "555 9th Avenue, Caloocan",
                    HireDate = new DateTime(2013, 10, 5).ToString("o"),
                    Status = "Active",
                    StationId = 6,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2023-014",
                    FirstName = "Marcelo",
                    LastName = "Del Pilar",
                    Rank = "Firefighter",
                    DateOfBirth = new DateTime(1997, 8, 30).ToString("o"),
                    PhoneNumber = "+63-921-456-7890",
                    Address = "666 8th Avenue, Caloocan",
                    HireDate = new DateTime(2022, 6, 1).ToString("o"),
                    Status = "Active",
                    StationId = 6,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    BadgeNumber = "FF-2019-015",
                    FirstName = "Gregorio",
                    LastName = "Del Pilar",
                    Rank = "Driver/Operator",
                    DateOfBirth = new DateTime(1991, 11, 14).ToString("o"),
                    PhoneNumber = "+63-921-567-8901",
                    Address = "777 11th Avenue, Caloocan",
                    HireDate = new DateTime(2014, 12, 1).ToString("o"),
                    Status = "Active",
                    StationId = 6,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                }
            };

            foreach (var firefighter in firefighters)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO Firefighters (BadgeNumber, FirstName, LastName, Rank, DateOfBirth, PhoneNumber, Address, HireDate, Status, StationId, CreatedAt, CreatedBy, IsDeleted)
                    VALUES (@BadgeNumber, @FirstName, @LastName, @Rank, @DateOfBirth, @PhoneNumber, @Address, @HireDate, @Status, @StationId, @CreatedAt, @CreatedBy, @IsDeleted)", firefighter);
            }

            Console.WriteLine("✓ Firefighters seeded (15 firefighters)");
        }

        private async Task SeedIncidents(DbConnection connection)
        {
            var random = new Random();
            var incidents = new[]
            {
                new {
                    IncidentNumber = "INC-2024-001",
                    IncidentType = "Fire",
                    IncidentDate = DateTime.UtcNow.AddDays(-45).ToString("o"),
                    Location = "Divisoria Market, Manila",
                    Description = "Large fire at textile market. Multiple stalls affected. Cause under investigation.",
                    Severity = "Critical",
                    Status = "Resolved",
                    StationId = 1,
                    ResponseTime = DateTime.UtcNow.AddDays(-45).AddMinutes(8).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-45).AddHours(4).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-002",
                    IncidentType = "Fire",
                    IncidentDate = DateTime.UtcNow.AddDays(-30).ToString("o"),
                    Location = "SM North EDSA, Quezon City",
                    Description = "Small kitchen fire in food court. Quickly contained.",
                    Severity = "Low",
                    Status = "Closed",
                    StationId = 2,
                    ResponseTime = DateTime.UtcNow.AddDays(-30).AddMinutes(6).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-30).AddMinutes(35).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-003",
                    IncidentType = "Medical",
                    IncidentDate = DateTime.UtcNow.AddDays(-25).ToString("o"),
                    Location = "Ayala Triangle, Makati",
                    Description = "Person collapsed during jogging. CPR administered.",
                    Severity = "High",
                    Status = "Resolved",
                    StationId = 3,
                    ResponseTime = DateTime.UtcNow.AddDays(-25).AddMinutes(5).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-25).AddMinutes(45).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-004",
                    IncidentType = "Vehicle Accident",
                    IncidentDate = DateTime.UtcNow.AddDays(-20).ToString("o"),
                    Location = "C5 Road, Pasig",
                    Description = "Multi-vehicle collision. Extraction required.",
                    Severity = "High",
                    Status = "Resolved",
                    StationId = 4,
                    ResponseTime = DateTime.UtcNow.AddDays(-20).AddMinutes(7).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-20).AddHours(2).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-005",
                    IncidentType = "Fire",
                    IncidentDate = DateTime.UtcNow.AddDays(-18).ToString("o"),
                    Location = "BGC High Street, Taguig",
                    Description = "Fire alarm activation. False alarm from construction dust.",
                    Severity = "Low",
                    Status = "Closed",
                    StationId = 5,
                    ResponseTime = DateTime.UtcNow.AddDays(-18).AddMinutes(4).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-18).AddMinutes(25).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-006",
                    IncidentType = "Rescue",
                    IncidentDate = DateTime.UtcNow.AddDays(-15).ToString("o"),
                    Location = "Monumento Circle, Caloocan",
                    Description = "Cat stuck in tree. Successfully rescued.",
                    Severity = "Low",
                    Status = "Resolved",
                    StationId = 6,
                    ResponseTime = DateTime.UtcNow.AddDays(-15).AddMinutes(10).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-15).AddMinutes(40).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-007",
                    IncidentType = "Fire",
                    IncidentDate = DateTime.UtcNow.AddDays(-12).ToString("o"),
                    Location = "Quiapo Church vicinity, Manila",
                    Description = "Residential fire. 5 families displaced.",
                    Severity = "High",
                    Status = "Resolved",
                    StationId = 1,
                    ResponseTime = DateTime.UtcNow.AddDays(-12).AddMinutes(6).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-12).AddHours(3).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-008",
                    IncidentType = "Hazmat",
                    IncidentDate = DateTime.UtcNow.AddDays(-10).ToString("o"),
                    Location = "Katipunan Avenue, QC",
                    Description = "Chemical spill from delivery truck. Area cordoned.",
                    Severity = "Critical",
                    Status = "Resolved",
                    StationId = 2,
                    ResponseTime = DateTime.UtcNow.AddDays(-10).AddMinutes(9).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-10).AddHours(6).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-009",
                    IncidentType = "Medical",
                    IncidentDate = DateTime.UtcNow.AddDays(-8).ToString("o"),
                    Location = "Glorietta Mall, Makati",
                    Description = "Elderly person with chest pain. Transported to hospital.",
                    Severity = "Medium",
                    Status = "Resolved",
                    StationId = 3,
                    ResponseTime = DateTime.UtcNow.AddDays(-8).AddMinutes(5).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-8).AddMinutes(30).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-010",
                    IncidentType = "Fire",
                    IncidentDate = DateTime.UtcNow.AddDays(-5).ToString("o"),
                    Location = "Ortigas Business District, Pasig",
                    Description = "Office building fire drill. No actual emergency.",
                    Severity = "Low",
                    Status = "Closed",
                    StationId = 4,
                    ResponseTime = DateTime.UtcNow.AddDays(-5).AddMinutes(7).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-5).AddMinutes(20).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-011",
                    IncidentType = "Rescue",
                    IncidentDate = DateTime.UtcNow.AddDays(-3).ToString("o"),
                    Location = "Venice Grand Canal, Taguig",
                    Description = "Person fell into canal. Water rescue performed.",
                    Severity = "Medium",
                    Status = "Resolved",
                    StationId = 5,
                    ResponseTime = DateTime.UtcNow.AddDays(-3).AddMinutes(8).ToString("o"),
                    ResolvedTime = DateTime.UtcNow.AddDays(-3).AddMinutes(50).ToString("o"),
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-012",
                    IncidentType = "Fire",
                    IncidentDate = DateTime.UtcNow.AddDays(-2).ToString("o"),
                    Location = "Araneta City, Caloocan",
                    Description = "Electrical fire in apartment unit. Contained to one room.",
                    Severity = "Medium",
                    Status = "InProgress",
                    StationId = 6,
                    ResponseTime = DateTime.UtcNow.AddDays(-2).AddMinutes(6).ToString("o"),
                    ResolvedTime = (string)null,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    IncidentNumber = "INC-2024-013",
                    IncidentType = "Vehicle Accident",
                    IncidentDate = DateTime.UtcNow.AddDays(-1).ToString("o"),
                    Location = "Roxas Boulevard, Manila",
                    Description = "Two-car collision. Minor injuries reported.",
                    Severity = "Low",
                    Status = "InProgress",
                    StationId = 1,
                    ResponseTime = DateTime.UtcNow.AddDays(-1).AddMinutes(5).ToString("o"),
                    ResolvedTime = (string)null,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                }
            };

            foreach (var incident in incidents)
            {
                await connection.ExecuteAsync(@"
                INSERT INTO Incidents (IncidentNumber, IncidentType, IncidentDate, Location, Description, Severity, Status, StationId, ResponseTime, ResolvedTime, CreatedAt, CreatedBy, IsDeleted)
                VALUES (@IncidentNumber, @IncidentType, @IncidentDate, @Location, @Description, @Severity, @Status, @StationId, @ResponseTime, @ResolvedTime, @CreatedAt, @CreatedBy, @IsDeleted)", incident);
            }

            Console.WriteLine("✓ Incidents seeded (14 incidents)");
        }


        private async Task SeedEquipment(DbConnection connection)
        {
            var equipment = new[]
            {
                // Manila Station Equipment
                new {
                    EquipmentNumber = "EQ-MNL-001",
                    Name = "Fire Truck Alpha",
                    Type = "Truck",
                    Manufacturer = "Isuzu Philippines",
                    PurchaseDate = new DateTime(2020, 5, 15).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 10, 1).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 1, 1).ToString("o"),
                    Status = "Available",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-MNL-002",
                    Name = "Aerial Ladder Unit",
                    Type = "Ladder",
                    Manufacturer = "Rosenbauer",
                    PurchaseDate = new DateTime(2019, 8, 20).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 11, 15).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 2, 15).ToString("o"),
                    Status = "Available",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-MNL-003",
                    Name = "High Pressure Hose Set A",
                    Type = "Hose",
                    Manufacturer = "Dixon Valve Philippines",
                    PurchaseDate = new DateTime(2021, 3, 10).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 12, 1).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 3, 1).ToString("o"),
                    Status = "Available",
                    StationId = 1,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
            
                // Quezon City Station Equipment
                new {
                    EquipmentNumber = "EQ-QC-001",
                    Name = "Fire Truck Bravo",
                    Type = "Truck",
                    Manufacturer = "Mitsubishi Fuso",
                    PurchaseDate = new DateTime(2021, 6, 1).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 11, 20).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 2, 20).ToString("o"),
                    Status = "InUse",
                    StationId = 2,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-QC-002",
                    Name = "SCBA Set Alpha",
                    Type = "Breathing Apparatus",
                    Manufacturer = "Dräger Safety",
                    PurchaseDate = new DateTime(2022, 1, 15).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 12, 10).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 3, 10).ToString("o"),
                    Status = "Available",
                    StationId = 2,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
            
                // Makati Station Equipment
                new {
                    EquipmentNumber = "EQ-MAK-001",
                    Name = "Rescue Vehicle Charlie",
                    Type = "Truck",
                    Manufacturer = "Toyota Philippines",
                    PurchaseDate = new DateTime(2020, 9, 5).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 10, 25).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 1, 25).ToString("o"),
                    Status = "Available",
                    StationId = 3,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-MAK-002",
                    Name = "Protective Gear Set A",
                    Type = "Protective Gear",
                    Manufacturer = "MSA Safety",
                    PurchaseDate = new DateTime(2023, 2, 20).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 11, 5).ToString("o"),
                    NextMaintenanceDate = (string)null,
                    Status = "Available",
                    StationId = 3,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
            
                // Pasig Station Equipment
                new {
                    EquipmentNumber = "EQ-PAS-001",
                    Name = "Fire Truck Delta",
                    Type = "Truck",
                    Manufacturer = "Hino Motors Philippines",
                    PurchaseDate = new DateTime(2019, 11, 10).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 9, 15).ToString("o"),
                    NextMaintenanceDate = new DateTime(2024, 12, 15).ToString("o"),
                    Status = "Maintenance",
                    StationId = 4,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-PAS-002",
                    Name = "Portable Radio Set",
                    Type = "Communication Device",
                    Manufacturer = "Motorola",
                    PurchaseDate = new DateTime(2022, 7, 1).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 11, 1).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 5, 1).ToString("o"),
                    Status = "Available",
                    StationId = 4,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
            
                // Taguig Station Equipment
                new {
                    EquipmentNumber = "EQ-TAG-001",
                    Name = "Mini Pumper Echo",
                    Type = "Truck",
                    Manufacturer = "Suzuki Philippines",
                    PurchaseDate = new DateTime(2021, 4, 18).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 11, 10).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 2, 10).ToString("o"),
                    Status = "Available",
                    StationId = 5,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-TAG-002",
                    Name = "Extension Ladder 35ft",
                    Type = "Ladder",
                    Manufacturer = "Werner Philippines",
                    PurchaseDate = new DateTime(2020, 12, 5).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 10, 20).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 4, 20).ToString("o"),
                    Status = "Available",
                    StationId = 5,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
            
                // Caloocan Station Equipment
                new {
                    EquipmentNumber = "EQ-CAL-001",
                    Name = "Fire Truck Foxtrot",
                    Type = "Truck",
                    Manufacturer = "Isuzu Philippines",
                    PurchaseDate = new DateTime(2018, 7, 25).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 8, 30).ToString("o"),
                    NextMaintenanceDate = new DateTime(2024, 11, 30).ToString("o"),
                    Status = "Maintenance",
                    StationId = 6,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-CAL-002",
                    Name = "Hose Bundle Set B",
                    Type = "Hose",
                    Manufacturer = "Dixon Valve Philippines",
                    PurchaseDate = new DateTime(2022, 9, 12).ToString("o"),
                    LastMaintenanceDate = new DateTime(2024, 12, 5).ToString("o"),
                    NextMaintenanceDate = new DateTime(2025, 6, 5).ToString("o"),
                    Status = "Available",
                    StationId = 6,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                },
                new {
                    EquipmentNumber = "EQ-CAL-003",
                    Name = "Thermal Imaging Camera",
                    Type = "Other",
                    Manufacturer = "FLIR Systems",
                    PurchaseDate = new DateTime(2023, 5, 8).ToString("o"),
                    LastMaintenanceDate = (string)null,
                    NextMaintenanceDate = new DateTime(2025, 5, 8).ToString("o"),
                    Status = "Available",
                    StationId = 6,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    CreatedBy = "System",
                    IsDeleted = 0
                }
            };

            foreach (var equip in equipment)
            {
                await connection.ExecuteAsync(@"
                INSERT INTO Equipment (EquipmentNumber, Name, Type, Manufacturer, PurchaseDate, LastMaintenanceDate, NextMaintenanceDate, Status, StationId, CreatedAt, CreatedBy, IsDeleted)
                VALUES (@EquipmentNumber, @Name, @Type, @Manufacturer, @PurchaseDate, @LastMaintenanceDate, @NextMaintenanceDate, @Status, @StationId, @CreatedAt, @CreatedBy, @IsDeleted)", equip);
            }

            Console.WriteLine("✓ Equipment seeded (15 items)");
        }
    }
}