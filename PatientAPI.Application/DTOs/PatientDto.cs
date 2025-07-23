using System.ComponentModel.DataAnnotations;

namespace PatientAPI.Application.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Age { get; set; }
    }
}
