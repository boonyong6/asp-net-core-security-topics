using Microsoft.AspNetCore.Identity.UI.Services;

namespace AspNetCoreIdentitySandbox.WebApi;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogDebug($"Hi {email}, {htmlMessage}");

        return Task.CompletedTask;
    }
}
