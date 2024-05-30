using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Tenant.Repository;

namespace Point.Of.Sale.Registrations;

public static class RepositoriesRegistration
{
    public static void AddRepositoriesRegistration(this IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<Events.Repository.IRepository, Events.Repository.Repository>();
    }
}
