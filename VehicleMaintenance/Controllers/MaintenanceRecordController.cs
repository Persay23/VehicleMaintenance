using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.MaintenanceRecords;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.Security;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRecordController(
        IMaintenanceRecordService iMaintenanceRecordService,
        IVehicleOwnershipService ownership) : ControllerBase
    {
        private readonly IMaintenanceRecordService _iMaintenanceRecordService = iMaintenanceRecordService;
        private readonly IVehicleOwnershipService _ownership = ownership;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordDto>> GetMaintenanceRecordById(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsMaintenanceRecordAsync(userId, id)) return NotFound();

            var record = await _iMaintenanceRecordService.GetMaintenanceRecordByIdAsync(id);
            if (record is null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(int vehicleId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] string? serviceType)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, vehicleId)) return NotFound();

            var records = await _iMaintenanceRecordService.GetMaintenanceRecordByVehicleAsync(
                vehicleId, fromDate, toDate, serviceType);
            return Ok(records);
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordDto>> CreateMaintenanceRecord(CreateMaintenanceRecordDto createMaintenanceRecordDto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, createMaintenanceRecordDto.VehicleId)) return Forbid();

            var createdMaintenanceRecord = await _iMaintenanceRecordService.CreateMaintenanceRecordAsync(createMaintenanceRecordDto);
            return CreatedAtAction(nameof(GetMaintenanceRecordById),
                new { id = createdMaintenanceRecord.MaintenanceRecordId },
                createdMaintenanceRecord);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordDto>> UpdateMaintenanceRecord(int id, UpdateMaintenanceRecordDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsMaintenanceRecordAsync(userId, id)) return NotFound();

            var updated = await _iMaintenanceRecordService.UpdateMaintenanceRecordByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMaintenanceRecord(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsMaintenanceRecordAsync(userId, id)) return NotFound();

            var deleted = await _iMaintenanceRecordService.DeleteMaintenanceRecordByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
