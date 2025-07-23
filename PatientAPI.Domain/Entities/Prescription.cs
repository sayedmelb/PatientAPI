
namespace PatientAPI.Domain.Entities
{
    public class Prescription
    {
        public string? MongoId { get; set; }
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string DrugName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public DateTime DatePrescribed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Domain methods
        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsExpired(int validityDays = 30)
        {
            return DatePrescribed.AddDays(validityDays) < DateTime.UtcNow;
        }
    }
}
