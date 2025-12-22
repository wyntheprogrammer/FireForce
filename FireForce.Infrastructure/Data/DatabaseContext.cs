using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace FireForce.Infrastructure.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();

            var createTablesSql = @"
                -- Users Table
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    FullName TEXT NOT NULL,
                    Role TEXT NOT NULL,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    UpdatedAt TEXT,
                    UpdatedBy TEXT,
                    IsDeleted INTEGER NOT NULL DEFAULT 0
                );

                -- Stations Table
                CREATE TABLE IF NOT EXISTS Stations (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StationNumber TEXT NOT NULL UNIQUE,
                    Name TEXT NOT NULL,
                    Address TEXT NOT NULL,
                    City TEXT NOT NULL,
                    PhoneNumber TEXT NOT NULL,
                    Capacity INTEGER NOT NULL,
                    Status TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    UpdatedAt TEXT,
                    UpdatedBy TEXT,
                    IsDeleted INTEGER NOT NULL DEFAULT 0
                );

                -- Firefighters Table
                CREATE TABLE IF NOT EXISTS Firefighters (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    BadgeNumber TEXT NOT NULL UNIQUE,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Rank TEXT NOT NULL,
                    DateOfBirth TEXT NOT NULL,
                    PhoneNumber TEXT NOT NULL,
                    Address TEXT NOT NULL,
                    HireDate TEXT NOT NULL,
                    Status TEXT NOT NULL,
                    StationId INTEGER,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    UpdatedAt TEXT,
                    UpdatedBy TEXT,
                    IsDeleted INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (StationId) REFERENCES Stations(Id)
                );

                -- Incidents Table
                CREATE TABLE IF NOT EXISTS Incidents (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IncidentNumber TEXT NOT NULL UNIQUE,
                    IncidentType TEXT NOT NULL,
                    IncidentDate TEXT NOT NULL,
                    Location TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    Severity TEXT NOT NULL,
                    Status TEXT NOT NULL,
                    StationId INTEGER,
                    ResponseTime TEXT,
                    ResolvedTime TEXT,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    UpdatedAt TEXT,
                    UpdatedBy TEXT,
                    IsDeleted INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (StationId) REFERENCES Stations(Id)
                );

                -- Equipment Table
                CREATE TABLE IF NOT EXISTS Equipment (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    EquipmentNumber TEXT NOT NULL UNIQUE,
                    Name TEXT NOT NULL,
                    Type TEXT NOT NULL,
                    Manufacturer TEXT NOT NULL,
                    PurchaseDate TEXT NOT NULL,
                    LastMaintenanceDate TEXT,
                    NextMaintenanceDate TEXT,
                    Status TEXT NOT NULL,
                    StationId INTEGER,
                    CreatedAt TEXT NOT NULL,
                    CreatedBy TEXT NOT NULL,
                    UpdatedAt TEXT,
                    UpdatedBy TEXT,
                    IsDeleted INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (StationId) REFERENCES Stations(Id)
                );

                -- AuditLogs Table
                CREATE TABLE IF NOT EXISTS AuditLogs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TableName TEXT NOT NULL,
                    Action TEXT NOT NULL,
                    RecordId INTEGER NOT NULL,
                    OldValue TEXT NOT NULL,
                    NewValue TEXT NOT NULL,
                    ChangedBy TEXT NOT NULL,
                    ChangedAt TEXT NOT NULL
                );

                -- Create Indexes
                CREATE INDEX IF NOT EXISTS idx_users_username ON Users(Username);
                CREATE INDEX IF NOT EXISTS idx_firefighters_badge ON Firefighters(BadgeNumber);
                CREATE INDEX IF NOT EXISTS idx_incidents_number ON Incidents(IncidentNumber);
                CREATE INDEX IF NOT EXISTS idx_equipment_number ON Equipment(EquipmentNumber);
                CREATE INDEX IF NOT EXISTS idx_auditlogs_table ON AuditLogs(TableName);
            ";

            using var command = connection.CreateCommand();
            command.CommandText = createTablesSql;
            await command.ExecuteNonQueryAsync();
        }
    }
}