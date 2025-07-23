using PatientAPI.Application.DTOs;
using PatientAPI.Domain.Entities;
using PatientAPI.Domain.Models;
using AutoMapper;


namespace PatientAPI.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Patient mappings
            CreateMap<Patient, PatientDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.GetAge()));

            CreateMap<CreatePatientDto, Patient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MongoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MongoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Prescription mappings
            CreateMap<Prescription, PrescriptionDto>()
               .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired(30)));

            CreateMap<CreatePrescriptionDto, Prescription>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MongoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdatePrescriptionDto, Prescription>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MongoId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // PatientWithPrescriptions mapping
            CreateMap<PatientWithPrescriptions, PatientWithPrescriptionsDto>();
        }
    }
}

