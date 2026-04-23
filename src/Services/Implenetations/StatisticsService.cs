using Microsoft.EntityFrameworkCore;
using PharmacyManager.Data;
using PharmacyManager.DTOs;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardStatisticsAsync(Guid userId)
        {
            var stats = new DashboardDto();

            // 1. Общ брой лекарства в склада (остава глобален, защото складът е общ)
            stats.TotalMedicaments = await _context.Medicaments.CountAsync();

            // 2. Ниски наличности (също глобално за аптеката)
            stats.LowStockCount = await _context.Medicaments.CountAsync(m => m.StockQuantity < 10);

            // 3. СРЕДНА СТОЙНОСТ НА ТВОИТЕ РЕЦЕПТИ (Филтрирано по User)
            var userPrescriptions = _context.Prescriptions.Where(p => p.UserId == userId);
            if (await userPrescriptions.AnyAsync())
            {
                // Кастваме към double, за да може SQLite да го сметне, 
                // след което връщаме резултата към decimal за твоя DTO/Model
                // Inside StatisticsService
                var values = await _context.Prescriptions.Select(p => p.TotalValue).ToListAsync();
                stats.AveragePrescriptionValue = values.Any() ? values.Average() : 0;
            }

            // 4. ТВОИТЕ НАЙ-ПРОДАВАНИ МЕДИКАМЕНТИ (Филтрирано по User)
            stats.TopSellingMedicaments = await _context.PrescriptionItems
                .Where(pi => pi.Prescription.UserId == userId) // Връзка през рецептата
                .GroupBy(pi => pi.Medicament.Name)
                .Select(g => new { Name = g.Key, Count = g.Sum(x => x.Quantity) })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToDictionaryAsync(x => x.Name, x => x.Count);

            return stats;
        }
    }
}