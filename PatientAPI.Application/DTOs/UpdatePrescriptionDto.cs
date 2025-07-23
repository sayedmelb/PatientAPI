using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Application.DTOs
{
    public class UpdatePrescriptionDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string DrugName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        public DateTime DatePrescribed { get; set; }
    }
}
