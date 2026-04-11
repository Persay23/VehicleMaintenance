using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.LiquidEntry;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LiquidEntryController(LiquidEntryService liquidEntryService) : ControllerBase
    {
        private readonly LiquidEntryService _liquidEntryService = liquidEntryService;

        [HttpGet]
        public async Task<ActionResult<List<LiquidEntryDto>>> GetLiquidEntries()
        {
            var liquidEntries = await _liquidEntryService.GetAllLiquidEntriesAsync();
            return Ok(liquidEntries);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LiquidEntryDto>> GetLiquidEntryById(int id)
        {
            var liquidEntry = await _liquidEntryService.GetLiquidEntryByIdAsync(id);
            if (liquidEntry is null)
            {
                return NotFound();
            }

            return Ok(liquidEntry);
        }

        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(int vehicleId, [FromQuery] LiquidType? liquidType, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var entries = await _liquidEntryService.GetByVehicleAsync(
                vehicleId, liquidType, fromDate, toDate);
            return Ok(entries);
        }

        [HttpPost]
        public async Task<ActionResult<LiquidEntryDto>> CreateLiquidEntry(CreateLiquidEntryDto createLiquidEntryDto)
        {
            var createdLiquidEntry = await _liquidEntryService.CreateLiquidEntryAsync(createLiquidEntryDto);
            return Ok(createdLiquidEntry);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<LiquidEntryDto>> UpdateLiquidEntry(int id, UpdateLiquidEntryDto dto)
        {
            var updated = await _liquidEntryService.UpdateLiquidEntryByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLiquidEntry(int id)
        {
            var deleted = await _liquidEntryService.DeleteLiquidEntryByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
