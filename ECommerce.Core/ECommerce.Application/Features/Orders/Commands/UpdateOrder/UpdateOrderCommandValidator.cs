using ECommerce.Domain.Enum;
using FluentValidation;

namespace ECommerce.Application.Features;

public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(or => or.CustomerId)
           .NotEqual(Guid.Empty).
            WithMessage("{PropertyName} Is Required");

        RuleFor(or => or.ShippingAddress).
             NotNull()
            .NotEmpty()
            .MaximumLength(50).
             WithMessage("{PropertyName} Must Not exceed 50 Characters");

        RuleFor(or=>or.OrderItems).
             Must(items => items != null && items.Count > 0)
            .WithMessage("The order must contain at least one item.");

        RuleFor(or => or.OrderStatus)
            .IsInEnum()
            .WithMessage("{PropertyName} Not Vd");
    }
}
