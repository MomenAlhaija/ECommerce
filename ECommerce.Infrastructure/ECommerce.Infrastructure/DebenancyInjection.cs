using ECommerce.Application.Contract;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.MailHelper;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;


namespace ECommerce.Infrastructure;

public static class DebenancyInjection
{
    public static void ConfigureLogging(IServiceCollection services, IConfiguration config)
    {
        var logLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), config["Serilog:MinimumLevel"]);

        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(
                path: Path.Combine(Directory.GetCurrentDirectory(), config["Serilog:FileLocation"]!.ToString(), "log-.txt"),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: logLevel
            );

        var logger = loggerConfiguration.CreateLogger();

        services.AddLogging(builder => builder.AddSerilog(logger));
    }

    public static void RegisterInfrastuctureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("SqlServerConnectionString");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(sqlConnectionString)
        );

        var redisConnectionString = configuration.GetSection("RedisConnectionString:Redis").Value;
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });



        services.AddTransient<IEmailService, EmailService>();

        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<ICsvExporter, CsvExporter>();

        
        ConfigureLogging(services, configuration);
    }

}
