
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PatientAPI.Domain.Entities;

namespace PatientAPI.Infrastructure.Persistence.Models
{
    public class PatientModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? MongoId { get; set; }

        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        public static PatientModel FromEntity(Patient patient)
        {
            return new PatientModel
            {
                MongoId = patient.MongoId,
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                CreatedAt = patient.CreatedAt,
                UpdatedAt = patient.UpdatedAt
            };
        }

        public Patient ToEntity()
        {
            return new Patient
            {
                MongoId = MongoId,
                Id = Id,
                FullName = FullName,
                DateOfBirth = DateOfBirth,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
        }
    }
}
