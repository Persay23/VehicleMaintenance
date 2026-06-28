using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using VehicleMaintenance.Models.Entities;

namespace VehicleMaintenance.Services.Email;

public class SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
    : IEmailService, IEmailSender<User>
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

        var secure = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, secure, ct);
        await client.AuthenticateAsync(user, key, ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(quit: true, ct);

        _logger.LogInformation("Confirmation email sent to {To}.", to);
    }

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink) =>
        SendAsync(email, "Confirm your AutoCare account",
            $"<p>Please confirm your account by clicking: <a href='{confirmationLink}'>{confirmationLink}</a></p>");

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink) =>
        SendAsync(email, "Reset your AutoCare password",
            $"<p>Click to reset your password: <a href='{resetLink}'>{resetLink}</a></p>");

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode) =>
        SendAsync(email, "Your AutoCare reset code",
            $"<p>Your password reset code is: <strong>{resetCode}</strong></p>");
}
