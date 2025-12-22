using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize(Roles = "Admin,Captain")]
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GenerateFirefighterReport()
        {
            var report = await _reportService.GenerateFirefighterReportAsync();
            return File(report, "text/plain", $"Firefighter_Report_{DateTime.Now:yyyyMMdd}.txt");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateStationReport()
        {
            var report = await _reportService.GenerateStationReportAsync();
            return File(report, "text/plain", $"Station_Report_{DateTime.Now:yyyyMMdd}.txt");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateIncidentReport(DateTime? startDate, DateTime? endDate)
        {
            var report = await _reportService.GenerateIncidentReportAsync(startDate, endDate);
            return File(report, "text/plain", $"Incident_Report_{DateTime.Now:yyyyMMdd}.txt");
        }


        [HttpPost]
        public async Task<IActionResult> GenerateEquipmentReport()
        {
            var report = await _reportService.GenerateEquipmentReportAsync();
            return File(report, "text/plain", $"Equipment_Report_{DateTime.Now:yyyyMMdd}.txt");
        }


        [HttpPost]
        public async Task<IActionResult> GenerateAuditLogReport(DateTime? startDate, DateTime? endDate)
        {
            var report = await _reportService.GenerateAuditLogReportAsync(startDate, endDate);
            return File(report, "text/plain", $"AuditLog_Report_{DateTime.Now:yyyyMMdd}.txt");
        }
    }
}