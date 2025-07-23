
namespace PatientAPI.Application.DTOs
{
    public class PatientWithPrescriptionsDto
    {
        public PatientDto Patient { get; set; } = new PatientDto();
        public IReadOnlyList<PrescriptionDto> Prescriptions { get; set; } = new List<PrescriptionDto>();
    }
}
