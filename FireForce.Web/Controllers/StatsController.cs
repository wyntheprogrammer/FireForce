using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize]
    public class StatsController : BaseController
    {
        private readonly IFirefighterService _firefighterService;
        private readonly IIncidentService _incidentService;
        private readonly IEquipmentService _equipmentService;

        public StatsController(
            IFirefighterService firefighterService,
            IIncidentService incidentService,
            IEquipmentService equipmentService)
        {
            _firefighterService = firefighterService;
            _incidentService = incidentService;
            _equipmentService = equipmentService;
        }

        [HttpGet]
        [Route("api/station/{stationId}/stats")]
        public async Task<IActionResult> GetStationStats(int stationId)
        {
            try
            {
                var firefighters = await _firefighterService.GetByStationIdAsync(stationId);
                var equipment = await _equipmentService.GetByStationIdAsync(stationId);
                var incidents = await _incidentService.GetByStationIdAsync(stationId);

                var firefightersByRank = firefighters
                    .GroupBy(f => f.Rank)
                    .Select(g => new { Rank = g.Key, Count = g.Count() })
                    .ToList();

                var equipmentByType = equipment
                    .GroupBy(e => e.Type)
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToList();

                return Json(new
                {
                    firefighters = firefightersByRank,
                    equipment = equipmentByType,
                    totalFirefighters = firefighters.Count(),
                    totalEquipment = equipment.Count(),
                    totalIncidents = incidents.Count()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message,
                    firefighters = new List<object>(),
                    equipment = new List<object>(),
                    totalFirefighters = 0,
                    totalEquipment = 0,
                    totalIncidents = 0
                });
            }
        }

        // DEBUG ACTION - Remove after testing
        [HttpGet]
        [Route("api/debug/all-data")]
        public async Task<IActionResult> DebugAllData()
        {
            try
            {
                var allFirefighters = await _firefighterService.GetAllAsync();
                var allEquipment = await _equipmentService.GetAllAsync();
                var allIncidents = await _incidentService.GetAllAsync();

                return Json(new
                {
                    totalFirefighters = allFirefighters.Count(),
                    totalEquipment = allEquipment.Count(),
                    totalIncidents = allIncidents.Count(),
                    firefighters = allFirefighters.Select(f => new {
                        f.Id,
                        f.FirstName,
                        f.LastName,
                        f.StationId,
                        f.StationName,
                        f.Rank
                    }),
                    equipment = allEquipment.Select(e => new {
                        e.Id,
                        e.Name,
                        e.StationId,
                        e.StationName,
                        e.Type
                    }),
                    incidents = allIncidents.Select(i => new {
                        i.Id,
                        i.IncidentNumber,
                        i.StationId,
                        i.StationName
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        [Route("api/firefighter/{firefighterId}/stats")]
        public async Task<IActionResult> GetFirefighterStats(int firefighterId)
        {
            try
            {
                var firefighter = await _firefighterService.GetByIdAsync(firefighterId);
                if (firefighter == null)
                    return NotFound();

                // Get incidents for the firefighter's station
                var incidents = firefighter.StationId.HasValue
                    ? await _incidentService.GetByStationIdAsync(firefighter.StationId.Value)
                    : new List<Application.DTOs.IncidentDTO>();

                var recentIncidents = incidents
                    .Where(i => i.IncidentDate >= DateTime.UtcNow.AddMonths(-3))
                    .Count();

                // Calculate average response time
                var responseTimes = incidents
                    .Where(i => i.ResponseTime.HasValue)
                    .Select(i => (i.ResponseTime.Value - i.IncidentDate).TotalMinutes)
                    .ToList();

                var avgResponseTime = responseTimes.Any() ? responseTimes.Average() : 6;

                return Json(new
                {
                    incidentCount = recentIncidents,
                    responseTime = avgResponseTime,
                    trainingHours = 120, // Would need separate table to track
                    rating = 4.5 // Would need separate table to track
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message,
                    incidentCount = 0,
                    responseTime = 0,
                    trainingHours = 0,
                    rating = 0
                });
            }
        }
    }
}