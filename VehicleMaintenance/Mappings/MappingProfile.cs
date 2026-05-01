using AutoMapper;
using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleDto>();

            CreateMap<CreateVehicleComponentDto, VehicleComponent>();
            CreateMap<VehicleComponent, VehicleComponentDto>();

            CreateMap<VehicleComponent, VehicleComponentDto>()
                .ForMember(dest => dest.VehicleComponentId, opt => opt.MapFrom(src => src.VehicleComponentId));

            CreateMap<CreateFuelEntryDto, FuelEntry>();
            CreateMap<FuelEntry, FuelEntryDto>();

            CreateMap<CreateVehicleDto, Vehicle>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => Enum.Parse<VehicleType>(src.VehicleType, true)));
            // ...repeat for TransmissionType, EngineType, FuelType
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.VehicleType.ToString()));
                  // ...repeat

            CreateMap<CreateMaintenanceRecordDto, MaintenanceRecord>();
            CreateMap<MaintenanceRecord, MaintenanceRecordDto>();

            CreateMap<CreateMaintenanceRecordComponentDto, MaintenanceRecordComponent>();
            CreateMap<MaintenanceRecordComponent, MaintenanceRecordComponentDto>()
                .ForMember(dest => dest.VehicleComponentName,
                    opt => opt.MapFrom(src => src.Component != null ? src.Component.VehicleComponentName : null))
                .ForMember(dest => dest.ComponentType,
                    opt => opt.MapFrom(src => src.Component != null ? src.Component.ComponentType.ToString() : null));

            CreateMap<CreatePredictionDto, Prediction>()
                .ForMember(dest => dest.ComponentType,
                    opt => opt.MapFrom(src => Enum.Parse<ComponentType>(src.ComponentType, true)))
                // ConfidenceScore division is handled manually in the service after mapping
                // because mapper runs before we know to divide — keep it simple
                .ForMember(dest => dest.ConfidenceScore, opt => opt.Ignore());

            CreateMap<Prediction, PredictionDto>()
                .ForMember(dest => dest.ComponentType,
                    opt => opt.MapFrom(src => src.ComponentType.ToString()))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ConfidenceScore,
                    opt => opt.MapFrom(src => src.ConfidenceScore * 100));
        }
    }
}
