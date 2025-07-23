using Microsoft.Extensions.Logging;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Interfaces;
using PatientAPI.Domain.Common;


namespace PatientAPI.Infrastructure.Services
{
    public class DataSeedingService : IDataSeedingService
    {
        private readonly IPatientService _patientService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly ILogger<DataSeedingService> _logger;

        // Sample data for seeding
        private readonly List<string> _sampleNames = new()
        {
            "John Smith", "Sarah Johnson", "Michael Brown", "Emily Davis",
            "David Wilson", "Jessica Miller", "Christopher Moore", "Ashley Taylor",
            "Matthew Anderson", "Amanda Thomas", "Daniel Jackson", "Jennifer White",
            "James Harris", "Lisa Martin", "Robert Thompson", "Michelle Garcia",
            "William Martinez", "Karen Rodriguez", "Charles Lewis", "Nancy Lee",
            "Joseph Walker", "Betty Hall", "Thomas Allen", "Helen Young",
            "Richard King", "Sandra Wright", "Mark Lopez", "Donna Hill",
            "Donald Scott", "Carol Green", "Steven Adams", "Sharon Baker",
            "Paul Gonzalez", "Kimberly Nelson", "Andrew Carter", "Angela Mitchell",
            "Kenneth Perez", "Brenda Roberts", "Edward Turner", "Emma Phillips"
        };

        private readonly List<string> _sampleDrugs = new()
        {
            "Amoxicillin", "Ibuprofen", "Acetaminophen", "Prednisone",
            "Azithromycin", "Omeprazole", "Metformin", "Lisinopril",
            "Simvastatin", "Levothyroxine", "Warfarin", "Furosemide",
            "Hydrochlorothiazide", "Atenolol", "Amlodipine", "Losartan",
            "Gabapentin", "Tramadol", "Sertraline", "Citalopram",
            "Fluoxetine", "Alprazolam", "Lorazepam", "Clonazepam",
            "Insulin", "Metoprolol", "Carvedilol", "Digoxin",
            "Albuterol", "Montelukast", "Fluticasone", "Cetirizine"
        };

        private readonly List<string> _sampleDosages = new()
        {
            "250mg", "500mg", "1g", "10mg", "20mg", "25mg", "50mg", "100mg",
            "5mg twice daily", "10mg once daily", "25mg three times daily",
            "500mg every 8 hours", "1 tablet daily", "2 tablets twice daily",
            "0.5mg as needed", "1mg at bedtime", "2.5mg morning and evening",
            "5ml every 6 hours", "1 puff twice daily", "2 puffs as needed",
            "Apply topically twice daily", "1 drop in each eye daily"
        };

        public DataSeedingService(
            IPatientService patientService,
            IPrescriptionService prescriptionService,
            ILogger<DataSeedingService> logger)
        {
            _patientService = patientService;
            _prescriptionService = prescriptionService;
            _logger = logger;
        }

        public async Task<Result> SeedDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Starting database seeding process...");

                if (await IsDatabaseSeededAsync())
                {
                    _logger.LogInformation("Database is already seeded. Skipping seeding process.");
                    return Result.Success();
                }

                // Seed patients first
                var patientResult = await SeedPatientsAsync(20);
                if (!patientResult.IsSuccess)
                {
                    _logger.LogError("Failed to seed patients: {Error}", patientResult.Error);
                    return Result.Failure($"Failed to seed patients: {patientResult.Error}");
                }

                // Then seed prescriptions
                var prescriptionResult = await SeedPrescriptionsAsync(50);
                if (!prescriptionResult.IsSuccess)
                {
                    _logger.LogError("Failed to seed prescriptions: {Error}", prescriptionResult.Error);
                    return Result.Failure($"Failed to seed prescriptions: {prescriptionResult.Error}");
                }

                _logger.LogInformation("Database seeding completed successfully");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database");
                return Result.Failure($"Database seeding failed: {ex.Message}");
            }
        }

        public async Task<Result> ClearDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Starting database clearing process...");

                // Get all patients and prescriptions
                var patientsResult = await _patientService.GetAllPatientsAsync();
                if (!patientsResult.IsSuccess)
                {
                    return Result.Failure($"Failed to get patients for clearing: {patientsResult.Error}");
                }

                var prescriptionsResult = await _prescriptionService.GetAllPrescriptionsAsync();
                if (!prescriptionsResult.IsSuccess)
                {
                    return Result.Failure($"Failed to get prescriptions for clearing: {prescriptionsResult.Error}");
                }

                // Delete all prescriptions first (due to foreign key constraints)
                foreach (var prescription in prescriptionsResult.Value!)
                {
                    var deleteResult = await _prescriptionService.DeletePrescriptionAsync(prescription.Id);
                    if (!deleteResult.IsSuccess)
                    {
                        _logger.LogWarning("Failed to delete prescription {Id}: {Error}",
                            prescription.Id, deleteResult.Error);
                    }
                }

                // Then delete all patients
                foreach (var patient in patientsResult.Value!)
                {
                    var deleteResult = await _patientService.DeletePatientAsync(patient.Id);
                    if (!deleteResult.IsSuccess)
                    {
                        _logger.LogWarning("Failed to delete patient {Id}: {Error}",
                            patient.Id, deleteResult.Error);
                    }
                }

                _logger.LogInformation("Database clearing completed successfully");
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while clearing the database");
                return Result.Failure($"Database clearing failed: {ex.Message}");
            }
        }

        public async Task<Result> SeedPatientsAsync(int count = 10)
        {
            try
            {
                _logger.LogInformation("Seeding {Count} patients...", count);

                var random = new Random();
                var createdCount = 0;

                for (int i = 0; i < count && i < _sampleNames.Count; i++)
                {
                    var createPatientDto = new CreatePatientDto
                    {
                        FullName = _sampleNames[i],
                        DateOfBirth = GenerateRandomDateOfBirth(random)
                    };

                    var result = await _patientService.CreatePatientAsync(createPatientDto);
                    if (result.IsSuccess)
                    {
                        createdCount++;
                        _logger.LogDebug("Created patient: {Name} (ID: {Id})",
                            createPatientDto.FullName, result.Value!.Id);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create patient {Name}: {Error}",
                            createPatientDto.FullName, result.Error);
                    }
                }

                _logger.LogInformation("Successfully seeded {CreatedCount}/{RequestedCount} patients",
                    createdCount, count);

                return createdCount > 0
                    ? Result.Success()
                    : Result.Failure("No patients were created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding patients");
                return Result.Failure($"Patient seeding failed: {ex.Message}");
            }
        }

        public async Task<Result> SeedPrescriptionsAsync(int count = 20)
        {
            try
            {
                _logger.LogInformation("Seeding {Count} prescriptions...", count);

                // First get all existing patients
                var patientsResult = await _patientService.GetAllPatientsAsync();
                if (!patientsResult.IsSuccess || !patientsResult.Value!.Any())
                {
                    return Result.Failure("No patients found. Please seed patients first.");
                }

                var patients = patientsResult.Value!.ToList();
                var random = new Random();
                var createdCount = 0;

                for (int i = 0; i < count; i++)
                {
                    var randomPatient = patients[random.Next(patients.Count)];
                    var randomDrug = _sampleDrugs[random.Next(_sampleDrugs.Count)];
                    var randomDosage = _sampleDosages[random.Next(_sampleDosages.Count)];

                    var createPrescriptionDto = new CreatePrescriptionDto
                    {
                        PatientId = randomPatient.Id,
                        DrugName = randomDrug,
                        Dosage = randomDosage,
                        DatePrescribed = GenerateRandomPrescriptionDate(random)
                    };

                    var result = await _prescriptionService.CreatePrescriptionAsync(createPrescriptionDto);
                    if (result.IsSuccess)
                    {
                        createdCount++;
                        _logger.LogDebug("Created prescription: {Drug} for patient {PatientId} (ID: {Id})",
                            randomDrug, randomPatient.Id, result.Value!.Id);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create prescription for patient {PatientId}: {Error}",
                            randomPatient.Id, result.Error);
                    }
                }

                _logger.LogInformation("Successfully seeded {CreatedCount}/{RequestedCount} prescriptions",
                    createdCount, count);

                return createdCount > 0
                    ? Result.Success()
                    : Result.Failure("No prescriptions were created");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding prescriptions");
                return Result.Failure($"Prescription seeding failed: {ex.Message}");
            }
        }

        public async Task<bool> IsDatabaseSeededAsync()
        {
            try
            {
                var patientsResult = await _patientService.GetAllPatientsAsync();
                if (!patientsResult.IsSuccess)
                {
                    _logger.LogWarning("Could not check if database is seeded: {Error}", patientsResult.Error);
                    return false;
                }

                var hasPatients = patientsResult.Value!.Any();
                _logger.LogDebug("Database seeded check: {HasPatients}", hasPatients);

                return hasPatients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if database is seeded");
                return false;
            }
        }

        private static DateTime GenerateRandomDateOfBirth(Random random)
        {
            // Generate birth dates between 18 and 90 years ago
            var minAge = 18;
            var maxAge = 90;

            var today = DateTime.Today;
            var minDate = today.AddYears(-maxAge);
            var maxDate = today.AddYears(-minAge);

            var range = (maxDate - minDate).Days;
            var randomDays = random.Next(range);

            return minDate.AddDays(randomDays);
        }

        private static DateTime GenerateRandomPrescriptionDate(Random random)
        {
            // Generate prescription dates within the last 2 years
            var today = DateTime.UtcNow;
            var minDate = today.AddYears(-2);

            var range = (today - minDate).Days;
            var randomDays = random.Next(range);

            return minDate.AddDays(randomDays);
        }
    }
}
