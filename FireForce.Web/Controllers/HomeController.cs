using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FireForce.Web.Models;

namespace FireForce.Web.Controllers;


[Authorize]
public class HomeController : BaseController
{
    private readonly IFirefighterService _firefighterService;
    private readonly IStationService _stationService;
    private readonly IIncidentService _incidentService;
    private readonly IEquipmentService _equipmentService;

    public HomeController (
        IFirefighterService firefighterService,
        IStationService stationService,
        IIncidentService incidentService,
        IEquipmentService equipmentService)
    {
        _firefighterService = firefighterService;
        _stationService = stationService;
        _incidentService = incidentService;
        _equipmentService = equipmentService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalFirefighters = (await _firefighterService.GetAllAsync()).Count();
        ViewBag.TotalStations = (await _stationService.GetAllAsync()).Count();
        ViewBag.TotalIncidents = (await _incidentService.GetAllAsync()).Count();
        ViewBag.TotalEquipment = (await _equipmentService.GetAllAsync()).Count();

        var recentIncidents = (await _incidentService.GetAllAsync()).Take(5);
        return View(recentIncidents);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}

    

// public class HomeController : Controller
// {

//     private readonly ILogger<HomeController> _logger;

//     public HomeController(ILogger<HomeController> logger)
//     {
//         _logger = logger;
//     }

//     public IActionResult Index()
//     {
//         return View();
//     }

//     public IActionResult Privacy()
//     {
//         return View();
//     }

//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }
// }
