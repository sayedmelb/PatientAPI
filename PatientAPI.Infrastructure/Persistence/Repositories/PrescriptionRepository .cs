using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Repositories;
using PatientAPI.Infrastructure.Configuration;
using PatientAPI.Infrastructure.Persistence.Models;

namespace PatientAPI.Infrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly IMongoCollection<PrescriptionModel> _prescriptionsCollection;

        public PrescriptionRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _prescriptionsCollection = mongoDatabase.GetCollection<PrescriptionModel>(databaseSettings.Value.PrescriptionsCollectionName);
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            var prescriptionModels = await _prescriptionsCollection.Find(_ => true).ToListAsync();
            return prescriptionModels.Select(p => p.ToEntity());
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            var prescriptionModel = await _prescriptionsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            return prescriptionModel?.ToEntity();
        }

        public async Task<IEnumerable<Prescription>> GetByPatientIdAsync(int patientId)
        {
            var prescriptionModels = await _prescriptionsCollection.Find(p => p.PatientId == patientId).ToListAsync();
            return prescriptionModels.Select(p => p.ToEntity());
        }

        public async Task<Prescription> CreateAsync(Prescription prescription)
        {
            var prescriptionModel = PrescriptionModel.FromEntity(prescription);
            await _prescriptionsCollection.InsertOneAsync(prescriptionModel);
            return prescriptionModel.ToEntity();
        }

        public async Task<bool> UpdateAsync(Prescription prescription)
        {
            var prescriptionModel = PrescriptionModel.FromEntity(prescription);
            var result = await _prescriptionsCollection.ReplaceOneAsync(p => p.Id == prescription.Id, prescriptionModel);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _prescriptionsCollection.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Prescription>> SearchByDrugNameAsync(string drugName)
        {
            var filter = Builders<PrescriptionModel>.Filter.Regex(p => p.DrugName, new MongoDB.Bson.BsonRegularExpression(drugName, "i"));
            var prescriptionModels = await _prescriptionsCollection.Find(filter).ToListAsync();
            return prescriptionModels.Select(p => p.ToEntity());
        }

        public async Task<int> GetNextIdAsync()
        {
            var lastPrescription = await _prescriptionsCollection
                .Find(_ => true)
                .SortByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            return lastPrescription?.Id + 1 ?? 1;
        }
    }
}
