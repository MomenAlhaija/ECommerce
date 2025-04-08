namespace ECommerce.Application.Exceptions;

public class BadRequestException:Exception
{
    public BadRequestException(string message) : base(message)
    {

    }
    public BadRequestException(string message, string detailes) : base(message)
    {
        Detailes = detailes;
    }
    public string Detailes { get; set; }
}
