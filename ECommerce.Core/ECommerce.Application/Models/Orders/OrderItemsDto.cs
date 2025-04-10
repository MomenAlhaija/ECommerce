﻿namespace ECommerce.Application.Models.Orders;

public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
