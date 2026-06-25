namespace VehicleMaintenance.Services.Storage;

/// <summary>
/// Shared validation for uploaded receipt images, used by both the parse and upload endpoints.
/// </summary>
public static class ImageUploadValidator
{
    public const long MaxBytes = 10 * 1024 * 1024; // 10 MB

    public static readonly string[] AllowedContentTypes =
        { "image/jpeg", "image/png", "image/webp" };

    /// <summary>Returns an error message if the file is invalid, or null if it is acceptable.</summary>
    public static string? Validate(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return "No image was provided.";

        if (file.Length > MaxBytes)
            return $"Image is too large. Maximum size is {MaxBytes / (1024 * 1024)} MB.";

        if (!AllowedContentTypes.Contains(file.ContentType))
            return $"Unsupported image type '{file.ContentType}'. Allowed: {string.Join(", ", AllowedContentTypes)}.";

        return null;
    }
}
