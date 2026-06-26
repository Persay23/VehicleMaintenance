using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.Security;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FuelEntryController(
        IFuelEntryService iFuelEntryService,
        IVehicleOwnershipService ownership) : ControllerBase
    {
        private readonly IFuelEntryService _iFuelEntryService = iFuelEntryService;
        private readonly IVehicleOwnershipService _ownership = ownership;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FuelEntryDto>> GetFuelEntryById(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsFuelEntryAsync(userId, id)) return NotFound();

            var fuelEntry = await _iFuelEntryService.GetFuelEntryByIdAsync(id);
            if (fuelEntry is null)
            {
                return NotFound();
            }

            return Ok(fuelEntry);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(int vehicleId, [FromQuery] string? fuelType, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, vehicleId)) return NotFound();

            var entries = await _iFuelEntryService.GetFuelEntryByVehicleAsync(
                vehicleId, fuelType, fromDate, toDate);
            return Ok(entries);
        }

        [HttpPost]
        public async Task<ActionResult<FuelEntryDto>> CreateFuelEntry(CreateFuelEntryDto createFuelEntryDto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, createFuelEntryDto.VehicleId)) return Forbid();

            var createdFuelEntry = await _iFuelEntryService.CreateFuelEntryAsync(createFuelEntryDto);
            return Ok(createdFuelEntry);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<FuelEntryDto>> UpdateFuelEntry(int id, UpdateFuelEntryDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsFuelEntryAsync(userId, id)) return NotFound();

            var updated = await _iFuelEntryService.UpdateFuelEntryByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFuelEntry(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsFuelEntryAsync(userId, id)) return NotFound();

            var deleted = await _iFuelEntryService.DeleteFuelEntryByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
