using PharmacyManager.DTOs;

namespace PharmacyManager.Services.Contracts
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto?> GetByIdAsync(int id); // Сменено на int
        Task<bool> CreateAsync(PrescriptionDto dto, Guid currentUserId);
        Task DeleteAsync(int id); // Сменено на int
        Task<decimal> GetAveragePrescriptionValueAsync();
        Task<Dictionary<string, int>> GetTopSellingMedicamentsAsync(int count);
    }
}