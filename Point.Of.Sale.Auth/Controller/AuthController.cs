using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Point.Of.Sale.Auth.Handlers.Command.AddRoleCommand;
using Point.Of.Sale.Auth.Handlers.Command.AddUserRoleCommand;
using Point.Of.Sale.Auth.Handlers.Command.DeleteRoleCommand;
using Point.Of.Sale.Auth.Handlers.Command.DeleteUserCommand;
using Point.Of.Sale.Auth.Handlers.Command.RegisterUser;
using Point.Of.Sale.Auth.Handlers.Command.RemoveUserRoleCommand;
using Point.Of.Sale.Auth.Handlers.Command.UpdateRoleCommand;
using Point.Of.Sale.Auth.Handlers.Query.GetRoles;
using Point.Of.Sale.Auth.Handlers.Query.GetUserRoles;
using Point.Of.Sale.Auth.Handlers.Query.GetUsers;
using Point.Of.Sale.Auth.Handlers.Query.UserExistQuery;
using Point.Of.Sale.Auth.Handlers.Query.ValidateUserQuery;
using Point.Of.Sale.Auth.Models;
using Point.Of.Sale.Shared.Configuration;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Controller;

[ApiController]
[Authorize]
[Route("/api/auth/")]
public sealed class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IOptions<PosConfiguration> _options;
    private readonly ISender _sender;

    public AuthController(ISender sender, ILogger<AuthController> logger, IOptions<PosConfiguration> options)
    {
        _sender = sender;
        _logger = logger;
        _options = options;
    }

    //users
    [Route("users/register")]
    [HttpPost]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RegisterUserCommand
        {
            Email = request.Email,
            UserName = request.UserName,
            Password = request.Password,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Phone = request.Phone,
            TenantId = request.TenantId,
        }, cancellationToken);
        return result.ToActionResult();
    }

    [Route("users/{user}")]
    [HttpDelete]
    public async Task<ActionResult> DeleteUser(string user, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteUserCommand(user), cancellationToken);
        return result.ToActionResult();
    }

    [AllowAnonymous]
    [Route("users/login")]
    [HttpPost]
    public async Task<ActionResult> ValidateUser([FromBody] ValidateUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new ValidateUserQuery(request.UserName, request.Password, request.Email, request.TenantId), cancellationToken);

        return result.ToActionResult();
    }


    [Route("users/all")]
    [HttpGet]
    public async Task<ActionResult> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetUsersQuery(), cancellationToken);
        return result.ToActionResult();
    }

    [Route("users/exist")]
    [HttpPost]
    public async Task<ActionResult> UserExist([FromBody] UserExistRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UserExistQuery(request.UserName, request.Email, request.TenantId), cancellationToken);
        return result.ToActionResult();
    }

    //roles
    [Route("roles/{role}")]
    [HttpPost]
    public async Task<ActionResult> AddRole(string role, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new AddRoleCommand(role), cancellationToken);
        return result.ToActionResult();
    }

    [Route("roles/{role}")]
    [HttpDelete]
    public async Task<ActionResult> DeleteRole(string role, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteRoleCommand(role), cancellationToken);
        return result.ToActionResult();
    }

    [Route("roles")]
    [HttpPut]
    public async Task<ActionResult> DeleteUserRole([FromBody] UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateRoleCommand(request.RoleName, request.RoleNormalizeName), cancellationToken);
        return result.ToActionResult();
    }

    [Route("roles")]
    [HttpGet]
    public async Task<ActionResult> GetAllRoles(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetRolesQuery(), cancellationToken);
        return result.ToActionResult();
    }

    //user-roles
    [Route("user-roles/{user}")]
    [HttpGet]
    public async Task<ActionResult> GetUserRole(string user, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetUserRolesQuery(user), cancellationToken);
        return result.ToActionResult();
    }

    [Route("user-role/{user}/{role}")]
    [HttpPost]
    public async Task<ActionResult> AddUserRole(string user, string role, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new AddUserRoleCommand(user, role), cancellationToken);
        return result.ToActionResult();
    }

    [Route("user-role/{user}")]
    [HttpDelete]
    public async Task<ActionResult> DeleteUserRole([FromBody] string[] roles, string user, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new RemoveUserRoleCommand(user, roles), cancellationToken);
        return result.ToActionResult();
    }
}
