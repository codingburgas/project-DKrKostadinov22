using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyManager.Models;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Controllers
{
    [Authorize] // Вече е достъпно за всички логнати
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(IStatisticsService statsService, UserManager<ApplicationUser> userManager)
        {
            _statsService = statsService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Взимаме ID на текущия потребител
            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return Challenge();

            Guid userId = Guid.Parse(userIdString);

            var stats = await _statsService.GetDashboardStatisticsAsync(userId);
            return View(stats);
        }
    }
}