using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Customer.Handlers.Command.LinkToTenant;
using Point.Of.Sale.Customer.Handlers.Command.Register;
using Point.Of.Sale.Customer.Handlers.Command.Update;
using Point.Of.Sale.Customer.Handlers.Query.GetAll;
using Point.Of.Sale.Customer.Handlers.Query.GetById;
using Point.Of.Sale.Customer.Handlers.Query.GetByTenantId;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Events.Attributes;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

namespace Point.Of.Sale.Customer.Controller;

[ApiController]
[Route("/api/customer/")]
[Authorize]
public class CustomerController : ControllerBase
{
    private readonly ISender _sender;

    public CustomerController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    [LogAuditAction]
    public async Task<IActionResult> Register([FromBody] AddCustomer newCategory, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            TenantId = newCategory.TenantId,
            Name = newCategory.Name,
            Address = newCategory.Address,
            PhoneNumber = newCategory.PhoneNumber,
            Email = newCategory.Email,
        }, cancellationToken);

        return result.ToActionResult();
    }

    [HttpGet]
    [LogAuditAction]
    public async Task<IActionResult> All(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    [LogAuditAction]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetById(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("{entityId:int}/{tenantId:int}")]
    [LogAuditAction]
    public async Task<IActionResult> LinkToTenant(int entityId, int tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _sender.Send(new GetTenantById(tenantId), cancellationToken);

        if (tenant.IsFailure() || tenant.IsNotFoundOrBadRequest())
        {
            return tenant.ToActionResult();
        }

        var result = await _sender.Send(new LinkToTenantCommand(tenantId, entityId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet]
    [Route("tenant/{tenantId:int}")]
    [LogAuditAction]
    public async Task<IActionResult> GetByTenantId(int tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _sender.Send(new GetTenantById(tenantId), cancellationToken);

        if (tenant.IsFailure() || tenant.IsNotFoundOrBadRequest())
        {
            return tenant.ToActionResult();
        }

        var result = await _sender.Send(new GetByTenantIdQuery(tenantId), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPut]
    [LogAuditAction]
    public async Task<IActionResult> Update([FromBody] UpdateCustomer request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }
}
