using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using ECommerce.Application.Contract;
using ECommerce.Application.Models.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ECommerce.Infrastructure.MailHelper;

public class EmailService : IEmailService
{
    public EmailSettings _emailSettings { get; set; }
    public ILogger<EmailService> _logger;
    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }
    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);
        var subject = email.Subject;
        var body = email.Body;
        var to = new EmailAddress(email.To);

        var from = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName
        };
        var sendGridMessage = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(sendGridMessage);

        if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation("Send Email To {0} Successfully", email.To);
            return true;
        }
        _logger.LogError("failed to send Email To {0} with Body {1}", email.To, body);
        return false;

    }
}