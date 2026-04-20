using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.FuelEntry;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FuelEntryController(IFuelEntryService iFuelEntryService) : ControllerBase
    {
        private readonly IFuelEntryService _iFuelEntryService = iFuelEntryService;

        [HttpGet]
        public async Task<ActionResult<List<FuelEntryDto>>> GetFuelEntries()
        {
            var FuelEntries = await _iFuelEntryService.GetAllFuelEntriesAsync();
            return Ok(FuelEntries);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FuelEntryDto>> GetFuelEntryById(int id)
        {
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
            var entries = await _iFuelEntryService.GetByVehicleAsync(
                vehicleId, fuelType, fromDate, toDate);
            return Ok(entries);
        }

        [HttpPost]
        public async Task<ActionResult<FuelEntryDto>> CreateFuelEntry(CreateFuelEntryDto createFuelEntryDto)
        {
            var createdFuelEntry = await _iFuelEntryService.CreateFuelEntryAsync(createFuelEntryDto);
            return Ok(createdFuelEntry);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<FuelEntryDto>> UpdateFuelEntry(int id, UpdateFuelEntryDto dto)
        {
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
            var deleted = await _iFuelEntryService.DeleteFuelEntryByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
