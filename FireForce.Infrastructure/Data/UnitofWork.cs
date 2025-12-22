using FireForce.Domain.Interfaces;
using FireForce.Infrastructure.Repositories;
using System.Data;

namespace FireForce.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IDbTransaction? _transaction;
        private IDbConnection? _connection;


        public IUserRepository Users { get; }
        public IFirefighterRepository Firefighters { get; }
        public IStationRepository Stations { get; }
        public IIncidentRepository Incidents { get; }
        public IEquipmentRepository Equipment { get; }
        public IAuditLogRepository AuditLogs { get; }


        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Firefighters = new FirefighterRepository (_context);
            Stations = new StationRepository(_context);
            Incidents = new IncidentRepository(_context);   
            Equipment = new EquipmentRepository(_context);
            AuditLogs = new AuditLogRepository(_context);
        }


        public async Task BeginTransactionAsync()
        {
            _connection = _context.CreateConnection();
            await Task.Run(() => _connection.Open());
            _transaction = _connection.BeginTransaction();
        }


        public async Task CommitAsync()
        {
            try
            {
                await Task.Run(() => _transaction?.Commit());
            }
            catch
            {
                await Task.Run(() => _transaction?.Rollback());
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _transaction = null;
                _connection = null;
            }
        }


        public async Task RollbackAsync()
        {
            await Task.Run(() => _transaction?.Rollback());
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = null;
            _connection = null;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    
    
    }


}