using PharmacyManager.Data;
using PharmacyManager.DTOs;
using PharmacyManager.Models;
using PharmacyManager.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace PharmacyManager.Services.Implenetations
{

    namespace PharmacyManager.Services.Implementations
    {
        public class CategoryService : ICategoryService
        {
            private readonly ApplicationDbContext _context;

            public CategoryService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<CategoryDto>> GetAllAsync()
            {
                return await _context.Categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        // LINQ: Преброяване на свързаните лекарства
                        MedicamentsCount = c.MedicamentCategories.Count
                    })
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }

            public async Task<CategoryDto?> GetByIdAsync(int id)
            {
                var category = await _context.Categories
                    .Include(c => c.MedicamentCategories)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null) return null;

                return new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    MedicamentsCount = category.MedicamentCategories.Count
                };
            }

            public async Task CreateAsync(CategoryDto dto)
            {
                var category = new Category
                {
                    Name = dto.Name
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(CategoryDto dto)
            {
                var category = await _context.Categories.FindAsync(dto.Id);
                if (category != null)
                {
                    category.Name = dto.Name;
                    await _context.SaveChangesAsync();
                }
            }

            public async Task DeleteAsync(int id)
            {
                var category = await _context.Categories.FindAsync(id);
                if (category != null)
                {
                    // Проверка: Не трием категория, ако има лекарства в нея (бизнес логика)
                    bool hasMedicaments = await _context.MedicamentCategories.AnyAsync(mc => mc.CategoryId == id);
                    if (!hasMedicaments)
                    {
                        _context.Categories.Remove(category);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
