using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace VehicleMaintenance.Services.Email;

/// <summary>
/// Generic SMTP email sender (MailKit). If <c>Email:Smtp:Host</c> is not configured (local dev), it
/// logs the message — including links — to the console instead of sending, so the confirmation flow
/// is fully testable without a provider. In production, set the SMTP settings (e.g. Brevo) and the
/// key in user-secrets / App Service config.
///
/// MailKit is used rather than System.Net.Mail.SmtpClient because the latter mishandles the
/// STARTTLS → AUTH sequence with some providers (Brevo returns "5.7.0 Please authenticate first").
/// </summary>
public class SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger) : IEmailService
{
    private readonly IConfiguration _config = config;
    private readonly ILogger<SmtpEmailService> _logger = logger;

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        var host = _config["Email:Smtp:Host"];

        if (string.IsNullOrWhiteSpace(host))
        {
            // Dev fallback — no provider configured. Log the email so links can be clicked from the console.
            _logger.LogWarning(
                "[DEV EMAIL — not sent] To: {To} | Subject: {Subject}\n{Body}",
                to, subject, htmlBody);
            return;
        }

        var port = int.TryParse(_config["Email:Smtp:Port"], out var p) ? p : 587;
        var user = _config["Email:Smtp:User"];
        var key = _config["Email:Smtp:Key"];
        var from = _config["Email:From"] ?? user ?? "noreply@autocare.app";
        var fromName = _config["Email:FromName"] ?? "AutoCare";

        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException(
                "Email:Smtp:Host is set but Email:Smtp:User and/or Email:Smtp:Key are missing. " +
                "Set them in user-secrets (local) or App Service config (Azure).");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        // 465 = implicit TLS; anything else (587/2525) = STARTTLS.
        var secure = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, secure, ct);
        await client.AuthenticateAsync(user, key, ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(quit: true, ct);

        _logger.LogInformation("Confirmation email sent to {To}.", to);
    }
}
