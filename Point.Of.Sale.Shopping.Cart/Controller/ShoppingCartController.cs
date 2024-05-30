using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Events.Attributes;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.LinkToTenant;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.Register;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.Update;
using Point.Of.Sale.Shopping.Cart.Handlers.Command.UpsertLineItem;
using Point.Of.Sale.Shopping.Cart.Handlers.Query.GetAll;
using Point.Of.Sale.Shopping.Cart.Handlers.Query.GetById;
using Point.Of.Sale.Shopping.Cart.Handlers.Query.GetByTenantId;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

namespace Point.Of.Sale.Shopping.Cart.Controller;

[ApiController]
[Route("/api/shopping-cart/")]
[Authorize]
public class ShoppingCartController : ControllerBase
{
    private readonly ISender _sender;

    public ShoppingCartController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    [LogAuditAction]
    public async Task<IActionResult> Register([FromBody] UpsertCart request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
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
    public async Task<IActionResult> Updaate([FromBody] UpsertCart request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            Active = request.Active,
        }, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    [LogAuditAction]
    public async Task<IActionResult> UpsertLineItem([FromBody] UpsertLineItem request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpsertLineItemCommand
        {
            CartId = request.CartId,
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineTotal = request.LineTotal,
        }, cancellationToken);
        return result.ToActionResult();
    }
}
