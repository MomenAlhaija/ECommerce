using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Models.Orders;

public class UpdateOrderItemDto:CreateOrderItemsDto
{
    public Guid Id { get; set; }
}
