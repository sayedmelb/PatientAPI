
namespace PatientAPI.Infrastructure.Configuration
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string PatientsCollectionName { get; set; } = string.Empty;
        public string PrescriptionsCollectionName { get; set; } = string.Empty;
    }
}
