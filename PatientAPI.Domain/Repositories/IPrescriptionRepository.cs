

using PatientAPI.Domain.Entities;

namespace PatientAPI.Domain.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription?> GetByIdAsync(int id);
        Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId);
        Task<Prescription> CreateAsync(Prescription prescription);
        Task<bool> UpdateAsync(Prescription prescription);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Prescription>> SearchByDrugNameAsync(string drugName);
        Task<int> GetNextIdAsync();
    }
}
