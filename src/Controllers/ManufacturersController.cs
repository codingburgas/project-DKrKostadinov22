using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManager.DTOs;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManufacturersController : Controller
    {
        private readonly IManufacturerService _s;
        public ManufacturersController(IManufacturerService s) => _s = s;
        public async Task<IActionResult> Index() => View(await _s.GetAllAsync());
        [HttpPost] public async Task<IActionResult> Create(ManufacturerDto d) { await _s.CreateAsync(d); return RedirectToAction("Index"); }
        [HttpPost] public async Task<IActionResult> Delete(int id) { await _s.DeleteAsync(id); return RedirectToAction("Index"); }
    }
}
