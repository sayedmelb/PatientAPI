using PatientAPI.Application.DTOs;
using PatientAPI.Domain.Common;

namespace PatientAPI.Application.Interfaces
{
    public interface IPatientService
    {
        Task<Result<IEnumerable<PatientDto>>> GetAllPatientsAsync();
        Task<Result<PatientDto>> GetPatientByIdAsync(int id);
        Task<Result<PatientDto>> CreatePatientAsync(CreatePatientDto createPatientDto);
        Task<Result> UpdatePatientAsync(int id, UpdatePatientDto updatePatientDto);
        Task<Result> DeletePatientAsync(int id);
        Task<Result<IEnumerable<PatientDto>>> SearchPatientsByNameAsync(string name);
        Task<Result<PatientWithPrescriptionsDto>> GetPatientWithPrescriptionsAsync(int patientId);
    }
}
