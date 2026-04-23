using Microsoft.EntityFrameworkCore;
using PharmacyManager.Data;
using PharmacyManager.DTOs;
using PharmacyManager.Models;
using PharmacyManager.Services.Contracts;

namespace PharmacyManager.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(PrescriptionDto dto, Guid currentUserId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.EGN == dto.PatientEgn);

            if (patient == null)
            {
                patient = new Patient { FullName = dto.PatientName, EGN = dto.PatientEgn };
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }

            var prescription = new Prescription
            {
                DoctorName = dto.DoctorName ?? "System Doctor",
                PatientId = patient.Id,
                CreatedAt = DateTime.UtcNow,
                UserId = currentUserId,
                PrescriptionItems = new List<PrescriptionItem>() // Ensure it's initialized
            };

            // If 'Items' is empty but the main DTO has a single medicament ID, add it to the list
            if ((dto.Items == null || !dto.Items.Any()) && dto.MedicamentId != 0)
            {
                dto.Items = new List<PrescriptionItemDto>
        {
            new PrescriptionItemDto { MedicamentId = dto.MedicamentId, Quantity = dto.Quantity }
        };
            }

            if (dto.Items == null || !dto.Items.Any()) return false; // Still nothing? Stop.

            decimal totalValue = 0;

            foreach (var itemDto in dto.Items)
            {
                var medicament = await _context.Medicaments.FindAsync(itemDto.MedicamentId);

                // Check if medicament exists and has enough stock
                if (medicament == null || medicament.StockQuantity < itemDto.Quantity)
                {
                    return false;
                }

                // Subtract stock right here in the service!
                medicament.StockQuantity -= itemDto.Quantity;

                var pItem = new PrescriptionItem
                {
                    MedicamentId = medicament.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = medicament.Price
                };

                prescription.PrescriptionItems.Add(pItem);
                totalValue += (medicament.Price * itemDto.Quantity);
            }

            prescription.TotalValue = totalValue;
            _context.Prescriptions.Add(prescription);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetAveragePrescriptionValueAsync()
        {
            if (!await _context.Prescriptions.AnyAsync()) return 0;
            return await _context.Prescriptions.AverageAsync(p => p.TotalValue);
        }

        public async Task<Dictionary<string, int>> GetTopSellingMedicamentsAsync(int count)
        {
            var topItems = await _context.PrescriptionItems
                .GroupBy(pi => pi.Medicament.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .ToListAsync();

            return topItems.ToDictionary(x => x.Name, x => x.TotalSold);
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Select(p => new PrescriptionDto
                {
                    Id = p.Id,
                    DoctorName = p.DoctorName,
                    PatientName = p.Patient.FullName,
                    TotalValue = p.TotalValue,
                    IssuedDate = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PrescriptionDto?> GetByIdAsync(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.PrescriptionItems)
                    .ThenInclude(pi => pi.Medicament)
                .Where(p => p.Id == id)
                .Select(p => new PrescriptionDto
                {
                    Id = p.Id,
                    DoctorName = p.DoctorName,
                    PatientName = p.Patient.FullName,
                    PatientEgn = p.Patient.EGN,
                    TotalValue = p.TotalValue,
                    IssuedDate = p.CreatedAt,
                    Items = p.PrescriptionItems.Select(pi => new PrescriptionItemDto
                    {
                        MedicamentId = pi.MedicamentId,
                        MedicamentName = pi.Medicament.Name,
                        Quantity = pi.Quantity,
                        UnitPrice = pi.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                await _context.SaveChangesAsync();
            }
        }

    }
}