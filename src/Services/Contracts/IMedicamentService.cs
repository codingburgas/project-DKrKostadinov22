using PharmacyManager.DTOs;

namespace PharmacyManager.Services.Contracts
{
    public interface IMedicamentService
    {
        // CRUD Operations
        Task<IEnumerable<MedicamentDto>> GetAllAsync();
        Task<MedicamentDto?> GetByIdAsync(int id);
        Task CreateAsync(MedicamentDto model);
        Task UpdateAsync(MedicamentDto model);
        Task DeleteAsync(int id);

        // Required Statistics & Alerts
        Task<IEnumerable<MedicamentDto>> GetLowStockAlertsAsync();
        Task<IEnumerable<MedicamentDto>> GetExpiredMedicamentsAsync();

        Task ReduceStockAsync(int medicamentId, int quantityToReduce);
    }
}
