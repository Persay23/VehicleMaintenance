using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Vehicles;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class VehicleController(IVehicleService iVehicleService, UserManager<User> userManager) : ControllerBase
    {
        private readonly IVehicleService _iVehicleService = iVehicleService;
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet]
        public async Task<ActionResult<List<VehicleDto>>> GetVehicles()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var vehicles = await _iVehicleService.GetAllVehiclesAsync(userId);
            return Ok(vehicles);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(int id)
        {
            var vehicle = await _iVehicleService.GetVehicleByIdAsync(id);
            if (vehicle is null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpGet("{vehicleId}/summary/costs")]
        public async Task<IActionResult> GetCostSummary(int vehicleId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var summary = await _iVehicleService.GetCostSummaryAsync(vehicleId, from, to);
            if (summary is null) return NotFound();
            return Ok(summary);
        }

        [HttpGet("{vehicleId}/summary/timeline")]
        public async Task<IActionResult> GetTimeline(int vehicleId)
        {
            var timeline = await _iVehicleService.GetTimelineAsync(vehicleId);
            return Ok(timeline);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> CreateVehicle(CreateVehicleDto createVehicleDto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null) return Unauthorized();

            var createdVehicle = await _iVehicleService.CreateVehicleAsync(createVehicleDto, userId);
            return Ok(createdVehicle);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(int id, UpdateVehicleDto dto)
        {
            var updated = await _iVehicleService.UpdateVehicleByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var deleted = await _iVehicleService.DeleteVehicleByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
