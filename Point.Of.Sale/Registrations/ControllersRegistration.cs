using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Point.Of.Sale.Registrations;

public static class ControllersRegistration
{
    public static void AddControllersRegistration(this IServiceCollection services)
    {
        services.AddMvc().AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Tenant")));
        services.AddMvc().AddApplicationPart(System.Reflection.Assembly.Load(new AssemblyName("Point.Of.Sale.Auth")));
    }
}
