using PharmacyManager.Services.Contracts;
using PharmacyManager.Services.Implementations;
using PharmacyManager.Services.Implenetations;
using PharmacyManager.Services.Implenetations.PharmacyManager.Services.Implementations;

namespace PharmacyManager.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService,AuthService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IManufacturerService, ManufacturerService>();

            services.AddScoped<IMedicamentService, MedicamentService>();

            services.AddScoped<IPrescriptionService, PrescriptionService>();

            services.AddScoped<IStatisticsService, StatisticsService>();
            return services;         
        }
    }
}
