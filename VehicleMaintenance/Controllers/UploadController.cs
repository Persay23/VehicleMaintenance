using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleMaintenance.Services.Storage;

namespace VehicleMaintenance.Controllers;

[ApiController]
[Route("api/uploads")]
[Authorize]
public class UploadController(IFileStorage storage) : ControllerBase
{
    private readonly IFileStorage _storage = storage;

    /// <summary>
    /// Stores a receipt image and returns its URL. The caller persists that URL on the
    /// record (e.g. MaintenanceRecord.InvoiceImageUrl). Called only on form submit, so a
    /// discarded form never leaves an orphaned file.
    /// </summary>
    [HttpPost("receipt")]
    public async Task<IActionResult> UploadReceipt(IFormFile image, CancellationToken ct)
    {
        var error = ImageUploadValidator.Validate(image);
        if (error is not null) return BadRequest(new { error });

        await using var stream = image.OpenReadStream();
        var url = await _storage.SaveAsync(stream, image.FileName, image.ContentType, ct);

        return Ok(new { url });
    }
}
