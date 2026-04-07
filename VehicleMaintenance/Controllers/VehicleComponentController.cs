using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Services;
using VehicleMaintenance.DTOs.VehicleComponents;

namespace VehicleMaintenance.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class VehicleComponentController(VehicleComponentService vehicleComponentService) : Controller
    {
        private readonly VehicleComponentService _vehicleComponentService = vehicleComponentService;
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
        /// <summary>
        /// Get all vehicles
        /// </summary>
        /// <param name="vehicleId">hhhhh</param>
        /// <returns>hhh</returns>
        [HttpGet("{vehicleId}")] // ?? does the path is correct
        public async Task<ActionResult<List<VehicleComponentDto>>> GetVehicleComponentsByVehicleId(int vehicleId)
        {
            var vehicleComponents = await _vehicleComponentService.GetVehicleComponentsByVehicleIdAsync(vehicleId);
            return Ok(vehicleComponents);
        }
    }// alot of repeted code in the controllers, better make a base controller for this? and alot of repetetive code in the services, better make a base service for this?
}
