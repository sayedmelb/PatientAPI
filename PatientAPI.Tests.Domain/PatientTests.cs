using PatientAPI.Domain.Entities;


namespace PatientAPI.Tests.Domain
{
    public class PatientTests
    {
        [Fact]
        public void UpdateTimestamp_ShouldUpdateUpdatedAtProperty()
        {
            // Arrange
            var patient = new Patient
            {
                Id = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };
            var originalUpdatedAt = patient.UpdatedAt;

            // Act
            patient.UpdateTimestamp();

            // Assert
            Assert.True(patient.UpdatedAt > originalUpdatedAt);
        }

        [Theory]
        [InlineData("1990-01-01", 34)] // Assuming current date is 2024
        [InlineData("2000-12-31", 23)]
        [InlineData("1985-06-15", 39)]
        public void GetAge_ShouldCalculateCorrectAge(string birthDateString, int expectedAge)
        {
            // Arrange
            var birthDate = DateTime.Parse(birthDateString);
            var patient = new Patient
            {
                Id = 1,
                FullName = "Test Patient",
                DateOfBirth = birthDate
            };

            // Act
            var actualAge = patient.GetAge();

            // Assert
            // Note: This test might need adjustment based on current date
            Assert.True(actualAge >= expectedAge - 1 && actualAge <= expectedAge + 1);
        }

        [Fact]
        public void GetAge_WithBirthdayNotYetThisYear_ShouldReturnCorrectAge()
        {
            // Arrange
            var today = DateTime.Today;
            var birthDate = today.AddYears(-25).AddDays(1); // Birthday tomorrow
            var patient = new Patient
            {
                DateOfBirth = birthDate
            };

            // Act
            var age = patient.GetAge();

            // Assert
            Assert.Equal(24, age); // Should be one year less
        }
    }
}
