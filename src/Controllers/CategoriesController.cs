using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManager.DTOs;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _s;
        public CategoriesController(ICategoryService s) => _s = s;
        public async Task<IActionResult> Index() => View(await _s.GetAllAsync());
        [HttpPost] public async Task<IActionResult> Create(CategoryDto d) { await _s.CreateAsync(d); return RedirectToAction("Index"); }
        [HttpPost] public async Task<IActionResult> Delete(int id) { await _s.DeleteAsync(id); return RedirectToAction("Index"); }
    }
}
