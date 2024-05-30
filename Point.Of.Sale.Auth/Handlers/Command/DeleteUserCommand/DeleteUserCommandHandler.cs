using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Auth.Handlers.Query.GetUserRoles;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Command.DeleteUserCommand;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly ISender _sender;
    private readonly UserManager<ServiceUser> _userManager;

    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, ISender sender, UserManager<ServiceUser> userManager)
    {
        _logger = logger;
        _sender = sender;
        _userManager = userManager;
    }

    public async Task<IFluentResults<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.UserName) is not { } user)
        {
            return ResultsTo.BadRequest<bool>("User does not exist").WithMessage("Invalid argument provided.");
        }

        if (await _sender.Send(new GetUserRolesQuery(request.UserName), cancellationToken) is { } userRoles && userRoles.Value.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles.Value);
        }

        return ResultsTo.Success((await _userManager.DeleteAsync(user)).Succeeded);
    }
}