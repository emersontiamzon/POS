using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Query.GetUserRoles;

public class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, List<string>>
{
    private readonly ILogger<GetUserRolesQueryHandler> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public GetUserRolesQueryHandler(ILogger<GetUserRolesQueryHandler> logger, ISender sender, UserManager<ServiceUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _sender = sender;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IFluentResults<List<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
        {
            return ResultsTo.BadRequest<List<string>>("User is null or empty").WithMessage("Invalid argument provided.");
        }

        if (await _userManager.FindByNameAsync(request.UserName) is not { } user)
        {
            return ResultsTo.NotFound<List<string>>("User does not exist").WithMessage("Invalid argument provided.");
        }

        if (await _userManager.GetRolesAsync(user) is List<string> roles && roles.Any())
        {
            return ResultsTo.Success(roles);
        }

        return ResultsTo.NotFound<List<string>>("User does not have any roles").WithMessage("No roles found");
    }
}