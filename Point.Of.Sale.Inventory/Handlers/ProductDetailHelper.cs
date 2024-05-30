using MediatR;
using Point.Of.Sale.Category.Handlers.Query.GetById;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Inventory.Handlers;

public class ProductDetailHelper
{
    private readonly int _categoryId;
    private readonly int _productId;
    private readonly ISender _sender;
    private readonly int _supplierId;
    private readonly int _tenandId;

    public ProductDetailHelper(ISender sender)
    {
        _sender = sender;
    }

    public ProductDetailHelper(int tenandId, int categoryId, int productId, int supplierId)
    {
        _tenandId = tenandId;
        _categoryId = categoryId;
        _productId = productId;
        _supplierId = supplierId;
    }

    public Task<IFluentResults<TenantResponse>> TenantResponse { get; private set; }

    public Task<IFluentResults<ProductResponse>> ProductResponse { get; private set; }

    public Task<IFluentResults<CategoryResponse>> CategoryResponse { get; private set; }

    public Task<IFluentResults<SupplierResponse>> SupplierResponse { get; private set; }

    public async Task GetProductDetail(CancellationToken cancellationToken)
    {
        TenantResponse = _sender.Send(new GetTenantById(_tenandId), cancellationToken);
        CategoryResponse = _sender.Send(new GetById(_categoryId), cancellationToken);
        ProductResponse = _sender.Send(new Product.Handlers.Query.GetById.GetById(_productId), cancellationToken);
        SupplierResponse = _sender.Send(new Supplier.Handlers.Query.GetById.GetById(_supplierId), cancellationToken);

        await Task.WhenAll(TenantResponse, CategoryResponse, ProductResponse, SupplierResponse);
    }
}