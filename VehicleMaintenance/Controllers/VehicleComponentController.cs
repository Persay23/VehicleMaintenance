using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Services;
using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class VehicleComponentController(VehicleComponentService vehicleComponentService) : ControllerBase
    {
        private readonly VehicleComponentService _vehicleComponentService = vehicleComponentService;

        /// <summary>
        /// Get all vehicles
        /// </summary>
        /// <param name="vehicleId">hhhhh</param>
        /// <returns>hhh</returns>
        [HttpGet]
        public async Task<ActionResult<List<VehicleComponentDto>>> GetVehicleComponents()
        {
            var vehicleComponents = await _vehicleComponentService.GetAllVehicleComponentsAsync();
            return Ok(vehicleComponents);
        }

        [HttpGet("vehicle/{vehicleId}/health")]
        public async Task<IActionResult> GetComponentHealth(int vehicleId)
        {
            var health = await _vehicleComponentService.GetComponentHealthAsync(vehicleId);
            return Ok(health);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createVehicleComponentDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateVehicleComponent(CreateVehicleComponentDto createVehicleComponentDto)
        {
            var createdVehicleComponent = await _vehicleComponentService.CreateVehicleComponentAsync(createVehicleComponentDto);
            return Ok(createdVehicleComponent);
        }
        

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleComponentDto>> GetVehicleComponentById(int id)
        {
            var component = await _vehicleComponentService.GetVehicleComponentByIdAsync(id);
            if (component is null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<VehicleComponentDto>> UpdateVehicleComponent(int id, UpdateVehicleComponentDto dto)
        {
            var updated = await _vehicleComponentService.UpdateVehicleComponentByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicleComponent(int id)
        {
            var deleted = await _vehicleComponentService.DeleteVehicleComponentByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
