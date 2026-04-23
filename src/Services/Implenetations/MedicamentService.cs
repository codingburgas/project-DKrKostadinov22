using Microsoft.EntityFrameworkCore;
using PharmacyManager.Data;
using PharmacyManager.DTOs;
using PharmacyManager.Models;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Services.Implementations
{
    public class MedicamentService : IMedicamentService
    {
        private readonly ApplicationDbContext _context;

        public MedicamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicamentDto>> GetAllAsync()
        {
            return await _context.Medicaments
                .Include(m => m.Manufacturer)
                .Select(m => new MedicamentDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    StockQuantity = m.StockQuantity,
                    ExpiryDate = m.ExpiryDate, // Вече е DateTime -> DateTime (OK)
                    ManufacturerId = m.ManufacturerId,
                    ManufacturerName = m.Manufacturer.Name
                })
                .ToListAsync();
        }

        public async Task<MedicamentDto?> GetByIdAsync(int id)
        {
            return await _context.Medicaments
                .Include(m => m.MedicamentCategories)
                .Include(m => m.Manufacturer)
                .Where(m => m.Id == id)
                .Select(m => new MedicamentDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    StockQuantity = m.StockQuantity,
                    Price = m.Price,
                    ExpiryDate = m.ExpiryDate, // ПРЕМАХНАТО: .ToString()
                    ManufacturerId = m.ManufacturerId,
                    ManufacturerName = m.Manufacturer.Name,
                    CategoryIds = m.MedicamentCategories.Select(mc => mc.CategoryId).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MedicamentDto dto)
        {
            var medicament = new Medicament
            {
                Name = dto.Name,
                StockQuantity = dto.StockQuantity,
                Price = dto.Price,
                ExpiryDate = dto.ExpiryDate, // ПРЕМАХНАТО: DateTime.Parse()
                ManufacturerId = dto.ManufacturerId // НЕ ЗАБРАВЯЙ ТОВА
            };

            if (dto.CategoryIds != null)
            {
                foreach (var catId in dto.CategoryIds)
                {
                    medicament.MedicamentCategories.Add(new MedicamentCategory
                    {
                        CategoryId = catId
                    });
                }
            }

            _context.Medicaments.Add(medicament);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MedicamentDto dto)
        {
            var medicament = await _context.Medicaments
                .Include(m => m.MedicamentCategories)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (medicament == null) return;

            medicament.Name = dto.Name;
            medicament.StockQuantity = dto.StockQuantity;
            medicament.Price = dto.Price;
            medicament.ExpiryDate = dto.ExpiryDate; // ПРЕМАХНАТО: DateTime.Parse()
            medicament.ManufacturerId = dto.ManufacturerId;

            medicament.MedicamentCategories.Clear();
            if (dto.CategoryIds != null)
            {
                foreach (var catId in dto.CategoryIds)
                {
                    medicament.MedicamentCategories.Add(new MedicamentCategory
                    {
                        CategoryId = catId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medicament = await _context.Medicaments.FindAsync(id);
            if (medicament != null)
            {
                _context.Medicaments.Remove(medicament);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MedicamentDto>> GetLowStockAlertsAsync()
        {
            return await _context.Medicaments
                .Include(m => m.Manufacturer)
                .Where(m => m.StockQuantity < 10)
                .OrderBy(m => m.StockQuantity)
                .Select(m => new MedicamentDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    StockQuantity = m.StockQuantity,
                    Price = m.Price,
                    ExpiryDate = m.ExpiryDate,
                    ManufacturerName = m.Manufacturer.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicamentDto>> GetExpiredMedicamentsAsync()
        {
            return await _context.Medicaments
                .Include(m => m.Manufacturer)
                .Where(m => m.ExpiryDate <= DateTime.UtcNow)
                .Select(m => new MedicamentDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    ExpiryDate = m.ExpiryDate, // ПРЕМАХНАТО: .ToString()
                    StockQuantity = m.StockQuantity,
                    ManufacturerName = m.Manufacturer.Name
                })
                .ToListAsync();
        }

        public async Task ReduceStockAsync(int medicamentId, int quantityToReduce)
        {
            var medicament = await _context.Medicaments.FindAsync(medicamentId);
            if (medicament != null)
            {
                // CHANGE 'Quantity' TO THE NAME IN YOUR Models/Medicament.cs
                medicament.StockQuantity -= quantityToReduce;

                if (medicament.StockQuantity < 0) medicament.StockQuantity = 0;

                await _context.SaveChangesAsync();
            }
        }
    }
}