using AutoMapper;
using VehicleMaintenance.DTOs.GeneralExpense;
using VehicleMaintenance.DTOs.UserDrivingProfile;
using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using RecurrenceInterval = VehicleMaintenance.Models.Enums.RecurrenceInterval;

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
            CreateMap<VehicleComponent, VehicleComponentDto>()
                .ForMember(dest => dest.VehicleComponentId, opt => opt.MapFrom(src => src.VehicleComponentId))
                .ForMember(dest => dest.VehicleCurrentMileage,
                    opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Mileage : 0));

            CreateMap<CreateFuelEntryDto, FuelEntry>();
            CreateMap<FuelEntry, FuelEntryDto>();

            CreateMap<CreateVehicleDto, Vehicle>()
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => Enum.Parse<VehicleType>(src.VehicleType, true)));
            // ...repeat for TransmissionType, EngineType, FuelType
            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.VehicleType,       opt => opt.MapFrom(src => src.VehicleType.ToString()))
                .ForMember(dest => dest.TransmissionType,  opt => opt.MapFrom(src => src.TransmissionType.ToString()))
                .ForMember(dest => dest.EngineType,        opt => opt.MapFrom(src => src.EngineType.ToString()))
                .ForMember(dest => dest.FuelType,          opt => opt.MapFrom(src => src.FuelType.ToString()));

            CreateMap<CreateMaintenanceRecordDto, MaintenanceRecord>();
            CreateMap<MaintenanceRecord, MaintenanceRecordDto>()
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src =>
                    Enum.IsDefined(typeof(ServiceType), src.ServiceType)
                        ? src.ServiceType.ToString()
                        : ServiceType.Other.ToString()));

            CreateMap<CreateMaintenanceRecordComponentDto, MaintenanceRecordComponent>();
            CreateMap<MaintenanceRecordComponent, MaintenanceRecordComponentDto>()
                .ForMember(dest => dest.VehicleComponentName,
                    opt => opt.MapFrom(src => src.Component != null ? src.Component.VehicleComponentName : null))
                .ForMember(dest => dest.ComponentType,
                    opt => opt.MapFrom(src => src.Component != null ? src.Component.ComponentType.ToString() : null));

            CreateMap<Prediction, PredictionDto>()
                .ForMember(dest => dest.ComponentName,
                    opt => opt.MapFrom(src => src.VehicleComponent != null
                        ? src.VehicleComponent.VehicleComponentName
                        : null))
                .ForMember(dest => dest.ComponentType,
                    opt => opt.MapFrom(src => src.VehicleComponent != null
                        ? src.VehicleComponent.ComponentType.ToString()
                        : null))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateUserDrivingProfileDto, UserDrivingProfile>()
                .ForMember(dest => dest.PrimaryUsage,
                    opt => opt.MapFrom(src => Enum.Parse<PrimaryUsage>(src.PrimaryUsage, true)))
                .ForMember(dest => dest.DrivingStyle,
                    opt => opt.MapFrom(src => Enum.Parse<DrivingStyle>(src.DrivingStyle, true)))
                .ForMember(dest => dest.UsagePattern,
                    opt => opt.MapFrom(src => Enum.Parse<UsagePattern>(src.UsagePattern, true)))
                .ForMember(dest => dest.ClimateZone,
                    opt => opt.MapFrom(src => Enum.Parse<ClimateZone>(src.ClimateZone, true)))
                .ForMember(dest => dest.ParkingType,
                    opt => opt.MapFrom(src => Enum.Parse<ParkingType>(src.ParkingType, true)));

            CreateMap<CreateGeneralExpenseDto, GeneralExpense>()
                .ForMember(dest => dest.ExpenseCategory,
                    opt => opt.MapFrom(src => Enum.Parse<ExpenseCategory>(src.ExpenseCategory, true)))
                .ForMember(dest => dest.RecurrenceInterval,
                    opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.RecurrenceInterval)
                        ? (RecurrenceInterval?)null
                        : Enum.Parse<RecurrenceInterval>(src.RecurrenceInterval, true)));

            CreateMap<MaintenanceRecordComponent, ComponentHistoryDto>()
                .ForMember(dest => dest.ServiceDate, opt => opt.MapFrom(src => src.MaintenanceRecord.ServiceDate))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.MaintenanceRecord.ServiceName))
                .ForMember(dest => dest.ServiceType, opt => opt.MapFrom(src =>
                    Enum.IsDefined(typeof(ServiceType), src.MaintenanceRecord.ServiceType)
                        ? src.MaintenanceRecord.ServiceType.ToString()
                        : ServiceType.Other.ToString()))
                .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.MaintenanceRecord.Mileage))
                .ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.MaintenanceRecord.TechnicianName))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.MaintenanceRecord.Notes))
                .ForMember(dest => dest.ComponentChangeType, opt => opt.MapFrom(src => src.ComponentChangeType.ToString()))
                .ForMember(dest => dest.OldState, opt => opt.MapFrom(src => src.OldState.ToString()))
                .ForMember(dest => dest.NewState, opt => opt.MapFrom(src => src.NewState.ToString()));
        }
    }
}
