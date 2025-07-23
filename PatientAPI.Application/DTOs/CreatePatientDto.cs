
using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Application.DTOs
{
    public class CreatePatientDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
