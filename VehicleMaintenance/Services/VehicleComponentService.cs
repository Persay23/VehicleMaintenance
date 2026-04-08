using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class VehicleComponentService(AppDbContext context, IMapper mapper)
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

        public async Task<List<VehicleComponentDto>> GetVehicleComponentsByVehicleIdAsync(int vehicleId)
        {
            var vehicleComponents = await _context.VehicleComponents
                .Where(vc => vc.VehicleId == vehicleId)
                .ToListAsync();

            return _mapper.Map<List<VehicleComponentDto>>(vehicleComponents);
        }
    }
}
