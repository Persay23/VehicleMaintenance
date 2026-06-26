using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.Security;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRecordComponentController(
        IMaintenanceRecordComponentService iMaintenanceRecordComponentService,
        IVehicleOwnershipService ownership) : ControllerBase
    {
        private readonly IMaintenanceRecordComponentService _iMaintenanceRecordComponentService = iMaintenanceRecordComponentService;
        private readonly IVehicleOwnershipService _ownership = ownership;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> GetMaintenanceRecordComponentById(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsRecordComponentAsync(userId, id)) return NotFound();

            var item = await _iMaintenanceRecordComponentService.GetMaintenanceRecordComponentByIdAsync(id);
            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> CreateMaintenanceRecordComponent(CreateMaintenanceRecordComponentDto createMaintenanceRecordComponentDto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsMaintenanceRecordAsync(userId, createMaintenanceRecordComponentDto.MaintenanceRecordId)) return Forbid();

            var createdMaintenanceRecordComponent = await _iMaintenanceRecordComponentService.CreateMaintenanceRecordComponentAsync(createMaintenanceRecordComponentDto);
            return Ok(createdMaintenanceRecordComponent);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> UpdateMaintenanceRecordComponent(int id, UpdateMaintenanceRecordComponentDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsRecordComponentAsync(userId, id)) return NotFound();

            var updated = await _iMaintenanceRecordComponentService.UpdateMaintenanceRecordComponentByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMaintenanceRecordComponent(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsRecordComponentAsync(userId, id)) return NotFound();

            var deleted = await _iMaintenanceRecordComponentService.DeleteMaintenanceRecordComponentByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
