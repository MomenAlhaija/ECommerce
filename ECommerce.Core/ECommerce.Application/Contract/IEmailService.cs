using ECommerce.Application.Models.Mail;

namespace ECommerce.Application.Contract;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}
