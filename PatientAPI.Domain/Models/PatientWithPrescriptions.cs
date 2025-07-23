using PatientAPI.Domain.Entities;

namespace PatientAPI.Domain.Models
{
    public class PatientWithPrescriptions
    {
        public Patient Patient { get; set; } = new Patient();
        public IReadOnlyList<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
