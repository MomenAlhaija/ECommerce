using ECommerce.Application.Models;
using ECommerce.Application.Models.Orders;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enum;
using MediatR;

namespace ECommerce.Application.Features;

public class UpdateOrderCommand:IRequest<OrderDto>
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }


    public string ShippingAddress { get; set; } = default!;

    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public ICollection<UpdateOrderItemDto> OrderItems { get; set; }

}
