using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize]
    public class StationController : BaseController
    {
        private readonly IStationService _stationService;

        public StationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        public async Task<IActionResult> Index()
        {
            var stations = await _stationService.GetAllAsync();
            return View(stations);
        }


        public async Task<IActionResult> Details(int id)
        {
            var station = await _stationService.GetByIdAsync(id);
            if (station == null)
            {
                ShowErrorMessage("Station not found.");
                return RedirectToAction(nameof(Index));
            }
            return View(station);
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
        public async Task<IActionResult> Create(StationDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }

            await _stationService.CreateAsync(model, CurrentUsername);
            ShowSuccessMessage("Station created successfully!");
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin, Captain")]
        public async Task<IActionResult> Edit(int id)
        {
            var station = await _stationService.GetByIdAsync(id);
            if (station == null)
            {
                ShowErrorMessage("Station not found.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(station);
        }


        [Authorize(Roles = "Admin,Captain")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StationDTO model)
        {
            if (id != model.Id)
            {
                ShowErrorMessage("Invalid station ID.");
                return RedirectToAction(nameof(Index));
            }


            if(!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }


            var result = await _stationService.UpdateAsync(model, CurrentUsername);
            if (result)
            {
                ShowSuccessMessage("Station updated successfully!");
                return RedirectToAction(nameof(Index));
            }

            ShowErrorMessage("Failed to update station.");
            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var station = await _stationService.GetByIdAsync(id);
            if (station == null)
            {
                ShowErrorMessage("Station not found.");
                return RedirectToAction(nameof(Index));
            }

            return View(station);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _stationService.DeleteAsync(id, CurrentUsername);
            if(result)
            {
                ShowSuccessMessage("Station deleted successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to delete station.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}