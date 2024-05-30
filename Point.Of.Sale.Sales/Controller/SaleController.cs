using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Events.Attributes;
using Point.Of.Sale.Sales.Handlers.Command.LinkToTenant;
using Point.Of.Sale.Sales.Handlers.Command.Register;
using Point.Of.Sale.Sales.Handlers.Command.Update;
using Point.Of.Sale.Sales.Handlers.Command.UpsertLineItem;
using Point.Of.Sale.Sales.Handlers.Query.GetAll;
using Point.Of.Sale.Sales.Handlers.Query.GetById;
using Point.Of.Sale.Sales.Handlers.Query.GetByTenantId;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

namespace Point.Of.Sale.Sales.Controller;

[ApiController]
[Route("/api/sale/")]
[Authorize]
public class SaleController : ControllerBase
{
    private readonly ISender _sender;

    public SaleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    [LogAuditAction]
    public async Task<IActionResult> Register([FromBody] UpsertSale newSale, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterCommand
        {
            CustomerId = newSale.CustomerId,
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
    public async Task<IActionResult> Update([FromBody] UpsertSale request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            LineItems = request.LineItems,
            SubTotal = request.SubTotal,
            TotalDiscounts = request.TotalDiscounts,
            TaxPercentage = request.TaxPercentage,
            SalesTax = request.SalesTax,
            TotalSales = request.TotalSales,
            SaleDate = DateTime.UtcNow,
            Active = true,
            Status = request.Status,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }

    [HttpPut]
    [Route("line-item/")]
    [LogAuditAction]
    public async Task<IActionResult> UpsertLineItem([FromBody] UpsertSaleLineItem request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpsertLineItemCommand
        {
            SaleId = request.SaleId,
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineDiscount = request.LineDiscount,
            Active = true,
            LineTax = request.LineTax,
            ProductDescription = request.ProductDescription,
            LineTotal = request.LineTotal,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }
}
