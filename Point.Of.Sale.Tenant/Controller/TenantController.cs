using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Events.Attributes;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Handlers.Command.Patch;
using Point.Of.Sale.Tenant.Handlers.Command.RegisterTenant;
using Point.Of.Sale.Tenant.Handlers.Command.Update;
using Point.Of.Sale.Tenant.Handlers.Query.GetAllTenants;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Tenant.Controller;

[ApiController]
[Route("/api/tenant/")]
[Authorize]
public sealed class TenantController : ControllerBase
{
    private static ActivitySource _activitySource;
    private readonly ILogger<TenantController> _logger;
    private readonly IOptions<PosConfiguration> _options;
    private readonly ISender _sender;

    public TenantController(ISender sender, ILogger<TenantController> logger, IOptions<PosConfiguration> options, ActivitySource activitySource)
    {
        _sender = sender;
        _logger = logger;
        _options = options;
        _activitySource = activitySource;
    }

    [HttpPost]
    [Route("register")]
    [LogAuditAction]
    public async Task<IActionResult> Register([FromBody] UpsertTenant request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            Code = request.Code,
            Name = request.Name,
            Type = TenantType.NonSpecific,
            Active = false,
        }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    [LogAuditAction]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetTenantById(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [LogAuditAction]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAll(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut]
    [LogAuditAction]
    public async Task<IActionResult> Upsert([FromBody] UpsertTenant request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Type = request.Type,
            Active = request.Active,
        }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPatch]
    [Route("{id:int}")]
    [LogAuditAction]
    public async Task<IActionResult> Patch(int id, JsonPatchDocument<Persistence.Models.Tenant> patch, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new PatchCommand
        {
            Id = id,
            Patch = patch,
        }, cancellationToken);
        return result.ToActionResult();
    }
}
