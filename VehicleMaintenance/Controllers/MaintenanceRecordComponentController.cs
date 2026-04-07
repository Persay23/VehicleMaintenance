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
        public async Task<ActionResult<List<MaintenanceRecordComponentDto>>> GetMaintenanceRecordComponentsByMaintenanceRecordId(int maintenanceRecordId)
        {
            var maintenanceRecordComponents = await _maintenanceRecordComponentService.GetMaintenanceRecordComponentsByMaintenanceRecordIdAsync(maintenanceRecordId);
            return Ok(maintenanceRecordComponents);
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordComponentDto>> CreateMaintenanceRecordComponent(CreateMaintenanceRecordComponentDto createMaintenanceRecordComponentDto)
        {
            var createdMaintenanceRecordComponent = await _maintenanceRecordComponentService.CreateMaintenanceRecordComponentAsync(createMaintenanceRecordComponentDto);
            return Ok(createdMaintenanceRecordComponent);
        }
    }
}
