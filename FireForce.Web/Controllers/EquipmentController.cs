using FireForce.Application.DTOs;
using FireForce.Application.Interfaces;
using FireForce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FireForce.Web.Controllers
{
    [Authorize]
    public class EquipmentController : BaseController
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IStationService _stationService;

        public EquipmentController(IEquipmentService equipmentService, IStationService stationService)
        {
            _equipmentService = equipmentService;
            _stationService = stationService;
        }

        public async Task<IActionResult> Index()
        {
            var equipments = await _equipmentService.GetAllAsync();
            return View(equipments);
        }


        public async Task<IActionResult> Details(int id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);
            if (equipment == null)
            {
                ShowErrorMessage("Equipment not found.");
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
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
        public async Task<IActionResult> Create(EquipmentDTO model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }

            await _equipmentService.CreateAsync(model, CurrentUsername);
            ShowSuccessMessage("Equipment created successfully!");
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin, Captain")]
        public async Task<IActionResult> Edit(int id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);
            if (equipment == null)
            {
                ShowErrorMessage("Equipment not found.");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(equipment);
        }


        [Authorize(Roles = "Admin,Captain")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EquipmentDTO model)
        {
            if (id != model.Id)
            {
                ShowErrorMessage("Invalid equipment ID.");
                return RedirectToAction(nameof(Index));
            }


            if(!ModelState.IsValid)
            {
                ViewBag.Stations = await _stationService.GetAllAsync();
                return View(model);
            }


            var result = await _equipmentService.UpdateAsync(model, CurrentUsername);
            if (result)
            {
                ShowSuccessMessage("Equipment updated successfully!");
                return RedirectToAction(nameof(Index));
            }

            ShowErrorMessage("Failed to update equipment.");
            ViewBag.Stations = await _stationService.GetAllAsync();
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _equipmentService.GetByIdAsync(id);
            if (equipment == null)
            {
                ShowErrorMessage("Equipment not found.");
                return RedirectToAction(nameof(Index));
            }

            return View(equipment);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _equipmentService.DeleteAsync(id, CurrentUsername);
            if(result)
            {
                ShowSuccessMessage("Equipment deleted successfully!");
            }
            else
            {
                ShowErrorMessage("Failed to delete equipment.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}