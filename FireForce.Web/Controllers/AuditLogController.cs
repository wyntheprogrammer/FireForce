using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLogController : BaseController
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async Task<IActionResult> Index(string? tableName = null)
        {
            IEnumerable<Application.DTOs.AuditLogDTO> logs;

            if (!string.IsNullOrEmpty(tableName))
            {
                logs = await _auditLogService.GetByTableNameAsync(tableName);
                ViewBag.FilteredTable = tableName;
            }
            else
            {
                logs = await _auditLogService.GetAllAsync();
            }

            return View(logs);
        }

        public async Task<IActionResult> ByUser(string username)
        {
            var logs = await _auditLogService.GetByUserAsync(username);
            ViewBag.FilteredUser = username;
            return View("Index", logs);
        }
    }
}