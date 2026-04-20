using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class VehicleComponentController(IVehicleComponentService iVehicleComponentService) : ControllerBase
    {
        private readonly IVehicleComponentService _iVehicleComponentService = iVehicleComponentService;

        /// <summary>
        /// Get all vehicles components
        /// </summary>
        /// <param name="vehicleId">hhhhh</param>
        /// <returns>hhh</returns>
        [HttpGet]
        public async Task<ActionResult<List<VehicleComponentDto>>> GetVehicleComponents()
        {
            var vehicleComponents = await _iVehicleComponentService.GetAllVehicleComponentsAsync();
            return Ok(vehicleComponents);
        }

        [HttpGet("vehicle/{vehicleId}/health")]
        public async Task<IActionResult> GetComponentHealth(int vehicleId)
        {
            var health = await _iVehicleComponentService.GetComponentHealthAsync(vehicleId);
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
            var createdVehicleComponent = await _iVehicleComponentService.CreateVehicleComponentAsync(createVehicleComponentDto);
            return Ok(createdVehicleComponent);
        }
        

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VehicleComponentDto>> GetVehicleComponentById(int id)
        {
            var component = await _iVehicleComponentService.GetVehicleComponentByIdAsync(id);
            if (component is null)
            {
                return NotFound();
            }

            return Ok(component);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<VehicleComponentDto>> UpdateVehicleComponent(int id, UpdateVehicleComponentDto dto)
        {
            var updated = await _iVehicleComponentService.UpdateVehicleComponentByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicleComponent(int id)
        {
            var deleted = await _iVehicleComponentService.DeleteVehicleComponentByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
