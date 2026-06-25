namespace VehicleMaintenance.Services.Storage;

/// <summary>
/// Abstraction over file storage so the backing store (local disk now, Azure Blob later)
/// can be swapped via DI without touching callers. Implementations return a URL/key that
/// can be persisted (e.g. on MaintenanceRecord.InvoiceImageUrl) and later served/fetched.
/// </summary>
public interface IFileStorage
{
    /// <summary>Saves the content and returns a URL/key that locates it.</summary>
    Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);

    /// <summary>Deletes a previously saved file. No-op if it does not exist.</summary>
    Task DeleteAsync(string urlOrKey, CancellationToken ct = default);
}
