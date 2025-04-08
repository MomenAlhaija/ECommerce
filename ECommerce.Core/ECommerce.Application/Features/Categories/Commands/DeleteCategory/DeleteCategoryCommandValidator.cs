
using FluentValidation;

namespace ECommerce.Application.Features;

public class DeleteCategoryCommandValidator:AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(ca => ca.Id)
           .NotEmpty()
           .NotNull()
           .NotEqual(Guid.Empty)
           .WithMessage("{PropertyName} Is Required");
    }
}

