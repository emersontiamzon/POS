using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Abstraction.Assembly;

namespace Point.Of.Sale.Registrations;

public static class ScrutorRegistration
{
    public static void AddScrutorRegistration(this IServiceCollection service)
    {
        service.Scan(
            selector => selector
                .FromAssemblies(AssemblyReference.Assembly)
                .FromAssemblies(Persistence.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Tenant.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Auth.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Shared.Assembly.AssemblyReference.Assembly)
                .FromAssemblies(Events.Assembly.AssemblyReference.Assembly)
                .AddClasses(false)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}
