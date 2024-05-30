using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Command.RemoveUserRoleCommand;

public class RemoveUserRoleCommandHandler : ICommandHandler<RemoveUserRoleCommand, bool>
{
    private readonly ILogger<RemoveUserRoleCommandHandler> _logger;
    private readonly UserManager<ServiceUser> _userManager;

    public RemoveUserRoleCommandHandler(ILogger<RemoveUserRoleCommandHandler> logger, UserManager<ServiceUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IFluentResults<bool>> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        return user is null
            ? ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.")
            : ResultsTo.Something((await _userManager.RemoveFromRolesAsync(user, request.RoleName)).Succeeded);
    }
}