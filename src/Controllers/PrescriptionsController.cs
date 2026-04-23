using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmacyManager.DTOs;
using PharmacyManager.Models;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Controllers
{
    [Authorize]
    public class PrescriptionsController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicamentService _medicamentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PrescriptionsController(
            IPrescriptionService prescriptionService,
            IMedicamentService medicamentService,
            UserManager<ApplicationUser> userManager)
        {
            _prescriptionService = prescriptionService;
            _medicamentService = medicamentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Показва всички рецепти
            var prescriptions = await _prescriptionService.GetAllAsync();
            return View(prescriptions);
        }

        public async Task<IActionResult> Create()
        {
            var medicaments = await _medicamentService.GetAllAsync();
            ViewBag.Medicaments = new SelectList(medicaments, "Id", "Name");
            return View(new PrescriptionDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionDto dto)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                if (userId != null)
                {
                    // The service now handles creating the patient, the items, 
                    // the total price, AND reducing the stock.
                    bool success = await _prescriptionService.CreateAsync(dto, Guid.Parse(userId));

                    if (success)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ModelState.AddModelError("", "Insufficient stock or invalid medicament selection.");
            }

            var medicaments = await _medicamentService.GetAllAsync();
            ViewBag.Medicaments = new SelectList(medicaments, "Id", "Name");
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _prescriptionService.GetByIdAsync(id);
            if (prescription == null) return NotFound();
            return View(prescription);
        }
    }
}