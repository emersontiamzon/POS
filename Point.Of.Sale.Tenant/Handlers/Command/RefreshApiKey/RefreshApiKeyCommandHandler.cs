using Microsoft.AspNetCore.JsonPatch;
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

namespace Point.Of.Sale.Tenant.Handlers.Command.RefreshApiKey;

public class RefreshApiKeyCommandHandler : ICommandHandler<RefreshApiKeyCommand, string>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly ILogger<RefreshApiKeyCommandHandler> _logging;
    private readonly IRepository _repository;

    public RefreshApiKeyCommandHandler(ILogger<RefreshApiKeyCommandHandler> logging, IOptions<PosConfiguration> configuration, IRepository repository)
    {
        _logging = logging;
        _configuration = configuration;
        _repository = repository;
    }

    public async Task<IFluentResults<string>> Handle(RefreshApiKeyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.TenantId, cancellationToken), _logging) is not {Result.Status: FluentResultsStatus.Success} tenant)
            {
                return ResultsTo.NotFound<string>().WithMessage("Tenant not found");
            }

            if ((tenant.Result.Value.TenantApiKey?.Trim() ?? string.Empty) != (!string.IsNullOrWhiteSpace(request.ApiKey.Trim()) ? request.ApiKey.Trim() : string.Empty))
            {
                return ResultsTo.BadRequest<string>().WithMessage("Api Key mismatched");
            }

            if (Parameters(tenant).GenerateToken() is { } apiKey && !string.IsNullOrWhiteSpace(apiKey))
            {
                var tenantApiKeyPatch = new JsonPatchDocument<Persistence.Models.Tenant>().Replace(t => t.TenantApiKey, apiKey);

                return ResultsTo.Something(await PosPolicies.ExecuteThenCaptureResult(() => _repository.Patch(tenant.Result.Value.Id, tenantApiKeyPatch, cancellationToken), _logging) is {Result.Value.Count: > 0}
                    ? apiKey
                    : string.Empty);
            }

            return ResultsTo.BadRequest<string>().WithMessage("Failed to generate api key");
        }
        catch (Exception e)
        {
            _logging.LogError(e, "Failed to generate api key");
            return ResultsTo.Failure<string>().FromException(e).WithMessage("Failed to generate api key");
        }
    }

    private TokenBuilderParameters Parameters(PolicyResult<IFluentResults<Persistence.Models.Tenant>> tenant)
    {
        return new TokenBuilderParameters
        {
            Claims = tenant.Result.Value.CreateClaims(),
            Configuration = _configuration.Value,
            ExpiresIn = TimeSpan.FromDays(3650),
        };
    }
}
