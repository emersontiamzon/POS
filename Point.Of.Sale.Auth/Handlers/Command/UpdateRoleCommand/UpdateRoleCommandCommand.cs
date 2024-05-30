using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Command.UpdateRoleCommand;

public class UpdateRoleCommandCommand : ICommandHandler<UpdateRoleCommand, bool>
{
    private readonly ILogger<UpdateRoleCommandCommand> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ServiceUser> _userManager;

    public UpdateRoleCommandCommand(ILogger<UpdateRoleCommandCommand> logger, UserManager<ServiceUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IFluentResults<bool>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleManager.FindByNameAsync(request.RoleName) is not { } role)
        {
            return ResultsTo.BadRequest<bool>("Role does not exist").WithMessage("Invalid argument provided.");
        }

        role.Name = request.RoleName;
        role.NormalizedName = request.RoleNormalizeName;

        return ResultsTo.Something((await _roleManager.UpdateAsync(role)).Succeeded);
    }
}