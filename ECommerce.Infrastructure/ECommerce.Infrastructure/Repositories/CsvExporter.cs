using CsvHelper;
using ECommerce.Application.Contract;
using ECommerce.Application.Models;

namespace ECommerce.Infrastructure.Repositories;

public class CsvExporter:ICsvExporter
{
    public byte[] ExportOrdersToCSV(List<OrderDto> orders)
    {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, new System.Globalization.CultureInfo(0x0000));
        csvWriter.WriteRecord(orders);
        return memoryStream.ToArray();
    }
}
