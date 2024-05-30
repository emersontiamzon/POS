using MediatR;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Abstraction.Message;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, IFluentResults>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, IFluentResults<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
