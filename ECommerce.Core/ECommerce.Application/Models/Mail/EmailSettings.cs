namespace ECommerce.Application.Models.Mail;

public class EmailSettings
{
    public static string SectionName { get; set; } = "EmailSettings";
    public string ApiKey { get; set; } = default!;

    public string FromAddress { get; set; } = default!;

    public string FromName { get; set; } = default!;
}
