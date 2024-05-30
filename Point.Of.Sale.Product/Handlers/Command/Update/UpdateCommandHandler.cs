using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Product.Handlers.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly ILogger<UpdateCommandHandler> _logger;
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommandHandler(IRepository repository, IUnitOfWork unitOfWork, ILogger<UpdateCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Update(new Persistence.Models.Product
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
            Image = request.Image,
            BarCodeType = request.BarCodeType,
            Barcode = request.Barcode,
            Active = request.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound().WithMessage("Shopping Cart Not Found"),
            {Result.Value.Count: 0} => ResultsTo.NotFound().WithMessage("Shopping Cart not updated"),
            _ => ResultsTo.Something(result.Result.Value.Count > 0),
        };
    }
}
