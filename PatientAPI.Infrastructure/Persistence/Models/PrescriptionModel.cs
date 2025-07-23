
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PatientAPI.Domain.Entities;

namespace PatientAPI.Infrastructure.Persistence.Models
{
    public class PrescriptionModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MongoId { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("patientId")]
        public int PatientId { get; set; }

        [BsonElement("drugName")]
        public string DrugName { get; set; } = string.Empty;

        [BsonElement("dosage")]
        public string Dosage { get; set; } = string.Empty;

        [BsonElement("datePrescribed")]
        public DateTime DatePrescribed { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        public static PrescriptionModel FromEntity(Prescription prescription)
        {
            return new PrescriptionModel
            {
                MongoId = prescription.MongoId,
                Id = prescription.Id,
                PatientId = prescription.PatientId,
                DrugName = prescription.DrugName,
                Dosage = prescription.Dosage,
                DatePrescribed = prescription.DatePrescribed,
                CreatedAt = prescription.CreatedAt,
                UpdatedAt = prescription.UpdatedAt
            };
        }

        public Prescription ToEntity()
        {
            return new Prescription
            {
                MongoId = MongoId,
                Id = Id,
                PatientId = PatientId,
                DrugName = DrugName,
                Dosage = Dosage,
                DatePrescribed = DatePrescribed,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }
    }
}
