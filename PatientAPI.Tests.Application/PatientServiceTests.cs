using AutoMapper;
using Moq;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Services;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Repositories;

namespace PatientAPI.Tests.Application
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockPatientRepository;
        private readonly Mock<IPrescriptionRepository> _mockPrescriptionRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _mockPatientRepository = new Mock<IPatientRepository>();
            _mockPrescriptionRepository = new Mock<IPrescriptionRepository>();
            _mockMapper = new Mock<IMapper>();
            _patientService = new PatientService(_mockPatientRepository.Object, _mockPrescriptionRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnSuccess_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FullName = "John Doe" },
                new Patient { Id = 2, FullName = "Jane Smith" }
            };
            var patientDtos = new List<PatientDto>
            {
                new PatientDto { Id = 1, FullName = "John Doe" },
                new PatientDto { Id = 2, FullName = "Jane Smith" }
            };

            _mockPatientRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(patients);
            _mockMapper.Setup(m => m.Map<IEnumerable<PatientDto>>(patients)).Returns(patientDtos);

            // Act
            var result = await _patientService.GetAllPatientsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value?.Count());
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnFailure_WhenExceptionThrown()
        {
            // Arrange
            _mockPatientRepository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _patientService.GetAllPatientsAsync();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Failed to retrieve patients", result.Error);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnSuccess_WhenPatientExists()
        {
            // Arrange
            var patient = new Patient { Id = 1, FullName = "John Doe" };
            var patientDto = new PatientDto { Id = 1, FullName = "John Doe" };

            _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _patientService.GetPatientByIdAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("John Doe", result.Value?.FullName);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnFailure_WhenPatientNotFound()
        {
            // Arrange
            _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _patientService.GetPatientByIdAsync(1);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Patient with ID 1 not found", result.Error);
        }

        [Fact]
        public async Task CreatePatientAsync_ShouldReturnSuccess_WhenValidData()
        {
            // Arrange
            var createPatientDto = new CreatePatientDto
            {
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            var patient = new Patient
            {
                Id = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            var patientDto = new PatientDto
            {
                Id = 1,
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            _mockMapper.Setup(m => m.Map<Patient>(createPatientDto)).Returns(patient);
            _mockPatientRepository.Setup(r => r.GetNextIdAsync()).ReturnsAsync(1);
            _mockPatientRepository.Setup(r => r.CreateAsync(It.IsAny<Patient>())).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<PatientDto>(patient)).Returns(patientDto);

            // Act
            var result = await _patientService.CreatePatientAsync(createPatientDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("John Doe", result.Value?.FullName);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldReturnSuccess_WhenPatientExists()
        {
            // Arrange
            var updatePatientDto = new UpdatePatientDto
            {
                FullName = "Updated Name",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            var existingPatient = new Patient { Id = 1, FullName = "John Doe" };

            _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingPatient);
            _mockMapper.Setup(m => m.Map(updatePatientDto, existingPatient));
            _mockPatientRepository.Setup(r => r.UpdateAsync(It.IsAny<Patient>())).ReturnsAsync(true);

            // Act
            var result = await _patientService.UpdatePatientAsync(1, updatePatientDto);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldReturnFailure_WhenPatientNotFound()
        {
            // Arrange
            var updatePatientDto = new UpdatePatientDto { FullName = "Updated Name" };
            _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _patientService.UpdatePatientAsync(1, updatePatientDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Patient with ID 1 not found", result.Error);
        }

        [Fact]
        public async Task DeletePatientAsync_ShouldReturnSuccess_WhenPatientDeleted()
        {
            // Arrange
            _mockPatientRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _patientService.DeletePatientAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SearchPatientsByNameAsync_ShouldReturnFailure_WhenNameIsEmpty()
        {
            // Act
            var result = await _patientService.SearchPatientsByNameAsync("");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Search name cannot be empty", result.Error);
        }

        [Fact]
        public async Task GetPatientWithPrescriptionsAsync_ShouldReturnSuccess_WhenPatientExists()
        {
            // Arrange
            var patient = new Patient { Id = 1, FullName = "John Doe" };
            var prescriptions = new List<Prescription>
            {
                new Prescription { Id = 1, PatientId = 1, DrugName = "Aspirin" }
            };
            var dto = new PatientWithPrescriptionsDto();

            _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);
            _mockPrescriptionRepository.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(prescriptions);
            _mockMapper.Setup(m => m.Map<PatientWithPrescriptionsDto>(It.IsAny<object>())).Returns(dto);

            // Act
            var result = await _patientService.GetPatientWithPrescriptionsAsync(1);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
