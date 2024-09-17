using System.Net;
using System.Net.Mail;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.External.Settings;
using Microsoft.Extensions.Options;

namespace CitizenFileManagement.Infrastructure.Persistence.Concrete;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        var settings = emailSettings.Value;
        _fromEmail = settings.FromEmail;

        _smtpClient = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
        {
            Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
        {
            IsBodyHtml = true
        };
        await _smtpClient.SendMailAsync(mailMessage);
    }
}