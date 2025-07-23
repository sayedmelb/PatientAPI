using AutoMapper;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Interfaces;
using PatientAPI.Domain.Common;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Models;
using PatientAPI.Domain.Repositories;

namespace PatientAPI.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository patientRepository,
            IPrescriptionRepository prescriptionRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _prescriptionRepository = prescriptionRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PatientDto>>> GetAllPatientsAsync()
        {
            try
            {
                var patients = await _patientRepository.GetAllAsync();
                var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
                return Result<IEnumerable<PatientDto>>.Success(patientDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PatientDto>>.Failure($"Failed to retrieve patients: {ex.Message}");
            }
        }

        public async Task<Result<PatientDto>> GetPatientByIdAsync(int id)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(id);
                if (patient == null)
                {
                    return Result<PatientDto>.Failure($"Patient with ID {id} not found");
                }

                var patientDto = _mapper.Map<PatientDto>(patient);
                return Result<PatientDto>.Success(patientDto);
            }
            catch (Exception ex)
            {
                return Result<PatientDto>.Failure($"Failed to retrieve patient: {ex.Message}");
            }
        }

        public async Task<Result<PatientDto>> CreatePatientAsync(CreatePatientDto createPatientDto)
        {
            try
            {
                var patient = _mapper.Map<Patient>(createPatientDto);
                patient.Id = await _patientRepository.GetNextIdAsync();

                var createdPatient = await _patientRepository.CreateAsync(patient);
                var patientDto = _mapper.Map<PatientDto>(createdPatient);

                return Result<PatientDto>.Success(patientDto);
            }
            catch (Exception ex)
            {
                return Result<PatientDto>.Failure($"Failed to create patient: {ex.Message}");
            }
        }

        public async Task<Result> UpdatePatientAsync(int id, UpdatePatientDto updatePatientDto)
        {
            try
            {
                var existingPatient = await _patientRepository.GetByIdAsync(id);
                if (existingPatient == null)
                {
                    return Result.Failure($"Patient with ID {id} not found");
                }

                _mapper.Map(updatePatientDto, existingPatient);
                existingPatient.Id = id;
                existingPatient.UpdateTimestamp();

                var success = await _patientRepository.UpdateAsync(existingPatient);

                return success ? Result.Success() : Result.Failure("Failed to update patient");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update patient: {ex.Message}");
            }
        }

        public async Task<Result> DeletePatientAsync(int id)
        {
            try
            {
                var success = await _patientRepository.DeleteAsync(id);
                return success ? Result.Success() : Result.Failure($"Patient with ID {id} not found");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete patient: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PatientDto>>> SearchPatientsByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<IEnumerable<PatientDto>>.Failure("Search name cannot be empty");
                }

                var patients = await _patientRepository.SearchByNameAsync(name);
                var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);

                return Result<IEnumerable<PatientDto>>.Success(patientDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PatientDto>>.Failure($"Failed to search patients: {ex.Message}");
            }
        }

        public async Task<Result<PatientWithPrescriptionsDto>> GetPatientWithPrescriptionsAsync(int patientId)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return Result<PatientWithPrescriptionsDto>.Failure($"Patient with ID {patientId} not found");
                }

                var prescriptions = await _prescriptionRepository.GetByPatientIdAsync(patientId);

                var patientWithPrescriptions = new PatientWithPrescriptions
                {
                    Patient = patient,
                    Prescriptions = prescriptions.ToList()
                };

                var dto = _mapper.Map<PatientWithPrescriptionsDto>(patientWithPrescriptions);
                return Result<PatientWithPrescriptionsDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<PatientWithPrescriptionsDto>.Failure($"Failed to retrieve patient with prescriptions: {ex.Message}");
            }
        }
    }
}
