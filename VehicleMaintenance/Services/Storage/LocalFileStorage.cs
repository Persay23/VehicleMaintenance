namespace VehicleMaintenance.Services.Storage;

/// <summary>
/// Stores files under wwwroot and returns a relative URL served by UseStaticFiles().
/// Swap this registration for an AzureBlobStorage implementation to move to the cloud —
/// no caller changes required.
/// </summary>
public class LocalFileStorage : IFileStorage
{
    private const string SubFolder = "uploads/receipts";
    private readonly string _webRoot;

    public LocalFileStorage(IWebHostEnvironment env)
    {
        // WebRootPath is null until wwwroot exists; fall back to the conventional path.
        _webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
    }

    public async Task<string> SaveAsync(
        Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        var ext = Path.GetExtension(fileName);
        var storedName = $"{Guid.NewGuid():N}{ext}";

        var folder = Path.Combine(_webRoot, SubFolder);
        Directory.CreateDirectory(folder);

        var fullPath = Path.Combine(folder, storedName);
        await using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await content.CopyToAsync(fs, ct);
        }

        // URL form (forward slashes) — this is what gets persisted on the record.
        return $"/{SubFolder}/{storedName}";
    }

    public Task DeleteAsync(string urlOrKey, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urlOrKey))
            return Task.CompletedTask;

        var relative = urlOrKey.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(_webRoot, relative);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
