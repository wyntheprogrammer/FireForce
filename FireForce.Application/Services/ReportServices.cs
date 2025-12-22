using FireForce.Application.Interfaces;
using FireForce.Domain.Interfaces;
using System.Text;

namespace FireForce.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<byte[]> GenerateFirefighterReportAsync()
        {
            var firefighters = await _unitOfWork.Firefighters.GetAllAsync();
            var sb = new StringBuilder();
            
            sb.AppendLine("FIREFIGHTER REPORT");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine(new string('=', 100));
            sb.AppendLine();
            sb.AppendLine($"{"Badge",-15} {"Name",-30} {"Rank",-20} {"Status",-15} {"Hire Date",-15}");
            sb.AppendLine(new string('-', 100));

            foreach (var ff in firefighters)
            {
                var name = $"{ff.FirstName} {ff.LastName}";
                sb.AppendLine($"{ff.BadgeNumber,-15} {name,-30} {ff.Rank,-20} {ff.Status,-15} {ff.HireDate:yyyy-MM-dd}");
            }

            sb.AppendLine(new string('=', 100));
            sb.AppendLine($"Total Firefighters: {firefighters.Count()}");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<byte[]> GenerateStationReportAsync()
        {
            var stations = await _unitOfWork.Stations.GetAllAsync();
            var sb = new StringBuilder();
            
            sb.AppendLine("STATION REPORT");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine(new string('=', 100));
            sb.AppendLine();
            sb.AppendLine($"{"Number",-10} {"Name",-30} {"City",-20} {"Capacity",-10} {"Status",-15}");
            sb.AppendLine(new string('-', 100));

            foreach (var station in stations)
            {
                sb.AppendLine($"{station.StationNumber,-10} {station.Name,-30} {station.City,-20} {station.Capacity,-10} {station.Status,-15}");
            }

            sb.AppendLine(new string('=', 100));
            sb.AppendLine($"Total Stations: {stations.Count()}");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<byte[]> GenerateIncidentReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            IEnumerable<Domain.Entities.Incident> incidents;
            
            if (startDate.HasValue && endDate.HasValue)
            {
                incidents = await _unitOfWork.Incidents.GetByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                incidents = await _unitOfWork.Incidents.GetAllAsync();
            }

            var sb = new StringBuilder();
            
            sb.AppendLine("INCIDENT REPORT");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            if (startDate.HasValue && endDate.HasValue)
            {
                sb.AppendLine($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
            }
            sb.AppendLine(new string('=', 120));
            sb.AppendLine();
            sb.AppendLine($"{"Number",-15} {"Type",-15} {"Date",-20} {"Location",-30} {"Severity",-12} {"Status",-12}");
            sb.AppendLine(new string('-', 120));

            foreach (var incident in incidents)
            {
                sb.AppendLine($"{incident.IncidentNumber,-15} {incident.IncidentType,-15} {incident.IncidentDate:yyyy-MM-dd HH:mm,-20} {incident.Location,-30} {incident.Severity,-12} {incident.Status,-12}");
            }

            sb.AppendLine(new string('=', 120));
            sb.AppendLine($"Total Incidents: {incidents.Count()}");
            
            var groupedBySeverity = incidents.GroupBy(i => i.Severity);
            sb.AppendLine("\nBreakdown by Severity:");
            foreach (var group in groupedBySeverity)
            {
                sb.AppendLine($"  {group.Key}: {group.Count()}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<byte[]> GenerateEquipmentReportAsync()
        {
            var equipment = await _unitOfWork.Equipment.GetAllAsync();
            var sb = new StringBuilder();
            
            sb.AppendLine("EQUIPMENT REPORT");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine(new string('=', 120));
            sb.AppendLine();
            sb.AppendLine($"{"Number",-15} {"Name",-25} {"Type",-20} {"Manufacturer",-25} {"Status",-15}");
            sb.AppendLine(new string('-', 120));

            foreach (var eq in equipment)
            {
                sb.AppendLine($"{eq.EquipmentNumber,-15} {eq.Name,-25} {eq.Type,-20} {eq.Manufacturer,-25} {eq.Status,-15}");
            }

            sb.AppendLine(new string('=', 120));
            sb.AppendLine($"Total Equipment: {equipment.Count()}");
            
            var groupedByStatus = equipment.GroupBy(e => e.Status);
            sb.AppendLine("\nBreakdown by Status:");
            foreach (var group in groupedByStatus)
            {
                sb.AppendLine($"  {group.Key}: {group.Count()}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public async Task<byte[]> GenerateAuditLogReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var logs = await _unitOfWork.AuditLogs.GetAllAsync();
            
            if (startDate.HasValue)
            {
                logs = logs.Where(l => l.ChangedAt >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                logs = logs.Where(l => l.ChangedAt <= endDate.Value);
            }

            var sb = new StringBuilder();
            
            sb.AppendLine("AUDIT LOG REPORT");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            if (startDate.HasValue && endDate.HasValue)
            {
                sb.AppendLine($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
            }
            sb.AppendLine(new string('=', 120));
            sb.AppendLine();
            sb.AppendLine($"{"Date",-20} {"Table",-20} {"Action",-10} {"RecordID",-10} {"User",-20}");
            sb.AppendLine(new string('-', 120));

            foreach (var log in logs.OrderByDescending(l => l.ChangedAt))
            {
                sb.AppendLine($"{log.ChangedAt:yyyy-MM-dd HH:mm:ss,-20} {log.TableName,-20} {log.Action,-10} {log.RecordId,-10} {log.ChangedBy,-20}");
            }

            sb.AppendLine(new string('=', 120));
            sb.AppendLine($"Total Log Entries: {logs.Count()}");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}