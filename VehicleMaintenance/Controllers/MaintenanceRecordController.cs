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
        public async Task<ActionResult<List<MaintenanceRecordDto>>> GetMaintenanceRecords()
        {
            var maintenanceRecords = await _maintenanceRecordService.GetAllMaintenanceRecordsAsync();
            return Ok(maintenanceRecords);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordDto>> GetMaintenanceRecordById(int id)
        {
            var record = await _maintenanceRecordService.GetMaintenanceRecordByIdAsync(id);
            if (record is null)
            {
                return NotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceRecordDto>> CreateMaintenanceRecord(CreateMaintenanceRecordDto createMaintenanceRecordDto)
        {
            var createdMaintenanceRecord = await _maintenanceRecordService.CreateMaintenanceRecordAsync(createMaintenanceRecordDto);
            return Ok(createdMaintenanceRecord);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<MaintenanceRecordDto>> UpdateMaintenanceRecord(int id, UpdateMaintenanceRecordDto dto)
        {
            var updated = await _maintenanceRecordService.UpdateMaintenanceRecordByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMaintenanceRecord(int id)
        {
            var deleted = await _maintenanceRecordService.DeleteMaintenanceRecordByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}