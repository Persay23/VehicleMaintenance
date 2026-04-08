using AutoMapper;
using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using VehicleMaintenance.DTOs.Users;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleDto>();

            CreateMap<CreateVehicleComponentDto, VehicleComponent>();
            CreateMap<VehicleComponent, VehicleComponentDto>();

            CreateMap<CreateLiquidEntryDto, LiquidEntry>();
            CreateMap<LiquidEntry, LiquidEntryDto>();

            CreateMap<CreateMaintenanceRecordDto, MaintenanceRecord>();
            CreateMap<MaintenanceRecord, MaintenanceRecordDto>()
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src =>
                    src.MaintenanceRecordComponents
                        .Select(mrc => (int?)mrc.ComponentId)
                        .FirstOrDefault()));

            CreateMap<CreateMaintenanceRecordComponentDto, MaintenanceRecordComponent>();
            CreateMap<MaintenanceRecordComponent, MaintenanceRecordComponentDto>();
        }
    }
}
