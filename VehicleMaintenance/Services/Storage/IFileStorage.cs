namespace VehicleMaintenance.Services.Storage;

public interface IFileStorage
{
    Task<string> SaveAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);
    Task DeleteAsync(string urlOrKey, CancellationToken ct = default);
}
