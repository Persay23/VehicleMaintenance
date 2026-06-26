using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.DTOs.GeneralExpense;
using VehicleMaintenance.Extensions;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.Security;

namespace VehicleMaintenance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeneralExpenseController(
        IGeneralExpenseService service,
        IVehicleOwnershipService ownership) : ControllerBase
    {
        private readonly IGeneralExpenseService _service = service;
        private readonly IVehicleOwnershipService _ownership = ownership;

        [HttpGet("vehicle/{vehicleId:int}")]
        public async Task<ActionResult<GeneralExpenseDto[]>> GetGeneralExpensesByVehicle(int vehicleId)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, vehicleId)) return NotFound();

            var expenses = await _service.GetGeneralExpensesByVehicleIdAsync(vehicleId);
            return Ok(expenses);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<GeneralExpenseDto[]>> GetGeneralExpensesByUser(string userId)
        {
            if (userId != User.GetUserId()) return Forbid();

            var expenses = await _service.GetGeneralExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralExpenseDto>> GetGeneralExpenseById(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsExpenseAsync(userId, id)) return NotFound();

            var expense = await _service.GetGeneralExpenseByIdAsync(id);
            if (expense is null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPost]
        public async Task<ActionResult<GeneralExpenseDto>> CreateGeneralExpense(CreateGeneralExpenseDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsVehicleAsync(userId, dto.VehicleId)) return Forbid();

            var created = await _service.CreateGeneralExpenseAsync(dto);
            return CreatedAtAction(nameof(GetGeneralExpenseById), new { id = created.GeneralExpenseId }, created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<GeneralExpenseDto>> UpdateGeneralExpense(int id, UpdateGeneralExpenseDto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsExpenseAsync(userId, id)) return NotFound();

            var updated = await _service.UpdateGeneralExpenseAsync(id, dto);
            if (updated is null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGeneralExpense(int id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();
            if (!await _ownership.OwnsExpenseAsync(userId, id)) return NotFound();

            var deleted = await _service.DeleteGeneralExpenseAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
