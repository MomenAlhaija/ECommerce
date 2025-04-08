using FluentValidation;

namespace ECommerce.Application.Features;

public class UpdateCategoryCommandValidator:AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(ca => ca.Id)
          .NotEmpty()
          .NotNull()
          .NotEqual(Guid.Empty)
          .WithMessage("{PropertyName} Is Required");

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
