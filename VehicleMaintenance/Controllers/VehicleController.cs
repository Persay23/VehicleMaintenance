using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Services;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class VehicleController(VehicleService vehicleService) : ControllerBase
    {
        private readonly VehicleService _vehicleService = vehicleService;

        [HttpPost]
        public async Task<ActionResult<CreateVehicleDto>> CreateVehicle(CreateVehicleDto createVehicleDto)
        {
            var createdVehicle = await _vehicleService.CreateVehicleAsync(createVehicleDto);
            return Ok(createdVehicle);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<VehicleDto>>> GetVehiclesByUserId(int userId)
        {
            var vehicles = await _vehicleService.GetVehiclesByUserIdAsync(userId);
            return Ok(vehicles);
        }
    }
}
