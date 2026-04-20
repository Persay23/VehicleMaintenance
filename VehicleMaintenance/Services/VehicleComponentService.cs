using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Services
{
    public class VehicleComponentService(AppDbContext context, IMapper mapper) : IVehicleComponentService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto)
        {
            var vehilcecomponent = _mapper.Map<VehicleComponent>(dto);

            _context.VehicleComponents.Add(vehilcecomponent);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleComponentDto>(vehilcecomponent);
        }

        public async Task<List<VehicleComponentDto>> GetAllVehicleComponentsAsync()
        {
            var vehicleComponents = await _context.VehicleComponents.ToListAsync();
            return _mapper.Map<List<VehicleComponentDto>>(vehicleComponents);
        }

        public async Task<VehicleComponentDto?> GetVehicleComponentByIdAsync(int id)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            return vehicleComponent is null ? null : _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }

        public async Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            if (vehicleComponent is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.ComponentType)) vehicleComponent.ComponentType = Enum.Parse<ComponentType>(dto.ComponentType, true);
            if (dto.InstallationDate.HasValue) vehicleComponent.InstallationDate = dto.InstallationDate.Value;
            if (dto.LastServiceDate.HasValue) vehicleComponent.LastServiceDate = dto.LastServiceDate.Value;
            if (!string.IsNullOrWhiteSpace(dto.State)) vehicleComponent.State = Enum.Parse<State>(dto.State, true);
            if (dto.Notes is not null) vehicleComponent.Notes = dto.Notes;
            if (dto.CurrentMileage.HasValue) vehicleComponent.CurrentMileage = dto.CurrentMileage.Value;
            if (dto.ExpectedLifetimeKm.HasValue) vehicleComponent.ExpectedLifetimeKm = dto.ExpectedLifetimeKm.Value;
            if (dto.ExpectedLifetimeYears.HasValue) vehicleComponent.ExpectedLifetimeYears = dto.ExpectedLifetimeYears.Value;

            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }

        public async Task<bool> DeleteVehicleComponentByIdAsync(int id)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.VehicleComponentId == id);
            if (vehicleComponent is null)
            {
                return false;
            }

            _context.VehicleComponents.Remove(vehicleComponent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ComponentHealthDto>> GetComponentHealthAsync(int vehicleId) // dive into this
        {
            var components = await _context.VehicleComponents
                .Where(c => c.VehicleId == vehicleId)
                .ToListAsync();

            var now = DateTime.UtcNow;

            return [.. components.Select(c =>
            {
                // KM-based health
                var remainingKm = c.ExpectedLifetimeKm - c.CurrentMileage;
                var kmPercent = c.ExpectedLifetimeKm > 0
                    ? Math.Max(0, (double)remainingKm / c.ExpectedLifetimeKm * 100)
                    : 100;

                // Year-based health — uses InstallationDate from your entity
                var yearsUsed = (now - c.InstallationDate).TotalDays / 365.25;
                var yearsPercent = c.ExpectedLifetimeYears > 0
                    ? Math.Max(0, (1 - yearsUsed / c.ExpectedLifetimeYears) * 100)
                    : 100;

                // Worst of the two determines overall status
                var worstPercent = Math.Min(kmPercent, yearsPercent);

                var status = worstPercent switch
                {
                    <= 15 => "Critical",
                    <= 30 => "Warning",
                    <= 50 => "Monitor",
                    <= 75 => "Good",
                    _ => "Excelent"
                };

                return new ComponentHealthDto
                {
                    ComponentId = c.VehicleComponentId,
                    ComponentType = c.ComponentType.ToString(),
                    CurrentState = c.State.ToString(), // State enum from your entity
                    RemainingKm = Math.Max(0, remainingKm),
                    KmLifetimePercent = Math.Round(kmPercent, 1),
                    YearsLifetimePercent = Math.Round(yearsPercent, 1),
                    Status = status
                };
            })];
        }
    }
}
