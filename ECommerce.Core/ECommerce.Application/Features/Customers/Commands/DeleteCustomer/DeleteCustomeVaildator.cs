using FluentValidation;

namespace ECommerce.Application.Features;

public class DeleteCustomeVaildator:AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomeVaildator()
    {
        RuleFor(cu => cu.Id)
           .NotEmpty()
           .NotNull()
           .NotEqual(Guid.Empty)
           .WithMessage("{PropertyName} Is Required");
    }
}
