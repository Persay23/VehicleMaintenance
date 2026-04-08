using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Services
{
    public class VehicleComponentService(AppDbContext context, IMapper mapper) : IVehicleComponentService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        // TODO: Add static general methods for creating, and retrieving users

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
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.ComponentId == id);
            return vehicleComponent is null ? null : _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }

        public async Task<VehicleComponentDto?> UpdateVehicleComponentByIdAsync(int id, UpdateVehicleComponentDto dto)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.ComponentId == id);
            if (vehicleComponent is null)
            {
                return null;
            }

            if (dto.ComponentType.HasValue) vehicleComponent.ComponentType = dto.ComponentType.Value;
            if (dto.InstallationDate.HasValue) vehicleComponent.InstallationDate = dto.InstallationDate.Value;
            if (dto.LastServiceDate.HasValue) vehicleComponent.LastServiceDate = dto.LastServiceDate.Value;
            if (dto.State.HasValue) vehicleComponent.State = dto.State.Value;
            if (dto.Notes is not null) vehicleComponent.Notes = dto.Notes;
            if (dto.CurrentMileage.HasValue) vehicleComponent.CurrentMileage = dto.CurrentMileage.Value;
            if (dto.ExpectedLifetimeKm.HasValue) vehicleComponent.ExpectedLifetimeKm = dto.ExpectedLifetimeKm.Value;
            if (dto.ExpectedLifetimeYears.HasValue) vehicleComponent.ExpectedLifetimeYears = dto.ExpectedLifetimeYears.Value;

            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleComponentDto>(vehicleComponent);
        }

        public async Task<bool> DeleteVehicleComponentByIdAsync(int id)
        {
            var vehicleComponent = await _context.VehicleComponents.FirstOrDefaultAsync(vc => vc.ComponentId == id);
            if (vehicleComponent is null)
            {
                return false;
            }

            _context.VehicleComponents.Remove(vehicleComponent);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
