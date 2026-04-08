using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.Services;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController(PredictionService predictionService) : ControllerBase
    {
        private readonly PredictionService _predictionService = predictionService;

        [HttpGet]
        public async Task<ActionResult<List<PredictionDto>>> GetPredictions()
        {
            var predictions = await _predictionService.GetAllPredictionsAsync();
            return Ok(predictions);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PredictionDto>> GetPredictionById(int id)
        {
            var prediction = await _predictionService.GetPredictionByIdAsync(id);
            if (prediction is null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }

        [HttpPost]
        public async Task<ActionResult<PredictionDto>> CreatePrediction(CreatePredictionDto createPredictionDto)
        {
            var created = await _predictionService.CreatePredictionAsync(createPredictionDto);
            return Ok(created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<PredictionDto>> UpdatePrediction(int id, UpdatePredictionDto dto)
        {
            var updated = await _predictionService.UpdatePredictionByIdAsync(id, dto);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePrediction(int id)
        {
            var deleted = await _predictionService.DeletePredictionByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
