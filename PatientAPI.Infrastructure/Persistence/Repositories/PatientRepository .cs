using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Repositories;
using PatientAPI.Infrastructure.Configuration;
using PatientAPI.Infrastructure.Persistence.Models;


namespace PatientAPI.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IMongoCollection<PatientModel> _patientsCollection;

        public PatientRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _patientsCollection = mongoDatabase.GetCollection<PatientModel>(databaseSettings.Value.PatientsCollectionName);
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            var patientModels = await _patientsCollection.Find(_ => true).ToListAsync();
            return patientModels.Select(p => p.ToEntity());
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            var patientModel = await _patientsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return patientModel?.ToEntity();
        }

        public async Task<Patient?> GetByMongoIdAsync(string mongoId)
        {
            var patientModel = await _patientsCollection.Find(p => p.MongoId == mongoId).FirstOrDefaultAsync();
            return patientModel?.ToEntity();
        }

        public async Task<Patient> CreateAsync(Patient patient)
        {
            var patientModel = PatientModel.FromEntity(patient);
            await _patientsCollection.InsertOneAsync(patientModel);
            return patientModel.ToEntity();
        }

        public async Task<bool> UpdateAsync(Patient patient)
        {
            var patientModel = PatientModel.FromEntity(patient);
            var result = await _patientsCollection.ReplaceOneAsync(p => p.Id == patient.Id, patientModel);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _patientsCollection.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Patient>> SearchByNameAsync(string name)
        {
            var filter = Builders<PatientModel>.Filter.Regex(p => p.FullName, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            var patientModels = await _patientsCollection.Find(filter).ToListAsync();

            return patientModels.Select(p => p.ToEntity());
        }

        public async Task<int> GetNextIdAsync()
        {
            var lastPatient = await _patientsCollection
                .Find(_ => true)
                .SortByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            return lastPatient?.Id + 1 ?? 1;
        }
    }
}
