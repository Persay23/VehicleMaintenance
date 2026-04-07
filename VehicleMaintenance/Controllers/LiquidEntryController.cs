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
        public async Task<ActionResult<List<LiquidEntryDto>>> GetLiquidEntriesByVehicleId(int vehicleId)
        {
            var liquidEntries = await _liquidEntryService.GetLiquidEntriesByVehicleIdAsync(vehicleId);
            return Ok(liquidEntries);
        }

        [HttpPost]
        public async Task<ActionResult<LiquidEntryDto>> CreateLiquidEntry(CreateLiquidEntryDto createLiquidEntryDto)
        {
            var createdLiquidEntry = await _liquidEntryService.CreateLiquidEntryAsync(createLiquidEntryDto);
            return Ok(createdLiquidEntry);
        }
    }
}
