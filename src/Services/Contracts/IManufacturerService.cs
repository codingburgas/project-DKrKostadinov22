using PharmacyManager.DTOs;

namespace PharmacyManager.Services.Contracts
{
    public interface IManufacturerService
    {
        Task<IEnumerable<ManufacturerDto>> GetAllAsync();
        Task<ManufacturerDto?> GetByIdAsync(int id);
        Task CreateAsync(ManufacturerDto dto);
        Task UpdateAsync(ManufacturerDto dto);
        Task DeleteAsync(int id);
    }
}
