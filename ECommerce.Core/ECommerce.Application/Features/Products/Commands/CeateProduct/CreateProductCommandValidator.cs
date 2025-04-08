using ECommerce.Domain.Enum;
using FluentValidation;

namespace ECommerce.Application.Features;

public class CreateProductCommandValidator:AbstractValidator<CreateProcductCommnd>
{
    public CreateProductCommandValidator()
    {
        RuleFor(e => e.Name)
           .NotNull().WithMessage("{PropertyName} Is Required")
           .NotEmpty()
           .MaximumLength(50).WithMessage("{PropertyName} Must Not exceed 50 Characters");


        RuleFor(e => e.Price)
             .GreaterThan(0)
             .WithMessage("{PropertyName} Must Not less tha zero");

        RuleFor(e => e.CategoryId)
         .NotEmpty()
         .WithMessage("{PropertyName}  Is Required");

    }
}
