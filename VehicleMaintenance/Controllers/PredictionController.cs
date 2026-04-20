using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.Prediction;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController(IPredictionService iPredictionService) : ControllerBase
    {
        private readonly IPredictionService _iPredictionService = iPredictionService;

        [HttpGet]
        public async Task<ActionResult<List<PredictionDto>>> GetPredictions()
        {
            var predictions = await _iPredictionService.GetAllPredictionsAsync();
            return Ok(predictions);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PredictionDto>> GetPredictionById(int id)
        {
            var prediction = await _iPredictionService.GetPredictionByIdAsync(id);
            if (prediction is null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }

        [HttpPost]
        public async Task<ActionResult<PredictionDto>> CreatePrediction(CreatePredictionDto createPredictionDto)
        {
            var created = await _iPredictionService.CreatePredictionAsync(createPredictionDto);
            return CreatedAtAction(nameof(GetPredictionById), new { id = created.PredictionId }, created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<PredictionDto>> UpdatePrediction(int id, UpdatePredictionDto dto)
        {
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
            var deleted = await _iPredictionService.DeletePredictionByIdAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
