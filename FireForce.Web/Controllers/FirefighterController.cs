using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize]
    public class FirefighterController : BaseController
    {
        private readonly IFirefighterService _firefighterService;
        private readonly IStationService _stationService;

        public FirefighterController(IFirefighterService firefighterService, IStationService stationService)
        {
            _firefighterService = firefighterService;
            _stationService = stationService;
        }

        public async Task<IActionResult> Index()
        {
            var firefighters = await _firefighterService.GetAllAsync();
            return View(firefighters);
        }


        public async Task<IActionResult> Details(int id)
        {
            var firefighter = await _firefighterService.GetByIdAsync(id);
            if (firefighter == null)
            {
                ShowErrorMessage("Firefighter not found.");
                return RedirectToAction(nameof(Index));
            }
            return View(firefighter);
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
        public async Task<IActionResult> Create(FirefighterDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }

            await _firefighterService.CreateAsync(model, CurrentUsername);
            ShowSuccessMessage("Firefighter created successfully!");
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin, Captain")]
        public async Task<IActionResult> Edit(int id)
        {
            var firefighter = await _firefighterService.GetByIdAsync(id);
            if (firefighter == null)
            {
                ShowErrorMessage("Firefighter not found.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(firefighter);
        }


        [Authorize(Roles = "Admin,Captain")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FirefighterDTO model)
        {
            if (id != model.Id)
            {
                ShowErrorMessage("Invalid firefighter ID.");
                return RedirectToAction(nameof(Index));
            }


            if(!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }


            var result = await _firefighterService.UpdateAsync(model, CurrentUsername);
            if (result)
            {
                ShowSuccessMessage("Firefighter updated successfully!");
                return RedirectToAction(nameof(Index));
            }

            ShowErrorMessage("Failed to update firefighter.");
            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var firefighter = await _firefighterService.GetByIdAsync(id);
            if (firefighter == null)
            {
                ShowErrorMessage("Firefighter not found.");
                return RedirectToAction(nameof(Index));
            }

            return View(firefighter);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _firefighterService.DeleteAsync(id, CurrentUsername);
            if(result)
            {
                ShowSuccessMessage("Firefighter deleted successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to delete firefighter.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}