using PharmacyManager.DTOs;

namespace PharmacyManager.Services.Contracts
{
    public interface IStatisticsService
    {
        Task<DashboardDto> GetDashboardStatisticsAsync(Guid userId);
    }
}
