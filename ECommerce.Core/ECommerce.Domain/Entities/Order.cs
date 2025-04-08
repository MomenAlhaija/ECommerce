
using System.Net;
using ECommerce.Domain.Enum;

namespace ECommerce.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }    


    public Guid CustomerId { get;  set; }


    public string ShippingAddress { get; set; } = default!;

    public decimal Payment { get;  set; }

    public OrderStatus OrderStatus { get;  set; } = OrderStatus.Pending;
    public ICollection<OrderItem> OrderItems { get; set; }
}
