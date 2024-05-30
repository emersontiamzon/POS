using Honeycomb.OpenTelemetry;
using Honeycomb.Serilog.Sink;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Point.Of.Sale.Shared.Configuration;
using Serilog;

namespace Point.Of.Sale.Registrations;

public static class OpenTelemetryAndLogging
{
    public static void AddOpenTelemetryAndLoggingRegistration(this WebApplicationBuilder builder, PosConfiguration configuration)
    {
        HoneycombOptions honeycombOptions = new()
        {
            ApiKey = configuration.HoneyComb.ApiKey,
            Dataset = configuration.HoneyComb.Dataset,
            ServiceName = configuration.HoneyComb.ServiceName,
            Endpoint = configuration.HoneyComb.Endpoint,
        };

        builder.Services.AddOpenTelemetry().WithTracing(otelBuilder =>
        {
            otelBuilder
                // .AddConsoleExporter()
                .AddSource(configuration.General.ServiceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault().AddService(configuration.General.ServiceName, serviceVersion: configuration.General.ServiceVersion)
                )
                .AddCommonInstrumentations()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHoneycomb(honeycombOptions);
        });

        builder.Services.Configure<AspNetCoreInstrumentationOptions>(i =>
            i.Filter = ctx => !ctx.Request.Path.ToString().Contains("swagger")
                              && !ctx.Request.Path.ToString().Contains("/_framework"));

        builder.Services.AddSingleton(TracerProvider.Default.GetTracer(honeycombOptions.ServiceName));

        // using var log = new LoggerConfiguration()
        //     .WriteTo.HoneycombSink(configuration.HoneyComb.ServiceName, configuration.HoneyComb.ApiKey)
        //     .WriteTo.HoneycombSink(configuration.HoneyComb.ServiceName, configuration.HoneyComb.ApiKey, 100, TimeSpan.FromSeconds(5), null, configuration.HoneyComb.Endpoint)
        //     .Enrich.FromLogContext()
        //     .CreateLogger();
        //
        // builder.Host.UseSerilog((ctx, lc) => lc
        //     .WriteTo.HoneycombSink(configuration.HoneyComb.Dataset, configuration.HoneyComb.ApiKey)
        //     .ReadFrom.Configuration(ctx.Configuration));

        // builder.Logging.AddSerilog(log);
        // builder.Services.AddSingleton<ILogger>(log);
    }
}
