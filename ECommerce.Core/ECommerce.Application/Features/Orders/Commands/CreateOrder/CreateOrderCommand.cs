using ECommerce.Application.Models;
using ECommerce.Application.Models.Orders;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enum;
using MediatR;

namespace ECommerce.Application.Features;
public class CreateOrderCommand:IRequest<OrderDto>
{

    public List<CreateOrderItemsDto> OrderItems { get; set; }

    public Guid CustomerId { get; set; }

    public string ShippingAddress { get; set; } = default!;


}

