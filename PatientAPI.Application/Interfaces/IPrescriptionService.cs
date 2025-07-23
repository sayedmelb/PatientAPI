using PatientAPI.Application.DTOs;
using PatientAPI.Domain.Common;

namespace PatientAPI.Application.Interfaces
{
    public interface IPrescriptionService
    {
        Task<Result<IEnumerable<PrescriptionDto>>> GetAllPrescriptionsAsync();
        Task<Result<PrescriptionDto>> GetPrescriptionByIdAsync(int id);
        Task<Result<IEnumerable<PrescriptionDto>>> GetPrescriptionsByPatientIdAsync(int patientId);
        Task<Result<PrescriptionDto>> CreatePrescriptionAsync(CreatePrescriptionDto createPrescriptionDto);
        Task<Result> UpdatePrescriptionAsync(int id, UpdatePrescriptionDto updatePrescriptionDto);
        Task<Result> DeletePrescriptionAsync(int id);
        Task<Result<IEnumerable<PrescriptionDto>>> SearchPrescriptionsByDrugNameAsync(string drugName);
    }
}
