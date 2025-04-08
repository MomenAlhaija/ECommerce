using ECommerce.Application.Models;

namespace ECommerce.Application.Contract;

public interface ICsvExporter
{
    byte[] ExportOrdersToCSV(List<OrderDto> orders);
}

