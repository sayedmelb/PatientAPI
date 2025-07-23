using AutoMapper;
using Moq;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Services;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Repositories;

namespace PatientAPI.Tests.Application
{
    public class PrescriptionServiceTests
    {
        private readonly Mock<IPrescriptionRepository> _mockPrescriptionRepository;
        private readonly Mock<IPatientRepository> _mockPatientRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PrescriptionService _prescriptionService;

        public PrescriptionServiceTests()
        {
            _mockPrescriptionRepository = new Mock<IPrescriptionRepository>();
            _mockPatientRepository = new Mock<IPatientRepository>();
            _mockMapper = new Mock<IMapper>();
            _prescriptionService = new PrescriptionService(_mockPrescriptionRepository.Object, _mockPatientRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllPrescriptionsAsync_ShouldReturnSuccess_WhenPrescriptionsExist()
        {
            // Arrange
            var prescriptions = new List<Prescription>
            {
                new Prescription { Id = 1, DrugName = "Aspirin" },
                new Prescription { Id = 2, DrugName = "Ibuprofen" }
            };
            var prescriptionDtos = new List<PrescriptionDto>
            {
                new PrescriptionDto { Id = 1, DrugName = "Aspirin" },
                new PrescriptionDto { Id = 2, DrugName = "Ibuprofen" }
            };

            _mockPrescriptionRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(prescriptions);
            _mockMapper.Setup(m => m.Map<IEnumerable<PrescriptionDto>>(prescriptions)).Returns(prescriptionDtos);

            // Act
            var result = await _prescriptionService.GetAllPrescriptionsAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value?.Count());
        }

        [Fact]
        public async Task CreatePrescriptionAsync_ShouldReturnFailure_WhenPatientNotFound()
        {
            // Arrange
            var createPrescriptionDto = new CreatePrescriptionDto
            {
                PatientId = 999,
                DrugName = "Aspirin",
                Dosage = "100mg",
                DatePrescribed = DateTime.UtcNow
            };

            _mockPatientRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Patient with ID 999 not found", result.Error);
        }

        [Fact]
        public async Task CreatePrescriptionAsync_ShouldReturnSuccess_WhenValidData()
        {
            // Arrange
            var createPrescriptionDto = new CreatePrescriptionDto
            {
                PatientId = 1,
                DrugName = "Aspirin",
                Dosage = "100mg",
                DatePrescribed = DateTime.UtcNow
            };
            var patient = new Patient { Id = 1, FullName = "John Doe" };
            var prescription = new Prescription
            {
                Id = 1,
                PatientId = 1,
                DrugName = "Aspirin",
                Dosage = "100mg"
            };
            var prescriptionDto = new PrescriptionDto
            {
                Id = 1,
                PatientId = 1,
                DrugName = "Aspirin",
                Dosage = "100mg"
            };

            _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);
            _mockMapper.Setup(m => m.Map<Prescription>(createPrescriptionDto)).Returns(prescription);
            _mockPrescriptionRepository.Setup(r => r.GetNextIdAsync()).ReturnsAsync(1);
            _mockPrescriptionRepository.Setup(r => r.CreateAsync(It.IsAny<Prescription>())).ReturnsAsync(prescription);
            _mockMapper.Setup(m => m.Map<PrescriptionDto>(prescription)).Returns(prescriptionDto);

            // Act
            var result = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Aspirin", result.Value?.DrugName);
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldReturnFailure_WhenPrescriptionNotFound()
        {
            // Arrange
            var updatePrescriptionDto = new UpdatePrescriptionDto { PatientId = 1, DrugName = "Updated Drug" };
            _mockPrescriptionRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Prescription?)null);

            // Act
            var result = await _prescriptionService.UpdatePrescriptionAsync(1, updatePrescriptionDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Prescription with ID 1 not found", result.Error);
        }

        [Fact]
        public async Task SearchPrescriptionsByDrugNameAsync_ShouldReturnFailure_WhenDrugNameIsEmpty()
        {
            // Act
            var result = await _prescriptionService.SearchPrescriptionsByDrugNameAsync("");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Drug name cannot be empty", result.Error);
        }
    }
}
