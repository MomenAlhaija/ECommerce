namespace ECommerce.Application.Exceptions;

public class ValidationException:Exception
{
    public List<string> validationErrors { get; set; }

    public ValidationException(FluentValidation.Results.ValidationResult validationResult)
    {
        validationErrors = new List<string>();
        foreach (var validationError in validationResult.Errors)
        {
            validationErrors.Add(validationError.ErrorMessage);
        }
    }
}
