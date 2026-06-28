using AutoMapper;
using VehicleMaintenance.DTOs.UserDrivingProfile;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Repositories.Interfaces;
using VehicleMaintenance.Services.GenralModelService.Interfaces;

namespace VehicleMaintenance.Services.GenralModelService
{
    public class UserDrivingProfileService(IUserDrivingProfileRepository repository, IMapper mapper) : IUserDrivingProfileService
    {
        private readonly IUserDrivingProfileRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<UserDrivingProfileDto?> GetUserDrivingProfileByUserIdAsync(string userId)
        {
            var profile = await _repository.GetUserDrivingProfileByUserIdAsync(userId);
            return profile is null ? null : MapToDto(profile);
        }

        public async Task<UserDrivingProfileDto> CreateUserDrivingProfileAsync(CreateUserDrivingProfileDto dto)
        {
            var profile = _mapper.Map<UserDrivingProfile>(dto);
            await _repository.AddAsync(profile);
            await _repository.SaveAsync();
            return MapToDto(profile);
        }

        public async Task<UserDrivingProfileDto?> UpdateUserDrivingProfileAsync(string userId, UpdateUserDrivingProfileDto dto)
        {
            var profile = await _repository.GetUserDrivingProfileByUserIdAsync(userId);
            if (profile is null)
                return null;

            if (dto.AnnualKm.HasValue) profile.AnnualKm = dto.AnnualKm.Value;
            if (!string.IsNullOrWhiteSpace(dto.PrimaryUsage))
                profile.PrimaryUsage = Enum.Parse<PrimaryUsage>(dto.PrimaryUsage, true);
            if (!string.IsNullOrWhiteSpace(dto.DrivingStyle))
                profile.DrivingStyle = Enum.Parse<DrivingStyle>(dto.DrivingStyle, true);
            if (!string.IsNullOrWhiteSpace(dto.UsagePattern))
                profile.UsagePattern = Enum.Parse<UsagePattern>(dto.UsagePattern, true);
            if (!string.IsNullOrWhiteSpace(dto.ClimateZone))
                profile.ClimateZone = Enum.Parse<ClimateZone>(dto.ClimateZone, true);
            if (!string.IsNullOrWhiteSpace(dto.ParkingType))
                profile.ParkingType = Enum.Parse<ParkingType>(dto.ParkingType, true);

            profile.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveAsync();
            return MapToDto(profile);
        }

        public async Task<bool> DeleteUserDrivingProfileAsync(string userId)
        {
            return await _repository.DeleteUserDrivingProfileAsync(userId);
        }

        private static UserDrivingProfileDto MapToDto(UserDrivingProfile profile)
        {
            return new UserDrivingProfileDto
            {
                UserDrivingProfileId = profile.UserDrivingProfileId,
                UserId               = profile.UserId,
                AnnualKm             = profile.AnnualKm,
                PrimaryUsage         = profile.PrimaryUsage.ToString(),
                DrivingStyle         = profile.DrivingStyle.ToString(),
                UsagePattern         = profile.UsagePattern.ToString(),
                ClimateZone          = profile.ClimateZone.ToString(),
                ParkingType          = profile.ParkingType.ToString(),
                CreatedAt            = profile.CreatedAt,
                UpdatedAt            = profile.UpdatedAt,
            };
        }
    }
}
