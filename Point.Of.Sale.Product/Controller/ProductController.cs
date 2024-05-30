using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Point.Of.Sale.Category.Handlers.Query.GetById;
using Point.Of.Sale.Events.Attributes;
using Point.Of.Sale.Product.Handlers.Command.LinkToTenant;
using Point.Of.Sale.Product.Handlers.Command.Register;
using Point.Of.Sale.Product.Handlers.Command.Update;
using Point.Of.Sale.Product.Handlers.Query.GetAll;
using Point.Of.Sale.Product.Handlers.Query.GetByTenantId;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

namespace Point.Of.Sale.Product.Controller;

[ApiController]
[Route("/api/product/")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("register")]
    [LogAuditAction]
    public async Task<IActionResult> Register([FromBody] UpsertProduct request, CancellationToken cancellationToken = default)
    {
        var category = _sender.Send(new GetById(request.CategoryId), cancellationToken);
        var supplier = _sender.Send(new Supplier.Handlers.Query.GetById.GetById(request.SupplierId), cancellationToken);

        await Task.WhenAll(category, supplier);

        if ((await category).IsNotFoundOrBadRequest())
        {
            return (await category).ToActionResult();
        }

        if ((await supplier).IsNotFoundOrBadRequest())
        {
            return (await supplier).ToActionResult();
        }

        var result = await _sender.Send(new RegisterCommand
        {
            SkuCode = request.SkuCode,
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            SupplierId = request.SupplierId,
            CategoryId = request.CategoryId,
            WebSite = request.WebSite,
            Image = request.Image,
            BarCodeType = request.BarCodeType,
            Barcode = request.Barcode,
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
        var result = await _sender.Send(new Handlers.Query.GetById.GetById(id), cancellationToken);
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
    public async Task<IActionResult> Update([FromBody] UpsertProduct request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateCommand
        {
            Id = request.Id,
            TenantId = request.TenantId,
            SkuCode = request.SkuCode,
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            SupplierId = request.SupplierId,
            CategoryId = request.CategoryId,
            WebSite = request.WebSite,
            Barcode = request.Barcode,
            BarCodeType = request.BarCodeType,
            Image = request.Image,
            Active = request.Active,
        }, cancellationToken);

        if (result.IsFailure() || result.IsNotFoundOrBadRequest())
        {
            return result.ToActionResult();
        }

        return result.ToActionResult();
    }
}
