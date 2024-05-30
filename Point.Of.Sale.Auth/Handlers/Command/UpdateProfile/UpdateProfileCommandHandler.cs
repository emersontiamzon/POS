using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Auth.Handlers.Command.UpdateProfile;

public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, bool>
{
    public Task<IFluentResults<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
