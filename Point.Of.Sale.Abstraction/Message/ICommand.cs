using MediatR;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Abstraction.Message;

public interface ICommand : IRequest<IFluentResults>
{
}

public interface ICommand<TResponse> : IRequest<IFluentResults<TResponse>>
{
}
