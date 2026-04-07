using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class VehicleComponentService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;
        // TODO: Add static general methods for creating, and retrieving users

        public async Task<VehicleComponentDto> CreateVehicleComponentAsync(CreateVehicleComponentDto dto)
        {
            var vehilcecomponent = new VehicleComponent
            {
                VehicleId = dto.VehicleId,
                ComponentType = dto.ComponentType,
                InstallationDate = dto.InstallationDate,
                LastServiceDate = dto.LastServiceDate,
                State = dto.State,
                Notes = dto.Notes,
                CurrentMileage = dto.CurrentMileage,
                ExpectedLifetimeKm = dto.ExpectedLifetimeKm,
                ExpectedLifetimeYears = dto.ExpectedLifetimeYears
            };

            _context.VehicleComponents.Add(vehilcecomponent);
            await _context.SaveChangesAsync();

            return new VehicleComponentDto
            {
                ComponentId = vehilcecomponent.ComponentId,
                ComponentType = vehilcecomponent.ComponentType, // dto.ComponentType or vehilcecomponent.ComponentType?
                InstallationDate = dto.InstallationDate,
                LastServiceDate = dto.LastServiceDate,
                State = dto.State,
                Notes = dto.Notes,
                CurrentMileage = dto.CurrentMileage,
                ExpectedLifetimeKm = dto.ExpectedLifetimeKm,
                ExpectedLifetimeYears = dto.ExpectedLifetimeYears
            };
        }

        public async Task<List<VehicleComponentDto>> GetVehicleComponentsByVehicleIdAsync(int vehicleId)
        {
            return await _context.VehicleComponents
                .Where(vc => vc.VehicleId == vehicleId)
                .Select(vc => new VehicleComponentDto
                {
                    ComponentId = vc.ComponentId,
                    VehicleId = vc.VehicleId,
                    ComponentType = vc.ComponentType,
                    InstallationDate = vc.InstallationDate,
                    LastServiceDate = vc.LastServiceDate,
                    State = vc.State,
                    Notes = vc.Notes,
                    CurrentMileage = vc.CurrentMileage,
                    ExpectedLifetimeKm = vc.ExpectedLifetimeKm,
                    ExpectedLifetimeYears = vc.ExpectedLifetimeYears
                })
                .ToListAsync();
        }
    }
}
