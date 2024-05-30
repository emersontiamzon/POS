using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Query.UserExistQuery;

public class UserExistQueryHandler : IQueryHandler<UserExistQuery, ServiceUser>
{
    private readonly ILogger<UserExistQueryHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public UserExistQueryHandler(ILogger<UserExistQueryHandler> logger, UserManager<ServiceUser> userManager, ISender sender)
    {
        _logger = logger;
        _userManager = userManager;
        _sender = sender;
    }

    public async Task<IFluentResults<ServiceUser>> Handle(UserExistQuery request, CancellationToken cancellationToken)
    {
        var result = await _userManager.FindByEmailAsync(request.Email);

        if (result is null || result.Active == false || result.TenantId != request.TenantId)
        {
            return ResultsTo.NotFound<ServiceUser>().WithMessage("User does not exist");
        }

        return ResultsTo.Something(result);
    }
}