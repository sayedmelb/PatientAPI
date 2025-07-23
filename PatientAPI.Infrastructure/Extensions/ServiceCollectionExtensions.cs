using Microsoft.Extensions.DependencyInjection;
using PatientAPI.Infrastructure.Services;


namespace PatientAPI.Infrastructure.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataSeeding(this IServiceCollection services)
        {
            services.AddScoped<IDataSeedingService, DataSeedingService>();
            return services;
        }
    }
}
