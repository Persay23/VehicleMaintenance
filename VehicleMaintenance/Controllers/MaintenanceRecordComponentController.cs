using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.MaintenanceRecordComponents;
using VehicleMaintenance.Services;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRecordComponentController(MaintenanceRecordComponentService maintenanceRecordComponentService) : ControllerBase
    {
        private readonly MaintenanceRecordComponentService _maintenanceRecordComponentService = maintenanceRecordComponentService;

        [HttpGet]
        public async Task<ActionResult<List<MaintenanceRecordComponentDto>>> GetMaintenanceRecordComponents()
        {
            var maintenanceRecordComponents = await _maintenanceRecordComponentService.GetAllMaintenanceRecordComponentsAsync();
            return Ok(maintenanceRecordComponents);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> GetMaintenanceRecordComponentById(int id)
        {
            var item = await _maintenanceRecordComponentService.GetMaintenanceRecordComponentByIdAsync(id);
            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> CreateMaintenanceRecordComponent(CreateMaintenanceRecordComponentDto createMaintenanceRecordComponentDto)
        {
            var createdMaintenanceRecordComponent = await _maintenanceRecordComponentService.CreateMaintenanceRecordComponentAsync(createMaintenanceRecordComponentDto);
            return Ok(createdMaintenanceRecordComponent);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> UpdateMaintenanceRecordComponent(int id, UpdateMaintenanceRecordComponentDto dto)
        {
            var updated = await _maintenanceRecordComponentService.UpdateMaintenanceRecordComponentByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMaintenanceRecordComponent(int id)
        {
            var deleted = await _maintenanceRecordComponentService.DeleteMaintenanceRecordComponentByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
