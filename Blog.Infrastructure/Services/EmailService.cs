using BlogApi.Application.Options;
using BlogApi.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BlogApi.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptionsMonitor<EmailSettings> options)
    {
        _settings = options.CurrentValue;
    }

    public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlContent)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        htmlContent = htmlContent.Replace("@Name", toName);
        message.Body = new TextPart("html") { Text = htmlContent };

        using var client = new SmtpClient();
        client.Timeout = 20000;
        await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendBulkEmailAsync(HashSet<string> toEmails, string subject,string name, string body)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);

        foreach (var recipient in toEmails)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress(name,recipient));
            message.Subject = subject;
            var htmlContent = body.Replace("@Name", name);
            message.Body = new TextPart("html") { Text = htmlContent };
           
            await client.SendAsync(message);
        }

        await client.DisconnectAsync(true);
    }
}