namespace ECommerce.Application.Exceptions;

public class InternalServerException:Exception
{
    public InternalServerException(string message) : base(message)
    {

    }
    public InternalServerException(string message, string detailes) : base(message)
    {
        Detailes = detailes;
    }
    public string Detailes { get; set; }
}
