using AutoMapper;
using PatientAPI.Application.DTOs;
using PatientAPI.Application.Interfaces;
using PatientAPI.Domain.Common;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Repositories;

namespace PatientAPI.Application.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PrescriptionService(
            IPrescriptionRepository prescriptionRepository,
            IPatientRepository patientRepository,
            IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PrescriptionDto>>> GetAllPrescriptionsAsync()
        {
            try
            {
                var prescriptions = await _prescriptionRepository.GetAllAsync();
                var prescriptionDtos = _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
                return Result<IEnumerable<PrescriptionDto>>.Success(prescriptionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PrescriptionDto>>.Failure($"Failed to retrieve prescriptions: {ex.Message}");
            }
        }

        public async Task<Result<PrescriptionDto>> GetPrescriptionByIdAsync(int id)
        {
            try
            {
                var prescription = await _prescriptionRepository.GetByIdAsync(id);
                if (prescription == null)
                {
                    return Result<PrescriptionDto>.Failure($"Prescription with ID {id} not found");
                }

                var prescriptionDto = _mapper.Map<PrescriptionDto>(prescription);
                return Result<PrescriptionDto>.Success(prescriptionDto);
            }
            catch (Exception ex)
            {
                return Result<PrescriptionDto>.Failure($"Failed to retrieve prescription: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PrescriptionDto>>> GetPrescriptionsByPatientIdAsync(int patientId)
        {
            try
            {
                var prescriptions = await _prescriptionRepository.GetByPatientIdAsync(patientId);
                var prescriptionDtos = _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
                return Result<IEnumerable<PrescriptionDto>>.Success(prescriptionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PrescriptionDto>>.Failure($"Failed to retrieve prescriptions for patient: {ex.Message}");
            }
        }

        public async Task<Result<PrescriptionDto>> CreatePrescriptionAsync(CreatePrescriptionDto createPrescriptionDto)
        {
            try
            {
                // Validate patient exists
                var patient = await _patientRepository.GetByIdAsync(createPrescriptionDto.PatientId);
                if (patient == null)
                {
                    return Result<PrescriptionDto>.Failure($"Patient with ID {createPrescriptionDto.PatientId} not found");
                }

                var prescription = _mapper.Map<Prescription>(createPrescriptionDto);
                prescription.Id = await _prescriptionRepository.GetNextIdAsync();

                var createdPrescription = await _prescriptionRepository.CreateAsync(prescription);
                var prescriptionDto = _mapper.Map<PrescriptionDto>(createdPrescription);

                return Result<PrescriptionDto>.Success(prescriptionDto);
            }
            catch (Exception ex)
            {
                return Result<PrescriptionDto>.Failure($"Failed to create prescription: {ex.Message}");
            }
        }

        public async Task<Result> UpdatePrescriptionAsync(int id, UpdatePrescriptionDto updatePrescriptionDto)
        {
            try
            {
                var existingPrescription = await _prescriptionRepository.GetByIdAsync(id);
                if (existingPrescription == null)
                {
                    return Result.Failure($"Prescription with ID {id} not found");
                }

                // Validate patient exists
                var patient = await _patientRepository.GetByIdAsync(updatePrescriptionDto.PatientId);
                if (patient == null)
                {
                    return Result.Failure($"Patient with ID {updatePrescriptionDto.PatientId} not found");
                }

                _mapper.Map(updatePrescriptionDto, existingPrescription);
                existingPrescription.Id = id;
                existingPrescription.UpdateTimestamp();

                var success = await _prescriptionRepository.UpdateAsync(existingPrescription);

                return success ? Result.Success() : Result.Failure("Failed to update prescription");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update prescription: {ex.Message}");
            }
        }

        public async Task<Result> DeletePrescriptionAsync(int id)
        {
            try
            {
                var success = await _prescriptionRepository.DeleteAsync(id);
                return success ? Result.Success() : Result.Failure($"Prescription with ID {id} not found");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete prescription: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<PrescriptionDto>>> SearchPrescriptionsByDrugNameAsync(string drugName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(drugName))
                {
                    return Result<IEnumerable<PrescriptionDto>>.Failure("Drug name cannot be empty");
                }

                var prescriptions = await _prescriptionRepository.SearchByDrugNameAsync(drugName);
                var prescriptionDtos = _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);

                return Result<IEnumerable<PrescriptionDto>>.Success(prescriptionDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PrescriptionDto>>.Failure($"Failed to search prescriptions: {ex.Message}");
            }
        }
    }
}
