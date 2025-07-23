using PatientAPI.Domain.Entities;

namespace PatientAPI.Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> GetByMongoIdAsync(string mongoId);
        Task<Patient> CreateAsync(Patient patient);
        Task<bool> UpdateAsync(Patient patient);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Patient>> SearchByNameAsync(string name);
        Task<int> GetNextIdAsync();
    }
}
