using AutoMapper;
using VehicleMaintenance.Data;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleMaintenance.Services
{
    public class VehicleService(AppDbContext context, IMapper mapper)
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto dto) // interfaces
        {
            var vehicle = _mapper.Map<Vehicle>(dto);

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return _mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<List<VehicleDto>> GetVehiclesByUserIdAsync(int userId)
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<VehicleDto>>(vehicles); // repositories!!
        }
    }
}