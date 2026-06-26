namespace VehicleMaintenance.Services.Export;

/// <summary>A rendered export file ready to return from a controller.</summary>
public record VehicleExportFile(byte[] Content, string ContentType, string FileName);

public interface IVehicleExportService
{
    /// <summary>
    /// Renders a vehicle's full service history as Markdown or PDF.
    /// Returns null if the vehicle does not exist or is not owned by <paramref name="userId"/>.
    /// </summary>
    Task<VehicleExportFile?> ExportAsync(int vehicleId, string userId, string format, CancellationToken ct = default);
}
