using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;
using Polly;

namespace Point.Of.Sale.Tenant.Handlers.Command.RegisterTenant;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository, IOptions<PosConfiguration> configuration, ILogger<RegisterCommandHandler> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var tenant = TenantToRegister(request);

        tenant.TenantApiKey = Parameters(tenant).GenerateToken();

        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Add(tenant, cancellationToken), _logger);

        return result switch
        {
            {Result: null, Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            _ => ResultsTo.Something(result.Result!.Value),
        };
    }

    private static Persistence.Models.Tenant TenantToRegister(RegisterCommand request)
    {
        return new Persistence.Models.Tenant
        {
            Type = request.Type,
            Code = request.Code,
            Name = request.Name,
            Email = request.Email,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        };
    }

    private TokenBuilderParameters Parameters(Persistence.Models.Tenant tenant)
    {
        return new TokenBuilderParameters
        {
            Claims = tenant.CreateClaims(),
            Configuration = _configuration.Value,
            ExpiresIn = TimeSpan.FromDays(3650),
        };
    }
}
