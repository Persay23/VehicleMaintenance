using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.Security;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController(
        IPredictionService iPredictionService,
        IVehicleOwnershipService ownership) : ControllerBase
    {
        private readonly IPredictionService _iPredictionService = iPredictionService;
        private readonly IVehicleOwnershipService _ownership = ownership;

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PredictionDto>> GetPredictionById(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsPredictionAsync(userId, id)) return NotFound();

            var prediction = await _iPredictionService.GetPredictionByIdAsync(id);
            if (prediction is null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }

        [HttpGet("vehicle/{vehicleId:int}")]
        public async Task<ActionResult<PredictionDto[]>> GetPredictionsByVehicle(int vehicleId)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, vehicleId)) return NotFound();

            var predictions = await _iPredictionService.GetPredictionsByVehicleAsync(vehicleId);
            return Ok(predictions);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<PredictionDto>> UpdatePrediction(int id, UpdatePredictionDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsPredictionAsync(userId, id)) return NotFound();

            var updated = await _iPredictionService.UpdatePredictionByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePrediction(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsPredictionAsync(userId, id)) return NotFound();

            var deleted = await _iPredictionService.DeletePredictionByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
