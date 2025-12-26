using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize(Roles = "Admin, Captain")]
    public class AnalyticsController :BaseController
    {
        private readonly IFirefighterService _firefighterService;
        private readonly IStationService _stationService;
        private readonly IIncidentService _incidentService;
        private readonly IEquipmentService _equipmentService;

        public AnalyticsController(
            IFirefighterService firefighterService, IStationService stationService, IIncidentService incidentService, IEquipmentService equipmentService)
        {
            _firefighterService = firefighterService;
            _stationService = stationService;
            _incidentService = incidentService;
            _equipmentService = equipmentService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Firefighters = await _firefighterService.GetAllAsync();
            ViewBag.Stations = await _stationService.GetAllAsync();
            ViewBag.Incidents = await _incidentService.GetAllAsync();
            ViewBag.Equipment = await _equipmentService.GetAllAsync();

            return View();
        }

        public async Task<IActionResult> Firefighters()
        {
            var firefighters = await _firefighterService.GetAllAsync();
            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(firefighters);
        }

        public async Task<IActionResult> Incident()
        {
            var incidents = await _incidentService.GetAllAsync();
            return View(incidents);
        }

        public async Task<IActionResult> Equipment()
        {
            var equipment = await _equipmentService.GetAllAsync();
            return View(equipment);
        }

    }
}
