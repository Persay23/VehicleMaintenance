namespace VehicleMaintenance.Services.Email;

public interface IEmailService
{
    /// <summary>Sends an HTML email. In dev (no SMTP host configured) it logs the message instead.</summary>
    Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}
