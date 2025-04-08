using FluentValidation;

namespace ECommerce.Application.Features;

public class CreateCategoryCommandValidator:AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(e => e.Name)
          .NotNull().WithMessage("{PropertyName} Is Required")
          .NotEmpty()
          .MaximumLength(50).WithMessage("{PropertyName} Must Not exceed 50 Characters");

        RuleFor(e => e.Description)
         .NotNull().WithMessage("{PropertyName} Is Required")
         .NotEmpty()
         .MaximumLength(100).WithMessage("{PropertyName} Must Not exceed 100 Characters");



    }
}
