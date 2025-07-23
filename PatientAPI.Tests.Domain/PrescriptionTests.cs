using PatientAPI.Domain.Entities;

namespace PatientAPI.Tests.Domain
{
    public class PrescriptionTests
    {
        [Fact]
        public void UpdateTimestamp_ShouldUpdateUpdatedAtProperty()
        {
            // Arrange
            var prescription = new Prescription
            {
                Id = 1,
                PatientId = 1,
                DrugName = "Aspirin",
                Dosage = "100mg",
                DatePrescribed = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };
            var originalUpdatedAt = prescription.UpdatedAt;

            // Act
            prescription.UpdateTimestamp();

            // Assert
            Assert.True(prescription.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void IsExpired_WithDefaultValidityDays_ShouldReturnTrue_WhenOlderThan30Days()
        {
            // Arrange
            var prescription = new Prescription
            {
                DatePrescribed = DateTime.UtcNow.AddDays(-31)
            };

            // Act
            var isExpired = prescription.IsExpired();

            // Assert
            Assert.True(isExpired);
        }

        [Fact]
        public void IsExpired_WithDefaultValidityDays_ShouldReturnFalse_WhenWithin30Days()
        {
            // Arrange
            var prescription = new Prescription
            {
                DatePrescribed = DateTime.UtcNow.AddDays(-29)
            };

            // Act
            var isExpired = prescription.IsExpired();

            // Assert
            Assert.False(isExpired);
        }

        [Theory]
        [InlineData(10, -11, true)]
        [InlineData(10, -9, false)]
        [InlineData(60, -61, true)]
        [InlineData(60, -59, false)]
        public void IsExpired_WithCustomValidityDays_ShouldReturnCorrectResult(int validityDays, int daysAgo, bool expectedExpired)
        {
            // Arrange
            var prescription = new Prescription
            {
                DatePrescribed = DateTime.UtcNow.AddDays(daysAgo)
            };

            // Act
            var isExpired = prescription.IsExpired(validityDays);

            // Assert
            Assert.Equal(expectedExpired, isExpired);
        }
    }
}
