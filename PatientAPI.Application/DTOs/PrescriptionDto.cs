using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Application.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string DrugName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Dosage { get; set; } = string.Empty;

        [Required]
        public DateTime DatePrescribed { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsExpired { get; set; }
    }
}
