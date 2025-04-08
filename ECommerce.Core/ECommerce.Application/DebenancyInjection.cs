using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace ECommerce.Application;

public static class DebenancyInjection
{
    public static void RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Scoped);
    }
}
