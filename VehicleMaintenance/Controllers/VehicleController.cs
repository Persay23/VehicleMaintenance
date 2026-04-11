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

        [HttpGet]
        public async Task<ActionResult<List<VehicleDto>>> GetVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(int id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpGet("{vehicleId}/summary/costs")]
        public async Task<IActionResult> GetCostSummary(int vehicleId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var summary = await _vehicleService.GetCostSummaryAsync(vehicleId, from, to);
            if (summary is null) return NotFound();
            return Ok(summary);
        }

        [HttpGet("{vehicleId}/summary/timeline")]
        public async Task<IActionResult> GetTimeline(int vehicleId)
        {
            var timeline = await _vehicleService.GetTimelineAsync(vehicleId);
            return Ok(timeline);
        }

        [HttpPost]
        public async Task<ActionResult<CreateVehicleDto>> CreateVehicle(CreateVehicleDto createVehicleDto)
        {
            var createdVehicle = await _vehicleService.CreateVehicleAsync(createVehicleDto);
            return Ok(createdVehicle);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(int id, UpdateVehicleDto dto)
        {
            var updated = await _vehicleService.UpdateVehicleByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var deleted = await _vehicleService.DeleteVehicleByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
