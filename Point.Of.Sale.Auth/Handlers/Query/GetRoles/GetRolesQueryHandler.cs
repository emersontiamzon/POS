using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Query.GetRoles;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, List<string>>
{
    private readonly ILogger<GetRolesQueryHandler> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    public GetRolesQueryHandler(ILogger<GetRolesQueryHandler> logger, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<IFluentResults<List<string>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        if (await _roleManager.Roles.ToListAsync(cancellationToken) is { } result && result.Any())
        {
            return ResultsTo.Something(result.Select(m => m.Name!.Trim()).ToList());
        }

        return ResultsTo.NotFound<List<string>>().WithMessage("No roles found");
    }
}