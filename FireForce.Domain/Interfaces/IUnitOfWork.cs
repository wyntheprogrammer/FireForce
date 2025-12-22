namespace FireForce.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IFirefighterRepository Firefighters { get; }
        IStationRepository Stations { get; }
        IIncidentRepository Incidents { get; }
        IEquipmentRepository Equipment { get; }
        IAuditLogRepository AuditLogs { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}