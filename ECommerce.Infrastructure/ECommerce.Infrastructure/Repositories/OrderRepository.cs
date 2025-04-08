using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories;

internal class OrderRepository : BaseRepository<Order>, IOrderRepository
{

    private readonly AppDbContext _context;
    private readonly ILogger<OrderRepository> _logger;
    public OrderRepository(AppDbContext context,IDistributedCache cache, ILogger<OrderRepository> logger) : base(context, cache)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Order> AddOrderWithItems(Order order)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            order.Id = Guid.NewGuid();
            await _context.AddAsync(order);
            foreach (var item in order.OrderItems)
            {
                item.OrderId = order.Id;  
            }
            await _context.OrderItems.AddRangeAsync(order.OrderItems);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            _logger.LogInformation("Commit Transaction And Add Order");
            return order;
        }
        catch (Exception ex)
        {
            // Rollback if any error occurs
            await transaction.RollbackAsync();
            _logger.LogCritical("An error occurred while adding the order with items.", ex);
            throw new Exception("An error occurred while adding the order with items.", ex);
        }


    }
    public async Task<Order> UpdateOrderWithItems(Order updatedOrder)
    {
        try
        {
            var existingOrder = await _context.Orders
               .Include(o => o.OrderItems)
               .FirstOrDefaultAsync(o => o.Id == updatedOrder.Id);

            if (existingOrder == null)
            {
                _logger.LogError("Order not found.");
                throw new NotFoundException("Order not found.");
            }

            existingOrder.CustomerId = updatedOrder.CustomerId;
            existingOrder.OrderStatus = updatedOrder.OrderStatus;
            existingOrder.ShippingAddress = updatedOrder.ShippingAddress;
            existingOrder.Payment = updatedOrder.Payment;

            foreach (var updatedItem in updatedOrder.OrderItems)
            {
                var existingItem = existingOrder.OrderItems
                    .FirstOrDefault(oi => oi.Id == updatedItem.Id);

                if (existingItem != null)
                {
                    existingItem.ProductId = updatedItem.ProductId;
                    existingItem.Quantity = updatedItem.Quantity;
                    existingItem.Price = updatedItem.Price;
                }
                else
                {
                    updatedItem.OrderId = existingOrder.Id;
                    _context.OrderItems.Add(updatedItem);
                }
            }

            var itemIdsToRemove = existingOrder.OrderItems
                .Where(oi => !updatedOrder.OrderItems.Any(ui => ui.Id == oi.Id))
                .Select(oi => oi.Id)
                .ToList();

            foreach (var itemId in itemIdsToRemove)
            {
                var itemToRemove = existingOrder.OrderItems.FirstOrDefault(oi => oi.Id == itemId);
                if (itemToRemove != null)
                {
                    _context.OrderItems.Remove(itemToRemove);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Commit Transaction And Update Order");

            return existingOrder;
        }
        catch (Exception ex)
        {
            
            _logger.LogCritical("Error Happines {0}", ex.Message);
throw;
        }
    }
    public async Task DeleteOrderWithItems(Guid orderId)
    {
        try
        {
            var order = await _context.Orders
              .Include(o => o.OrderItems)
              .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                _logger.LogError("Order not found.");
                throw new NotFoundException("Order not found.");
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            _logger.LogInformation("Commit Transaction And Delete Order");
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
throw;
        }
    }

}
