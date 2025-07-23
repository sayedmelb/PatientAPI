using PatientAPI.Domain.Entities;
using PatientAPI.Infrastructure.Persistence.Models;

namespace PatientAPI.Tests.Infrastructure.Persistence.Models
{
    public class PatientModelTests
    {
        [Fact]
        public void FromEntity_ShouldMapAllProperties()
        {
            // Arrange
            var patient = new Patient
            {
                MongoId = "507f1f77bcf86cd799439011",
                Id = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var patientModel = PatientModel.FromEntity(patient);

            // Assert
            Assert.Equal(patient.MongoId, patientModel.MongoId);
            Assert.Equal(patient.Id, patientModel.Id);
            Assert.Equal(patient.FullName, patientModel.FullName);
            Assert.Equal(patient.DateOfBirth, patientModel.DateOfBirth);
            Assert.Equal(patient.CreatedAt, patientModel.CreatedAt);
            Assert.Equal(patient.UpdatedAt, patientModel.UpdatedAt);
        }

        [Fact]
        public void ToEntity_ShouldMapAllProperties()
        {
            // Arrange
            var patientModel = new PatientModel
            {
                MongoId = "507f1f77bcf86cd799439011",
                Id = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var patient = patientModel.ToEntity();

            // Assert
            Assert.Equal(patientModel.MongoId, patient.MongoId);
            Assert.Equal(patientModel.Id, patient.Id);
            Assert.Equal(patientModel.FullName, patient.FullName);
            Assert.Equal(patientModel.DateOfBirth, patient.DateOfBirth);
            Assert.Equal(patientModel.CreatedAt, patient.CreatedAt);
            Assert.Equal(patientModel.UpdatedAt, patient.UpdatedAt);
        }
    }
}
