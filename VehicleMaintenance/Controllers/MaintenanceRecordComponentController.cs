using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRecordComponentController(IMaintenanceRecordComponentService iMaintenanceRecordComponentService) : ControllerBase
    {
        private readonly IMaintenanceRecordComponentService _iMaintenanceRecordComponentService = iMaintenanceRecordComponentService;

        [HttpGet]
        public async Task<ActionResult<List<MaintenanceRecordComponentDto>>> GetMaintenanceRecordComponents()
        {
            var maintenanceRecordComponents = await _iMaintenanceRecordComponentService.GetAllMaintenanceRecordComponentsAsync();
            return Ok(maintenanceRecordComponents);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> GetMaintenanceRecordComponentById(int id)
        {
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
            var createdMaintenanceRecordComponent = await _iMaintenanceRecordComponentService.CreateMaintenanceRecordComponentAsync(createMaintenanceRecordComponentDto);
            return Ok(createdMaintenanceRecordComponent);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> UpdateMaintenanceRecordComponent(int id, UpdateMaintenanceRecordComponentDto dto)
        {
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
            var deleted = await _iMaintenanceRecordComponentService.DeleteMaintenanceRecordComponentByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
