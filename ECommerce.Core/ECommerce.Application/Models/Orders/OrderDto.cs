using ECommerce.Application.Models.Orders;
using ECommerce.Domain.Enum;

namespace ECommerce.Application.Models;

public class OrderDto
{
    public Guid Id { get; set; }


    public Guid CustomerId { get; set; }


    public string ShippingAddress { get; set; } = default!;

    public decimal Payment { get; set; }

    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public ICollection<OrderItemDto> OrderItems { get; set; }
}
