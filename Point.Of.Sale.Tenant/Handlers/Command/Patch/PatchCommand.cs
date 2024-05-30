using Microsoft.AspNetCore.JsonPatch;
using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Tenant.Handlers.Command.Patch;

public sealed record PatchCommand : ICommand
{
    public int Id { get; set; }
    public JsonPatchDocument<Persistence.Models.Tenant> Patch { get; set; }
}