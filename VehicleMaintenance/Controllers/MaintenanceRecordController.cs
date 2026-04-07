using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Services;
using VehicleMaintenance.DTOs.MaintenanceRecords;


namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceRecordController(MaintenanceRecordService maintenanceRecordService) : ControllerBase
    {
        private readonly MaintenanceRecordService _maintenanceRecordService = maintenanceRecordService;

        [HttpGet]
        public async Task<ActionResult<List<MaintenanceRecordDto>>> GetMaintenanceRecordsByVehicleId(int vehicleId)
        {
            var maintenanceRecords = await _maintenanceRecordService.GetMaintenanceRecordsByVehicleIdAsync(vehicleId);
            return Ok(maintenanceRecords);
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordDto>> CreateMaintenanceRecord(CreateMaintenanceRecordDto createMaintenanceRecordDto)
        {
            var createdMaintenanceRecord = await _maintenanceRecordService.CreateMaintenanceRecordAsync(createMaintenanceRecordDto);
            return Ok(createdMaintenanceRecord);
        }
    }
}