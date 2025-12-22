using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize]
    public class IncidentController : BaseController
    {
        private readonly IIncidentService _incidentService;
        private readonly IStationService _stationService;

        public IncidentController(IIncidentService incidentService, IStationService stationService)
        {
            _incidentService = incidentService;
            _stationService = stationService;
        }

        public async Task<IActionResult> Index()
        {
            var incidents = await _incidentService.GetAllAsync();
            return View(incidents);
        }


        public async Task<IActionResult> Details(int id)
        {
            var incident = await _incidentService.GetByIdAsync(id);
            if (incident == null)
            {
                ShowErrorMessage("Incident not found.");
                return RedirectToAction(nameof(Index));
            }
            return View(incident);
        }

        [Authorize(Roles = "Admin,Captain")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Stations = await _stationService.GetAllAsync();
            return View();
        }


        [Authorize(Roles = "Admin,Captain")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }

            await _incidentService.CreateAsync(model, CurrentUsername);
            ShowSuccessMessage("Incident created successfully!");
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin, Captain")]
        public async Task<IActionResult> Edit(int id)
        {
            var incident = await _incidentService.GetByIdAsync(id);
            if (incident == null)
            {
                ShowErrorMessage("Incident not found.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(incident);
        }


        [Authorize(Roles = "Admin,Captain")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IncidentDTO model)
        {
            if (id != model.Id)
            {
                ShowErrorMessage("Invalid incident ID.");
                return RedirectToAction(nameof(Index));
            }


            if(!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }


            var result = await _incidentService.UpdateAsync(model, CurrentUsername);
            if (result)
            {
                ShowSuccessMessage("Incident updated successfully!");
                return RedirectToAction(nameof(Index));
            }

            ShowErrorMessage("Failed to update incident.");
            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var incident = await _incidentService.GetByIdAsync(id);
            if (incident == null)
            {
                ShowErrorMessage("Incident not found.");
                return RedirectToAction(nameof(Index));
            }

            return View(incident);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _incidentService.DeleteAsync(id, CurrentUsername);
            if(result)
            {
                ShowSuccessMessage("Incident deleted successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to delete incident.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}