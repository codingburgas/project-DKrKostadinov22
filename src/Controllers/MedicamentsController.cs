using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmacyManager.DTOs;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Controllers
{
    [Authorize]
    public class MedicamentsController : Controller
    {
        private readonly IMedicamentService _medicamentService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;

        public MedicamentsController(IMedicamentService ms, ICategoryService cs, IManufacturerService mans)
        {
            _medicamentService = ms;
            _categoryService = cs;
            _manufacturerService = mans;
        }

        public async Task<IActionResult> Index() => View(await _medicamentService.GetAllAsync());

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await LoadData();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken] // Recommended for security
        public async Task<IActionResult> Create(MedicamentDto dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Actually call your service to save the data!
                    await _medicamentService.CreateAsync(dto);

                    // 2. Redirect to Index so the user sees the new item
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // If the database fails (e.g. unique constraint), add an error
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                }
            }

            // 3. If we got here, something failed. Reload dropdowns and show errors.
            await LoadData();
            return View(dto);
        }



        public async Task<IActionResult> LowStock()
            => View("Index", await _medicamentService.GetLowStockAlertsAsync());

        public async Task<IActionResult> Expired()
            => View("Index", await _medicamentService.GetExpiredMedicamentsAsync());

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _medicamentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadData()
        {
            ViewBag.Categories = new MultiSelectList(await _categoryService.GetAllAsync(), "Id", "Name");
            ViewBag.Manufacturers = new SelectList(await _manufacturerService.GetAllAsync(), "Id", "Name");
        }

        // Добави това в MedicamentsController.cs
        public async Task<IActionResult> Details(int id)
        {
            var medicament = await _medicamentService.GetByIdAsync(id);

            if (medicament == null)
            {
                return NotFound(); // Връща 404, ако лекарството не съществува в базата
            }

            return View(medicament);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var medicament = await _medicamentService.GetByIdAsync(id);
            if (medicament == null)
            {
                return NotFound();
            }

            // Map your Model to DTO if your Service returns a Model
            // If GetByIdAsync already returns a MedicamentDto, just pass 'medicament'
            await LoadData(); // To reload the Categories/Manufacturers dropdowns
            return View(medicament);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicamentDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _medicamentService.UpdateAsync(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                }
            }

            await LoadData();
            return View(dto);
        }
    }
}