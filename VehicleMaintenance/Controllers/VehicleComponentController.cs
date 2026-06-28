using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Security;
using VehicleMaintenance.Services.GenralModelService.Interfaces;

namespace VehicleMaintenance.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class VehicleComponentController(
        IVehicleComponentService iVehicleComponentService,
        IVehicleOwnershipService ownership) : ControllerBase
    {
        private readonly IVehicleComponentService _iVehicleComponentService = iVehicleComponentService;
        private readonly IVehicleOwnershipService _ownership = ownership;

        [HttpGet("vehicle/{vehicleId}/health")]
        public async Task<IActionResult> GetComponentHealth(int vehicleId)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, vehicleId)) return NotFound();

            var health = await _iVehicleComponentService.GetComponentHealthAsync(vehicleId);
            return Ok(health);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<ActionResult<VehicleComponentDto[]>> GetByVehicle(int vehicleId)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, vehicleId)) return NotFound();

            var components = await _iVehicleComponentService.GetVehicleComponentByVehicleAsync(vehicleId);
            return Ok(components);
        }

        [HttpGet("{id:int}/history")]
        public async Task<ActionResult<ComponentHistoryDto[]>> GetComponentHistory(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsComponentAsync(userId, id)) return NotFound();

            var history = await _iVehicleComponentService.GetComponentHistoryAsync(id);
            return Ok(history);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleComponentDto>> GetVehicleComponentById(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsComponentAsync(userId, id)) return NotFound();

            var component = await _iVehicleComponentService.GetVehicleComponentByIdAsync(id);
            if (component is null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicleComponent(CreateVehicleComponentDto createVehicleComponentDto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, createVehicleComponentDto.VehicleId)) return Forbid();

            var createdVehicleComponent = await _iVehicleComponentService.CreateVehicleComponentAsync(createVehicleComponentDto);
            return Ok(createdVehicleComponent);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<VehicleComponentDto>> UpdateVehicleComponent(int id, UpdateVehicleComponentDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsComponentAsync(userId, id)) return NotFound();

            var updated = await _iVehicleComponentService.UpdateVehicleComponentByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicleComponent(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsComponentAsync(userId, id)) return NotFound();

            var deleted = await _iVehicleComponentService.DeleteVehicleComponentByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
