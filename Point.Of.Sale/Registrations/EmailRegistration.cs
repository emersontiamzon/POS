using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Shared.Configuration;

namespace Point.Of.Sale.Registrations;

public static class EmailRegistration
{
    public static void AddEmailRegistrration(this IServiceCollection services, Smtp smtp)
    {
        services.AddFluentEmail(smtp.Sender)
            .AddSmtpSender(smtp.Host,
                smtp.Port,
                smtp.UserName,
                smtp.Password);
    }
}
