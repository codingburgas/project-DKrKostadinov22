using Microsoft.EntityFrameworkCore;
using PharmacyManager.Data;
using PharmacyManager.DTOs;
using PharmacyManager.Models;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Services.Implementations
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly ApplicationDbContext _context;

        public ManufacturerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ManufacturerDto>> GetAllAsync()
        {
            return await _context.Manufacturers
                .Select(m => new ManufacturerDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Address = m.Address
                })
                .ToListAsync();
        }

        public async Task<ManufacturerDto?> GetByIdAsync(int id)
        {
            var m = await _context.Manufacturers.FindAsync(id);
            if (m == null) return null;

            return new ManufacturerDto { Id = m.Id, Name = m.Name, Address = m.Address };
        }

        public async Task CreateAsync(ManufacturerDto dto)
        {
            var manufacturer = new Manufacturer { Name = dto.Name, Address = dto.Address };
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ManufacturerDto dto)
        {
            var m = await _context.Manufacturers.FindAsync(dto.Id);
            if (m != null)
            {
                m.Name = dto.Name;
                m.Address = dto.Address;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var m = await _context.Manufacturers.FindAsync(id);
            if (m != null)
            {
                _context.Manufacturers.Remove(m);
                await _context.SaveChangesAsync();
            }
        }
    }
}