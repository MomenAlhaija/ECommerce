using FluentValidation;

namespace ECommerce.Application.Features;

public class UpdateProdcutVaildator:AbstractValidator<UpdateProductCommand>
{
    public UpdateProdcutVaildator()
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

        RuleFor(p => p.Id)
            .NotEmpty()
            .WithMessage("{PropertyName} Is Required");
    }
}
