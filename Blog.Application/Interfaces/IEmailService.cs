namespace BlogApi.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string name,string subject, string body);
    Task SendBulkEmailAsync(HashSet<string> toEmails,string name, string subject, string body);
}