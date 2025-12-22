using FireForce.Application.DTOs;

namespace FireForce.Application.Interfaces
{
     public interface IReportService
    {
        Task<byte[]> GenerateFirefighterReportAsync();
        Task<byte[]> GenerateStationReportAsync();
        Task<byte[]> GenerateIncidentReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> GenerateEquipmentReportAsync();
        Task<byte[]> GenerateAuditLogReportAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}