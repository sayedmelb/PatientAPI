using PatientAPI.Domain.Entities;
using PatientAPI.Infrastructure.Persistence.Models;

namespace PatientAPI.Tests.Infrastructure.Persistence.Models
{
    public class PrescriptionModelTests
    {
        [Fact]
        public void FromEntity_ShouldMapAllProperties()
        {
            // Arrange
            var prescription = new Prescription
            {
                MongoId = "507f1f77bcf86cd799439011",
                Id = 1,
                PatientId = 1,
                DrugName = "Aspirin",
                Dosage = "100mg",
                DatePrescribed = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var prescriptionModel = PrescriptionModel.FromEntity(prescription);

            // Assert
            Assert.Equal(prescription.MongoId, prescriptionModel.MongoId);
            Assert.Equal(prescription.Id, prescriptionModel.Id);
            Assert.Equal(prescription.PatientId, prescriptionModel.PatientId);
            Assert.Equal(prescription.DrugName, prescriptionModel.DrugName);
            Assert.Equal(prescription.Dosage, prescriptionModel.Dosage);
            Assert.Equal(prescription.DatePrescribed, prescriptionModel.DatePrescribed);
            Assert.Equal(prescription.CreatedAt, prescriptionModel.CreatedAt);
            Assert.Equal(prescription.UpdatedAt, prescriptionModel.UpdatedAt);
        }

        [Fact]
        public void ToEntity_ShouldMapAllProperties()
        {
            // Arrange
            var prescriptionModel = new PrescriptionModel
            {
                MongoId = "507f1f77bcf86cd799439011",
                Id = 1,
                PatientId = 1,
                DrugName = "Aspirin",
                Dosage = "100mg",
                DatePrescribed = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            var prescription = prescriptionModel.ToEntity();

            // Assert
            Assert.Equal(prescriptionModel.MongoId, prescription.MongoId);
            Assert.Equal(prescriptionModel.Id, prescription.Id);
            Assert.Equal(prescriptionModel.PatientId, prescription.PatientId);
            Assert.Equal(prescriptionModel.DrugName, prescription.DrugName);
            Assert.Equal(prescriptionModel.Dosage, prescription.Dosage);
            Assert.Equal(prescriptionModel.DatePrescribed, prescription.DatePrescribed);
            Assert.Equal(prescriptionModel.CreatedAt, prescription.CreatedAt);
            Assert.Equal(prescriptionModel.UpdatedAt, prescription.UpdatedAt);
        }
    }
}
