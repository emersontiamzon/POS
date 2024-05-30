using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Abstraction.Assembly;
using Point.Of.Sale.Events.Behaviours;

namespace Point.Of.Sale.Registrations;

public static class MediatrRegistration
{
    public static void AddMediatrRegistration(this IServiceCollection services)
    {
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Tenant.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Shared.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Auth.Assembly.AssemblyReference.Assembly));
        services.AddMediatR(m => m.RegisterServicesFromAssemblies(Events.Assembly.AssemblyReference.Assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
    }
}
