using ECommerce.Application.Models;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Contract;

public interface IOrderRepository:IAsyncRepository<Order>
{
    Task DeleteOrderWithItems(Guid orderId);
    Task<Order> UpdateOrderWithItems(Order updatedOrder);
    Task<Order> AddOrderWithItems(Order order);
}
