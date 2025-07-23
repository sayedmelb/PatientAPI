using PatientAPI.Domain.Common;

namespace PatientAPI.Infrastructure.Services
{
    public interface IDataSeedingService
    {
        Task<Result> SeedDatabaseAsync();
        Task<Result> ClearDatabaseAsync();
        Task<Result> SeedPatientsAsync(int count = 10);
        Task<Result> SeedPrescriptionsAsync(int count = 20);
        Task<bool> IsDatabaseSeededAsync();
    }
}
