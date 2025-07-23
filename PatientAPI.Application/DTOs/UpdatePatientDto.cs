using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Application.DTOs
{
    public class UpdatePatientDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
