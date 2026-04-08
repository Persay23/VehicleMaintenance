using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Services;
using VehicleMaintenance.DTOs.LiquidEntry;

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
