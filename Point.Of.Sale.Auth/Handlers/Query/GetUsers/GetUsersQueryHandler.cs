using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Auth.Models;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Query.GetUsers;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<ServiceUserResponse>>
{
    private readonly UserManager<ServiceUser> _userManager;

    public GetUsersQueryHandler(UserManager<ServiceUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IFluentResults<List<ServiceUserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userManager.Users.ToListAsync(cancellationToken);

        if (!result.Any())
        {
            return ResultsTo.NotFound<List<ServiceUserResponse>>().WithMessage("No Users Found");
        }

        var response = result.Where(r => r.Active).Select(m => new ServiceUserResponse
        {
            FirstName = m.FirstName,
            MiddleName = m.MiddleName,
            LastName = m.LastName,
            UserName = m.UserName ?? string.Empty,
            TenantId = m.TenantId,
        }).ToList();

        return ResultsTo.Something(response);
    }
}
