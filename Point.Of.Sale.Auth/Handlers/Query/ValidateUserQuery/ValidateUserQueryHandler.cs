using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Extensions;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Query.ValidateUserQuery;

public class ValidateUserQueryHandler : IQueryHandler<ValidateUserQuery, string>
{
    private readonly IOptions<PosConfiguration> _configuration;
    private readonly ILogger<ValidateUserQueryHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public ValidateUserQueryHandler(ILogger<ValidateUserQueryHandler> logger, UserManager<ServiceUser> userManager, ISender sender, IOptions<PosConfiguration> configuration)
    {
        _logger = logger;
        _userManager = userManager;
        _sender = sender;
        _configuration = configuration;
    }

    public async Task<IFluentResults<string>> Handle(ValidateUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _sender.Send(new UserExistQuery.UserExistQuery(request.UserName, request.Email, request.TenantId), cancellationToken);
            var validated = await _userManager.CheckPasswordAsync(user.Value, request.Password);

            var parameters = BuildParameters(user.Value, _configuration.Value);
            var token = parameters.GenerateToken();

            return user.Status == FluentResultsStatus.NotFound
                ? ResultsTo.NotFound<string>().WithMessage("User not found.")
                : ResultsTo.Something(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user");
            return ResultsTo.Failure<string>().WithMessage(ex.Message);
        }
    }

    private static TokenBuilderParameters BuildParameters(ServiceUser user, PosConfiguration configuration)
    {
        return new TokenBuilderParameters
        {
            Claims = user.CreateClaims(),
            ExpiresIn = TimeSpan.FromHours(configuration.General.Environment == "Development" ? 24 : 2), // 2 Hours
            Configuration = configuration,
        };
    }
}