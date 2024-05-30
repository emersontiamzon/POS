using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Point.Of.Sale.Events.Handlers.Command.LogAuditAction;
using Point.Of.Sale.Shared.Extensions;

namespace Point.Of.Sale.Events.Attributes;

public class LogAuditActionAttribute : ActionFilterAttribute
{
    private readonly ISender _send;
    // public string User { get; set; }
    // public ActivitySource ActivitySource { get; set; }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (next is not null)
        {
            await next();
        }

        var endpoint = context.HttpContext.RequestServices.GetService<IMediator>();

        if (context.Controller is ControllerBase controllerBase)
        {
            var controllerData = context.RouteData.Values.Keys.Select(valuesKey => new Dictionary<string, object?> {{valuesKey, context.RouteData.Values[valuesKey] ?? null}}).ToList();
            controllerData.Add(new Dictionary<string, object?> {{"RequestBody", Regex.Unescape(Regex.Unescape(StringHelper.RemoveSpecialCharacters(context.HttpContext.Request.BodyToString())) ?? string.Empty) ?? string.Empty}});
            controllerData.Add(new Dictionary<string, object?> {{"LocalIp", $"{context.HttpContext.Connection.LocalIpAddress} : {context.HttpContext.Connection.LocalPort}"}});
            controllerData.Add(new Dictionary<string, object?> {{"RemoteIp", $"{context.HttpContext.Connection.RemoteIpAddress} : {context.HttpContext.Connection.RemotePort}"}});

            if (endpoint != null)
            {
                await endpoint.Send(new LogAuditActionCommand(context.HttpContext.User.Identity?.Name ?? string.Empty, controllerData), context.HttpContext.RequestAborted);
            }
            // using var activity = _activitySource.StartActivity("Controller Event");
            // activity?.SetTag("User", User ?? string.Empty);
            // activity?.SetTag("ActionOn", DateTime.UtcNow);
            // activity?.SetTag("Data", controllerData);
        }
    }
}
