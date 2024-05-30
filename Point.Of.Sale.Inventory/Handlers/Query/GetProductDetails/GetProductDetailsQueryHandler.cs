using MediatR;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetProductDetails;

public sealed class GetProductDetailsQueryHandler : IQueryHandler<GetProductDetailsQuery, ProductDetailsResponse>
{
    private readonly ISender _sender;

    public GetProductDetailsQueryHandler(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IFluentResults<ProductDetailsResponse>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
    {
        var tenant = _sender.Send(new GetTenantById(request.tenandId), cancellationToken);
        var category = _sender.Send(new Category.Handlers.Query.GetById.GetById(request.categoryId), cancellationToken);
        var product = _sender.Send(new Product.Handlers.Query.GetById.GetById(request.productId), cancellationToken);
        var supplier = _sender.Send(new Supplier.Handlers.Query.GetById.GetById(request.supplierId), cancellationToken);

        await Task.WhenAll(tenant, category, product, supplier);

        var result = new ProductDetailsResponse
        {
            TenantResponse = await tenant,
            ProductResponse = await product,
            CategoryResponse = await category,
            SupplierResponse = await supplier,
        };

        return ResultsTo.Success(result);
    }
}