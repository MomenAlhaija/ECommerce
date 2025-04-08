using FluentValidation;

namespace ECommerce.Application.Features;

public class DeleteOrderCommandValidator:AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(or => or.Id)
            .NotEmpty()
            .NotNull()
            .NotEqual(Guid.Empty)
            .WithMessage("{PropertyName} Is Required");
    }
}
