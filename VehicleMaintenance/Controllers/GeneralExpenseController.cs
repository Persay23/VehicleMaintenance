using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.GeneralExpense;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeneralExpenseController(IGeneralExpenseService service) : ControllerBase
    {
        private readonly IGeneralExpenseService _service = service;

        [HttpGet("vehicle/{vehicleId:int}")]
        public async Task<ActionResult<List<GeneralExpenseDto>>> GetGeneralExpensesByVehicle(int vehicleId)
        {
            var expenses = await _service.GetGeneralExpensesByVehicleIdAsync(vehicleId);
            return Ok(expenses);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<GeneralExpenseDto>>> GetGeneralExpensesByUser(string userId)
        {
            var expenses = await _service.GetGeneralExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralExpenseDto>> GetGeneralExpenseById(int id)
        {
            var expense = await _service.GetGeneralExpenseByIdAsync(id);
            if (expense is null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<GeneralExpenseDto>> CreateGeneralExpense(CreateGeneralExpenseDto dto)
        {
            var created = await _service.CreateGeneralExpenseAsync(dto);
            return CreatedAtAction(nameof(GetGeneralExpenseById), new { id = created.GeneralExpenseId }, created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<GeneralExpenseDto>> UpdateGeneralExpense(int id, UpdateGeneralExpenseDto dto)
        {
            var updated = await _service.UpdateGeneralExpenseAsync(id, dto);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGeneralExpense(int id)
        {
            var deleted = await _service.DeleteGeneralExpenseAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
