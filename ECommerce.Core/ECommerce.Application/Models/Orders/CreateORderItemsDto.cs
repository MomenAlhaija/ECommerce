namespace ECommerce.Application.Models.Orders;

public class CreateOrderItemsDto
{
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}